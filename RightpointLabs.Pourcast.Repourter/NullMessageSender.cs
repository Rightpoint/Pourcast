#if NO_NETWORKING
using System;
using System.Threading;
using Microsoft.SPOT;

namespace RightpointLabs.Pourcast.Repourter
{
    public class NullMessageSender : IMessageSender
    {
        public void FetchURL(Uri url)
        {
            Debug.Print("Asked to fetch " + url.AbsoluteUri);
            Thread.Sleep(400);
        }

        public void Initalize()
        {
        }
    }
}
#endif