namespace RightpointLabs.Pourcast.Infrastructure.Data.Repositories
{
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class BeerRepository : EntityRepository<Beer, Entities.Beer>, IBeerRepository
    {
        public BeerRepository(IMongoConnectionHandler<Entities.Beer> connectionHandler)
            : base(connectionHandler)
        {
            AutoMapper.Mapper.CreateMap<Entities.Beer, Beer>();
            AutoMapper.Mapper.CreateMap<Beer, Entities.Beer>();
        }
    }
}