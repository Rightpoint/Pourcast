namespace RightpointLabs.Pourcast.Infrastructure.Data.Repositories
{
    using AutoMapper;

    using RightpointLabs.Pourcast.Domain.Repositories;
    using RightpointLabs.Pourcast.Domain.Models;

    public class BreweryRepository : EntityRepository<Brewery, Entities.Brewery>, IBreweryRepository
    {
        public BreweryRepository(IMongoConnectionHandler<Entities.Brewery> connectionHandler)
            : base(connectionHandler)
        {
            Mapper.CreateMap<Entities.Brewery, Brewery>()
                .ConstructUsing(b => new Brewery(b.Name));

            Mapper.CreateMap<Brewery, Entities.Brewery>();
        }
    }
}