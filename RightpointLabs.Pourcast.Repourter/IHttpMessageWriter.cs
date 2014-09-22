namespace RightpointLabs.Pourcast.Repourter
{
    public interface IHttpMessageWriter
    {
        void SendStartAsync(string tapId);
        void SendPouringAsync(string tapId, double ounces);
        void SendStopAsync(string tapId, double ounces);
        void SendLogMessageAsync(string message);
    }
}