namespace RightpointLabs.Pourcast.Infrastructure.Data.Repositories
{
    using RightpointLabs.Pourcast.Domain.Repositories;
    using RightpointLabs.Pourcast.Domain.Models;

    using Slugify;

    public class BreweryRepository : EntityRepository<Brewery, Entities.Brewery>, IBreweryRepository
    {
        private readonly SlugHelper _slug;

        public BreweryRepository(IMongoConnectionHandler<Entities.Brewery> connectionHandler)
            : base(connectionHandler)
        {
            _slug = new SlugHelper();
        }

        public override void Create(Brewery entity)
        {
            entity.Slug = _slug.GenerateSlug(entity.Name);
            base.Create(entity);
        }

        public override void Update(Brewery entity)
        {
            throw new System.NotImplementedException();
        }
    }
}