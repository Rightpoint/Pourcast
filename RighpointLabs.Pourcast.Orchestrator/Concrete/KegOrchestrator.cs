using System.Collections.Generic;
using AutoMapper;
using RighpointLabs.Pourcast.Orchestrator.Abstract;
using RighpointLabs.Pourcast.Orchestrator.Models;
using RightpointLabs.Pourcast.Repository.Abstract;
using DM = RightpointLabs.Pourcast.DataModel.Entities;

namespace RighpointLabs.Pourcast.Orchestrator.Concrete
{
    public class KegOrchestrator : BaseOrchestrator, IKegOrchestrator
    {
        private readonly IKegRepository _kegRepository;

        public KegOrchestrator(IKegRepository kegRepository)
        {
            _kegRepository = kegRepository;
        }

         public IEnumerable<Keg> GetAll()
         {

             var source = _kegRepository.GetAll();
             return Mapper.Map<IEnumerable<Keg>>(source);
         }

        public IEnumerable<Keg> GetOnTap()
        {
            return Mapper.Map<IEnumerable<Keg>>(_kegRepository.OnTap());
        }
    }
}