namespace RightpointLabs.Pourcast.Infrastructure.Data.Repositories
{
    using AutoMapper;

    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class BeerRepository : EntityRepository<Beer, Entities.Beer>, IBeerRepository
    {
        public BeerRepository(IMongoConnectionHandler<Entities.Beer> connectionHandler)
            : base(connectionHandler)
        {
            Mapper.CreateMap<Entities.Beer, Beer>()
                .ConstructUsing(b => new Beer(b.Name));

            Mapper.CreateMap<Beer, Entities.Beer>();
        }
    }
}