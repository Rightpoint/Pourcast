using System;
using Microsoft.SPOT;

namespace RightpointLabs.Pourcast.Repourter
{
    public abstract class HttpMessageWriterBase : IHttpMessageWriter
    {
        public void SendStartAsync(int tapId)
        {
            _queue.Add(new Message() { IsStart = true, TapId = tapId });
        }

        public void SendStopAsync(int tapId, double ounces)
        {
            _queue.Add(new Message() { IsStart = true, TapId = tapId, Volume = ounces });
        }

        protected readonly BoundedBuffer _queue = new BoundedBuffer();
        protected class Message
        {
            public int TapId { get; set; }
            public double Volume { get; set; }
            public bool IsStart { get; set; }
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
                    SendMessage(msg);
                }
                catch (Exception ex)
                {
                    Debug.Print("Couldn't send message: " + ex.ToString());
                }
            }
        }

        protected abstract void SendMessage(Message message);
    }
}