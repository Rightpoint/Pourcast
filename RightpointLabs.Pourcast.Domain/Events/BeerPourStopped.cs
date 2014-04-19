namespace RightpointLabs.Pourcast.Domain.Events
{
    public class BeerPourStopped : IDomainEvent
    {
        public string TapId { get; private set; }

        public string KegId { get; private set; }

        public double Volume { get; private set; }

        public double PercentRemaining { get; private set; }

        public BeerPourStopped(string tapId, string kegId, double volume, double percentRemaining)
        {
            TapId = tapId;
            KegId = kegId;
            Volume = volume;
            PercentRemaining = percentRemaining;
        }
    }
}
