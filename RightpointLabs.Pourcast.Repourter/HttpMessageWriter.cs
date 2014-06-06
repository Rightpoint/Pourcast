using System;
using System.Threading;
using Microsoft.SPOT;

namespace RightpointLabs.Pourcast.Repourter
{
    public class HttpMessageWriter : IHttpMessageWriter
    {
        private readonly IMessageSender _messageSender;
        private readonly string _baseUrl = "http://pourcast.labs.rightpoint.com/api/Tap/";

//http://pourcast.labs.rightpoint.com/api/Tap/535c61a951aa0405287989ec/StartPour
//http://pourcast.labs.rightpoint.com/api/Tap/535c61a951aa0405287989ec/StopPour?volume=xxxx

        public HttpMessageWriter(IMessageSender messageSender)
        {
            _messageSender = messageSender;
            StartThread();
        }

        public void SendStartAsync(string tapId)
        {
            Debug.Print("Queing: start " + tapId);
            _queue.Add(new Uri(_baseUrl + tapId + "/StartPour"));
        }

        public void SendStopAsync(string tapId, double ounces)
        {
            Debug.Print("Queing: stop " + tapId + " " + ounces);
            _queue.Add(new Uri(_baseUrl + tapId + "/StopPour?volume=" + ounces));
        }

        public void SendHeartbeatAsync()
        {
            Debug.Print("Queing: heartbeat");
            _queue.Add(new Uri(_baseUrl + "heartbeat"));
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
                    _messageSender.FetchURL(uri);
                }
                catch (ThreadAbortException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    Debug.Print("Couldn't fetch URL: " + uri.AbsoluteUri + ": " + ex.ToString());
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
                Debug.Print("Failed to join sending thread - aborting");
                _sendThread.Abort();
            }
        }
    }
}