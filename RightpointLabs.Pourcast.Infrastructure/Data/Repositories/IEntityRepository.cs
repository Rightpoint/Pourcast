namespace RightpointLabs.Pourcast.Infrastructure.Data.Repositories
{
    public interface IEntityRepository<T>
    {
        void Add(T entity);
        void Delete(string id);
        T GetById(string id);
        void Update(T entity);
    }
}