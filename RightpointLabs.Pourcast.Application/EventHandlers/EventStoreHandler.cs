namespace RightpointLabs.Pourcast.Application.EventHandlers
{
    using System;

    using RightpointLabs.Pourcast.Domain.Events;
    using RightpointLabs.Pourcast.Domain.Repositories;
    using RightpointLabs.Pourcast.Domain.Services;

    public class EventStoreHandler<T> : TransactionDependantEventHandler<T> where T : IDomainEvent
    {
        private readonly IStoredEventRepository _storedEventRepository;

        private readonly IDateTimeProvider _dateTimeProvider;

        public EventStoreHandler(IStoredEventRepository storedEventRepository, IDateTimeProvider dateTimeProvider)
        {
            if (storedEventRepository == null) throw new ArgumentNullException("storedEventRepository");
            if (dateTimeProvider == null) throw new ArgumentNullException("dateTimeProvider");

            _storedEventRepository = storedEventRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        protected override void HandleAfterTransaction(T domainEvent)
        {
            var id = _storedEventRepository.NextIdentity();
            var occuredOn = _dateTimeProvider.GetCurrentDateTime();

            var storedEvent = new StoredEvent(id, occuredOn, domainEvent);

            _storedEventRepository.Add(storedEvent);
        }
    }
}
