namespace RightpointLabs.Pourcast.Domain.Events
{
    public class KegTemperatureChanged : IDomainEvent
    {
        public string KegId { get; private set; }

        public double TemperatureF { get; private set; }

        public KegTemperatureChanged(string kegId, double temperatureF)
        {
            KegId = kegId;
            TemperatureF = temperatureF;
        }
    }
}