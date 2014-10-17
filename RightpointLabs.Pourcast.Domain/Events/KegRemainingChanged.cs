namespace RightpointLabs.Pourcast.Domain.Events
{
    public class KegRemainingChanged : IDomainEvent
    {
        public string KegId { get; private set; }

        public double PercentRemaining { get; private set; }

        public KegRemainingChanged(string kegId, double percentRemaining)
        {
            KegId = kegId;
            PercentRemaining = percentRemaining;
        }
    }
}
