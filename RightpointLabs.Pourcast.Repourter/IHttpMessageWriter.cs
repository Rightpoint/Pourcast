namespace RightpointLabs.Pourcast.Repourter
{
    public interface IHttpMessageWriter
    {
        void SendStartAsync(int tapId);
        void SendStopAsync(int tapId, double ounces);
    }
}