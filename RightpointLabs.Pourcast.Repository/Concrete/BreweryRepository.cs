using System.Collections.Generic;
using System.Linq;
using RightpointLabs.Pourcast.DataModel;
using RightpointLabs.Pourcast.DataModel.Entities;
using RightpointLabs.Pourcast.Repository.Abstract;
using Slugify;

namespace RightpointLabs.Pourcast.Repository.Concrete
{
    public class BreweryRepository : EntityRepository<Brewery>, IBreweryRepository
    {
        private readonly SlugHelper _slug;

        public BreweryRepository(IMongoConnectionHandler<Brewery> connectionHandler) : base(connectionHandler)
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