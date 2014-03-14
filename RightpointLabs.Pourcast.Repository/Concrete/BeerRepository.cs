using MongoDB.Driver.Linq;
using System.Linq;
using RightpointLabs.Pourcast.DataModel;
using RightpointLabs.Pourcast.Repository.Abstract;
using RightpointLabs.Pourcast.DataModel.Entities;
using Slugify;

namespace RightpointLabs.Pourcast.Repository.Concrete
{
    public class BeerRepository : EntityRepository<Beer>, IBeerRepository
    {
        private readonly SlugHelper _slug;

        public BeerRepository(IMongoConnectionHandler<Beer> connectionHandler) : base(connectionHandler)
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