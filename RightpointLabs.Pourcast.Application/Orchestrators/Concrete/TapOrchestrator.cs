namespace RightpointLabs.Pourcast.Application.Orchestrators.Concrete
{
    using System;
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;
    using RightpointLabs.Pourcast.Application.Transactions;
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class TapOrchestrator : ITapOrchestrator
    {
        private readonly ITapRepository _tapRepository;

        private readonly IKegRepository _kegRepository;

        public TapOrchestrator(ITapRepository tapRepository, IKegRepository kegRepository)
        {
            if (tapRepository == null) throw new ArgumentNullException(nameof(tapRepository));
            if (kegRepository == null) throw new ArgumentNullException(nameof(kegRepository));

            _tapRepository = tapRepository;
            _kegRepository = kegRepository;
        }

        public Tap GetTapById(string id, string organizationId)
        {
            return _tapRepository.GetById(id, organizationId);
        }

        public Tap GetByName(string name, string organizationId)
        {
            return _tapRepository.GetByName(name, organizationId);
        }

        public IEnumerable<Tap> GetTaps(string organizationId)
        {
            return _tapRepository.GetAll(organizationId);
        }

        //[Transactional]
        //public void StartPourFromTap(string tapId)
        //{
        //    var tap = _tapRepository.GetById(tapId);
        //    var keg = _kegRepository.GetById(tap.KegId);

        //    keg.StartPourFromTap(tap.Id);

        //    _kegRepository.Update(keg);
        //}

        //[Transactional]
        //public void PouringFromTap(string tapId, double volume)
        //{
        //    var tap = _tapRepository.GetById(tapId);
        //    var keg = _kegRepository.GetById(tap.KegId);

        //    keg.PouringFromTap(tap.Id, volume);

        //    _kegRepository.Update(keg);
        //}

        //[Transactional]
        //public void StopPourFromTap(string tapId, double volume)
        //{
        //    var tap = _tapRepository.GetById(tapId);
        //    var keg = _kegRepository.GetById(tap.KegId);

        //    keg.StopPourFromTap(tap.Id, volume);

        //    _kegRepository.Update(keg);
        //}

        //[Transactional]
        //public void RemoveKegFromTap(string tapId)
        //{
        //    var tap = _tapRepository.GetById(tapId);

        //    tap.RemoveKeg();

        //    _tapRepository.Update(tap);
        //}

        //[Transactional]
        //public void TapKeg(string tapId, string kegId)
        //{
        //    var tap = _tapRepository.GetById(tapId);
        //    var keg = _kegRepository.GetById(kegId);

        //    tap.TapKeg(keg.Id);

        //    _tapRepository.Update(tap);
        //}

        [Transactional]
        public string CreateTap(string name, string organizationId)
        {
            var id = _tapRepository.NextIdentity();
            var tap = new Tap(id, name);
            tap.OrganizationId = organizationId;
            _tapRepository.Insert(tap);

            return id;
        }

        //[Transactional]
        //public string CreateTap(string name, string kegId)
        //{
        //    var id = _tapRepository.NextIdentity();
        //    var tap = new Tap(id, name);
        //    _tapRepository.Insert(tap);
        //    tap.TapKeg(kegId);
        //    return id;
        //}

        [Transactional]
        public void Save(Tap tap)
        {
            _tapRepository.Update(tap);
        }

        //[Transactional]
        //public void UpdateTemperature(string tapId, double temperatureF)
        //{
        //    var tap = _tapRepository.GetById(tapId);
        //    var keg = _kegRepository.GetById(tap.KegId);

        //    keg.UpdateTemperature(tap.Id, temperatureF);

        //    _kegRepository.Update(keg);
        //}
    }
}