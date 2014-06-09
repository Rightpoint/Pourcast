using System;

namespace RightpointLabs.Pourcast.Repourter
{
    public interface IMessageSender
    {
        void FetchURL(Uri url);
        void Initalize();
    }
}