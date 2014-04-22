using RightpointLabs.Pourcast.Application.Commands;

namespace RightpointLabs.Pourcast.Application.Orchestrators.Concrete
{
    using System.Collections.Generic;
    using System.Transactions;

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

        public string Create(CreateBrewery breweryCommand)
        {
            //TODO Check for existing brewery with the name

            var id = "";

            using (var scope = new TransactionScope())
            {
                id = _breweryRepository.NextIdentity();
                var brewery = new Brewery(id, breweryCommand.Name)
                {
                    City = breweryCommand.City,
                    State = breweryCommand.State,
                    Country = breweryCommand.Country,
                    PostalCode = breweryCommand.PostalCode,
                    Website = breweryCommand.Website,
                    Logo = breweryCommand.Logo
                };

                _breweryRepository.Add(brewery);

                scope.Complete();
            }

            return id;
        }


        public EditBrewery EditBrewery(string breweryId)
        {
            var brewery = _breweryRepository.GetById(breweryId);
            if (brewery == null) return null;

            return new EditBrewery()
            {
                Id = brewery.Id,
                Name = brewery.Name,
                City = brewery.City,
                State = brewery.State,
                Country = brewery.Country,
                PostalCode = brewery.PostalCode,
                Website = brewery.Website,
                Logo = brewery.Logo
            };
        }

        public void EditBrewery(EditBrewery editBreweryCommand)
        {
            var brewery = _breweryRepository.GetById(editBreweryCommand.Id);
            //TODO Check for existing brewery name
            brewery.Name = editBreweryCommand.Name;
            brewery.City = editBreweryCommand.City;
            brewery.State = editBreweryCommand.State;
            brewery.Country = editBreweryCommand.Country;
            brewery.PostalCode = editBreweryCommand.PostalCode;
            brewery.Website = editBreweryCommand.Website;
            brewery.Logo = editBreweryCommand.Logo;
            _breweryRepository.Update(brewery);
        }
    }
}