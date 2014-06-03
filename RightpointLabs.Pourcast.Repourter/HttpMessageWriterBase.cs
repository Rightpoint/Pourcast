using System;
using System.Threading;
using Microsoft.SPOT;

namespace RightpointLabs.Pourcast.Repourter
{
    public abstract class HttpMessageWriterBase : IHttpMessageWriter
    {
        private readonly Watchdog _watchdog;

        protected HttpMessageWriterBase(Watchdog watchdog)
        {
            _watchdog = watchdog;
        }

        public void SendStartAsync(int tapId)
        {
            Debug.Print("Queing: start " + tapId);
            _queue.Add(new Message() { IsStart = true, TapId = tapId });
        }

        public void SendStopAsync(int tapId, double ounces)
        {
            Debug.Print("Queing: stop " + tapId + " " + ounces);
            _queue.Add(new Message() { IsStart = false, TapId = tapId, Volume = ounces });
        }

        public void SendHeartbeatAsync()
        {
            Debug.Print("Queing: heartbeat");
            _queue.Add(new Message() { IsHeartbeat = true});
        }

        protected readonly BoundedBuffer _queue = new BoundedBuffer();
        private Thread _sendThread = null;

        protected class Message
        {
            public int TapId { get; set; }
            public double Volume { get; set; }
            public bool IsStart { get; set; }
            public bool IsHeartbeat { get; set; }
        }

        protected void SendMessages()
        {
            while (true)
            {
                var msg = (Message)_queue.Take();
                if (null == msg)
                    return;
                try
                {
                    _watchdog.Reset();
                    if (msg.IsHeartbeat)
                        continue;
                    SendMessage(msg);
                }
                catch (ThreadAbortException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    Debug.Print("Couldn't send message: " + ex.ToString());
                }
            }
        }

        protected abstract void SendMessage(Message message);

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