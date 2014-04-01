namespace RightpointLabs.Pourcast.Infrastructure.Data.Repositories
{
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;

    using Slugify;

    public class BeerRepository : EntityRepository<Beer, Entities.Beer>, IBeerRepository
    {
        private readonly SlugHelper _slug;

        public BeerRepository(IMongoConnectionHandler<Entities.Beer> connectionHandler)
            : base(connectionHandler)
        {
        }

        public override void Create(Beer entity)
        {
            entity.Slug = _slug.GenerateSlug(entity.Name);
            base.Create(entity);
        }

        public override void Update(Beer entity)
        {
            throw new System.NotImplementedException();
        }
    }
}