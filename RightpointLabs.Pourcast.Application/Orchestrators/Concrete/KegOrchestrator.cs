namespace RightpointLabs.Pourcast.Application.Orchestrators.Concrete
{
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class KegOrchestrator : BaseOrchestrator, IKegOrchestrator
    {
        private readonly IKegRepository _kegRepository;

        public KegOrchestrator(IKegRepository kegRepository)
        {
            _kegRepository = kegRepository;
        }

         public IEnumerable<Keg> GetAll()
         {
             return _kegRepository.GetAll();
         }

        public IEnumerable<Keg> GetOnTap()
        {
            return _kegRepository.OnTap();
        }
    }
}