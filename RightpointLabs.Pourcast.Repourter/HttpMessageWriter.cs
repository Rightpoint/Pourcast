using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace RightpointLabs.Pourcast.Repourter
{
    public class HttpMessageWriter : IHttpMessageWriter
    {
        //http://pourcast.labs.rightpoint.com/api/Tap/535c61a951aa0405287989ec/StartPour
        //http://pourcast.labs.rightpoint.com/api/Tap/535c61a951aa0405287989ec/StopPour?volume=xxxx

        private readonly IMessageSender _messageSender;
        private readonly string _baseUrl;
        private readonly OutputPort _light;
        private readonly ILogger _logger;
        private readonly Watchdog[] _resetWatchdogsOnSend;

        public HttpMessageWriter(IMessageSender messageSender, string baseUrl, OutputPort light, ILogger logger, Watchdog[] resetWatchdogsOnSend)
        {
            _messageSender = messageSender;
            _baseUrl = baseUrl;
            _light = light;
            _logger = logger;
            _resetWatchdogsOnSend = resetWatchdogsOnSend ?? new Watchdog[] {};
            StartThread();
        }

        public void SendStartAsync(string tapId)
        {
            Debug.Print("Queing: start " + tapId);
            _queue.Add(new Uri(_baseUrl + "Tap/" + tapId + "/StartPour"));
        }

        public void SendPouringAsync(string tapId, double ounces)
        {
            Debug.Print("Queing: pouring " + tapId + " " + ounces);
            _queue.Add(new Uri(_baseUrl + "Tap/" + tapId + "/Pouring?volume=" + ounces));
        }

        public void SendStopAsync(string tapId, double ounces)
        {
            Debug.Print("Queing: stop " + tapId + " " + ounces);
            _queue.Add(new Uri(_baseUrl + "Tap/" + tapId + "/StopPour?volume=" + ounces));
        }

        public void SendHeartbeatAsync()
        {
            Debug.Print("Queing: heartbeat");
            _queue.Add(new Uri(_baseUrl + "Status/heartbeat"));
            var result = Debug.GC(true);
            _logger.Log("GC complete, free bytes: " + result);
        }

        public void SendLogMessageAsync(string message)
        {
            Debug.Print("Queing: logMessage");
            _queue.Add(new Uri(_baseUrl + "Status/logMessage?message=" + Toolbox.NETMF.Tools.RawUrlEncode(message)));
        }

        protected readonly BoundedBuffer _queue = new BoundedBuffer();
        private Thread _sendThread = null;

        protected void SendMessages()
        {
            while (true)
            {
                var uri = (Uri)_queue.Take();
                if (null == uri)
                    return;
                try
                {
                    _light.Write(false);
                    _messageSender.FetchURL(uri);
                }
                catch (ThreadAbortException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.Log("Couldn't fetch URL: " + uri.AbsoluteUri + ": " + ex.ToString());
                }
                finally
                {
                    _light.Write(true);
                    foreach(var wd in _resetWatchdogsOnSend)
                    {
                        wd.Reset();
                    }
                }
            }
        }

        protected void StartThread()
        {
            _sendThread = new Thread(SendMessages);
            _sendThread.Start();
        }

        public void Stop()
        {
            _queue.Add(null);
            if(!_sendThread.Join(1000))
            {
                _logger.Log("Failed to join sending thread - aborting");
                _sendThread.Abort();
            }
        }
    }
}