using System;

namespace RightpointLabs.Pourcast.Domain.Services
{
    public interface IDateTimeProvider
    {
        DateTime GetCurrentDateTime();
    }
}
