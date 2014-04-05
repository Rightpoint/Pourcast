namespace RightpointLabs.Pourcast.Infrastructure.Data.Repositories
{
    using RightpointLabs.Pourcast.Domain.Repositories;
    using RightpointLabs.Pourcast.Domain.Models;

    public class BreweryRepository : EntityRepository<Brewery, Entities.Brewery>, IBreweryRepository
    {
        public BreweryRepository(IMongoConnectionHandler<Entities.Brewery> connectionHandler)
            : base(connectionHandler)
        {
            AutoMapper.Mapper.CreateMap<Entities.Brewery, Brewery>();
            AutoMapper.Mapper.CreateMap<Brewery, Entities.Brewery>();
        }
    }
}