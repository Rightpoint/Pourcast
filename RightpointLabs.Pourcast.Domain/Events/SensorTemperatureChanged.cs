namespace RightpointLabs.Pourcast.Domain.Events
{
    public class SensorTemperatureChanged : IDomainEvent
    {
        public string SensorId { get; private set; }

        public double TemperatureF { get; private set; }

        public SensorTemperatureChanged(string sensorId, double temperatureF)
        {
            SensorId = sensorId;
            TemperatureF = temperatureF;
        }
    }
}