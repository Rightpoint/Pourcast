namespace RightpointLabs.Pourcast.Domain.Models
{
    using System;

    using RightpointLabs.Pourcast.Domain.Events;

    public class Keg : Entity, IByOrganizationId
    {
        private Keg() { }

        public Keg(string id, string beerId, string kegTypeId)
            : base(id)
        {
            BeerId = beerId;
            KegTypeId = kegTypeId;

            DomainEvents.Raise(new KegCreated(id, beerId));
        }

        public string BeerId { get; private set; }

        public string KegTypeId { get; set; }

        public string LocationId { get; set; }

        public string OrganizationId { get; set; }

        public double PercentRemaining { get; set; }
    }
}