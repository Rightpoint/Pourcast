using System.Collections.Generic;
using AutoMapper;
using RighpointLabs.Pourcast.Orchestrator.Abstract;
using RighpointLabs.Pourcast.Orchestrator.Models;
using RightpointLabs.Pourcast.Repository.Abstract;

namespace RighpointLabs.Pourcast.Orchestrator.Concrete
{
    public class BreweryOrchestrator : BaseOrchestrator, IBreweryOrchestrator
    {
        private readonly IBreweryRepository _breweryRepository;

        public BreweryOrchestrator(IBreweryRepository breweryRepository)
        {
            _breweryRepository = breweryRepository;
        }

        public IEnumerable<Brewery> GetBreweries()
        {
            var results = _breweryRepository.GetAll();
            return Mapper.Map<IEnumerable<Models.Brewery>>(results);
        }
    }
}