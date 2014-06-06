namespace RightpointLabs.Pourcast.Repourter
{
    public interface IHttpMessageWriter
    {
        void SendStartAsync(string tapId);
        void SendStopAsync(string tapId, double ounces);
    }
}