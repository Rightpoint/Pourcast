#if NO_NETWORKING
using System;
using Microsoft.SPOT;

namespace RightpointLabs.Pourcast.Repourter
{
    public class NullMessageSender : IMessageSender
    {
        public void FetchURL(Uri url)
        {
            Debug.Print("Asked to fetch " + url.AbsoluteUri);
        }

        public void Initalize()
        {
        }
    }
}
#endif