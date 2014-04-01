namespace RightpointLabs.Pourcast.Domain.Repositories
{
    using RightpointLabs.Pourcast.Domain.Models;

    public interface IBeerRepository
    {
        Beer GetById(string id);
    }
}