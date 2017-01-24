namespace RightpointLabs.Pourcast.Application.Orchestrators.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;
    using RightpointLabs.Pourcast.Application.Transactions;
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class KegOrchestrator : BaseOrchestrator, IKegOrchestrator
    {
        private readonly IKegRepository _kegRepository;

        public KegOrchestrator(IKegRepository kegRepository)
        {
            if (kegRepository == null) throw new ArgumentNullException(nameof(kegRepository));

            _kegRepository = kegRepository;
        }

        //public IEnumerable<Keg> GetKegs()
        //{
        //    return _kegRepository.GetAll();
        //}

        //public IEnumerable<Keg> GetKegs(bool isEmpty)
        //{
        //    return _kegRepository.GetAll().Where(k => k.IsEmpty == isEmpty);
        //} 

        public Keg GetKeg(string kegId, string organizationId)
        {
            return _kegRepository.GetById(kegId, organizationId);
        }

        //public IEnumerable<Keg> GetKegsOnTap()
        //{
        //    var taps = _tapRepository.GetAll();
        //    var kegs = taps.Select(t => _kegRepository.GetById(t.KegId));

        //    return kegs;
        //}

        //public Keg GetKegOnTap(string tapId)
        //{
        //    var tap = _tapRepository.GetById(tapId);
        //    var keg = _kegRepository.GetById(tap.KegId);

        //    return keg;
        //}

        [Transactional]
        public string CreateKeg(string beerId, string kegTypeId, string organizationId)
        {
            var id = _kegRepository.NextIdentity();
            var keg = new Keg(id, beerId, kegTypeId);
            keg.OrganizationId = organizationId;

            _kegRepository.Insert(keg);

            return id;
        }

        //[Transactional]
        //public void UpdateCapacityAndPoured(string kegId, double capacity, double amountOfBeerPoured)
        //{
        //    var keg = _kegRepository.GetById(kegId);
        //    keg.UpdateCapacityAndPoured(capacity, amountOfBeerPoured);
        //    _kegRepository.Update(keg);
        //}
    }
}