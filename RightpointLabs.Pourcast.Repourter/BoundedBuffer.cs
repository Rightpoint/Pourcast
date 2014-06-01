using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;

namespace RightpointLabs.Pourcast.Repourter
{
    /// <summary>
    /// http://staceyw1.wordpress.com/2011/02/02/consumer-producer-in-netmf-using-a-boundedbuffer/
    /// </summary>
    public class BoundedBuffer
    {
        private Queue queue = new Queue();
        private int consumersWaiting;
        private int producersWaiting;
        private const int maxBufferSize = 128;
        private AutoResetEvent are = new AutoResetEvent(false);

        public int Count
        {
            get { return queue.Count; }
        }

        public void Add(object obj)
        {
            Monitor.Enter(queue);
            try
            {
                while (queue.Count == (maxBufferSize - 1))
                {
                    producersWaiting++;
                    Monitor.Exit(queue);
                    are.WaitOne();
                    Monitor.Enter(queue);
                    producersWaiting--;
                }
                queue.Enqueue(obj);
                if (consumersWaiting > 0)
                    are.Set();
            }
            finally
            {
                Monitor.Exit(queue);
            }
        }

        public object Take()
        {
            object item;
            Monitor.Enter(queue);
            try
            {
                while (queue.Count == 0)
                {
                    consumersWaiting++;
                    Monitor.Exit(queue);
                    are.WaitOne();
                    Monitor.Enter(queue);
                    consumersWaiting--;
                }
                item = queue.Dequeue();
                if (producersWaiting > 0)
                    are.Set();
            }
            finally
            {
                Monitor.Exit(queue);
            }
            return item;
        }
    }
}
