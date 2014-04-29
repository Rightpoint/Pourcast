namespace RightpointLabs.Pourcast.Application.Orchestrators.Abstract
{
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Domain.Models;

    public interface IBreweryOrchestrator
    {
        IEnumerable<Brewery> GetBreweries();
        Brewery GetById(string id);
        Brewery GetByName(string name);
        string Create(string name, string city, string state, string country, string postalCode, string website,
            string logo);
        void Save(Brewery brewery);
    }
}