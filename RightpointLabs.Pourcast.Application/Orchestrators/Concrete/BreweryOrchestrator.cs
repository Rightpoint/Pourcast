using System;
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

        public string Create(string name, string city, string state, string country, string postalCode, string website, string logo)
        {
            //TODO Check for existing brewery with the name
            var id = "";
            using (var scope = new TransactionScope())
            {
                id = _breweryRepository.NextIdentity();
                var brewery = new Brewery(id, name)
                {
                    City = city,
                    State = state,
                    Country = country,
                    PostalCode = postalCode,
                    Website = website,
                    Logo = logo
                };
                _breweryRepository.Add(brewery);
                scope.Complete();
            }

            return id;
        }

        public void Save(Brewery brewery)
        {
            _breweryRepository.Update(brewery);
        }


        public Brewery GetByName(string name)
        {
            return _breweryRepository.GetByName(name);
        }
    }
}