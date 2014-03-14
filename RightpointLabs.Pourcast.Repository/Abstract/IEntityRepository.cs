using RightpointLabs.Pourcast.DataModel.Entities;

namespace RightpointLabs.Pourcast.Repository.Abstract
{
    public interface IEntityRepository<T> where T : IMongoEntity
    {
        void Create(T entity);
        void Delete(string id);
        T GetById(string id);
        void Update(T entity);
    }
}