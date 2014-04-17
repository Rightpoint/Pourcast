using RightpointLabs.Pourcast.Application.Commands;

namespace RightpointLabs.Pourcast.Application.Orchestrators.Concrete
{
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class BreweryOrchestrator : BaseOrchestrator, IBreweryOrchestrator
    {
        private readonly IBreweryRepository _breweryRepository;

        public BreweryOrchestrator(IBreweryRepository breweryRepository)
        {
            _breweryRepository = breweryRepository;
        }

        public IEnumerable<Brewery> GetBreweries()
        {
            return _breweryRepository.GetAll();
        }

        public Brewery GetById(string id)
        {
            return _breweryRepository.GetById(id);
        }


        public void Create(CreateBrewery breweryCommand)
        {
            //TODO Check for existing brewery with the name

            var brewery = new Brewery(_breweryRepository.NextIdentity(), breweryCommand.Name)
            {
                City = breweryCommand.City,
                State = breweryCommand.State,
                Country = breweryCommand.Country,
                PostalCode = breweryCommand.PostalCode,
                Website = breweryCommand.Website,
                Logo = breweryCommand.Logo
            };
            _breweryRepository.Create(brewery);
        }
    }
}