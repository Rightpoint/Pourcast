namespace RightpointLabs.Pourcast.Infrastructure.Services
{
    using System;

    using RightpointLabs.Pourcast.Domain.Services;

    public class CurrentDateTimeProvider : IDateTimeProvider
    {
        public DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }
    }
}