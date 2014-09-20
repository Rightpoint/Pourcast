using Microsoft.SPOT;

namespace RightpointLabs.Pourcast.Repourter
{
    public class Logger : ILogger
    {
        private IHttpMessageWriter _writer = null;

        public void SetWriter(IHttpMessageWriter writer)
        {
            _writer = writer;
        }

        public void Log(string message)
        {
            Debug.Print(message);
            if (null != _writer)
            {
                _writer.SendLogMessageAsync(message);
            }
        }
    }
}