namespace RightpointLabs.Pourcast.Domain.Events
{
    public class LogMessage : IDomainEvent
    {
        public string Message { get; private set; }

        public LogMessage(string message)
        {
            Message = message;
        }
    }
}