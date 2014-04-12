using System;

namespace RightpointLabs.Pourcast.Application.EventHandlers
{
    using RightpointLabs.Pourcast.Domain.Events;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class BeerPouredEventHandler: IEventHandler<BeerPoured>
    {
        private readonly IKegRepository _kegRepository;

        public BeerPouredEventHandler(IKegRepository kegRepository)
        {
            if (kegRepository == null) throw new ArgumentNullException("kegRepository");

            _kegRepository = kegRepository;
        }

        public void Handle(BeerPoured domainEvent)
        {
            var keg = _kegRepository.GetById(domainEvent.KegId);

            keg.PourBeer(domainEvent.Volume);
        }
    }
}
