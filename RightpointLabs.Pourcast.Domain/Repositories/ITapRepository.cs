namespace RightpointLabs.Pourcast.Domain.Repositories
{
    using RightpointLabs.Pourcast.Domain.Models;

    public interface ITapRepository
    {
        Tap GetById(string id);
    }
}
