namespace RightpointLabs.Pourcast.Infrastructure.Persistance.Repositories
{
    public interface IEntityRepository<T>
    {
        void Add(T entity);
        void Delete(string id);
        T GetById(string id);
        void Update(T entity);
    }
}