namespace RightpointLabs.Pourcast.Domain.Repositories
{
    public interface IRepository
    {
        void Init();
        string NextIdentity();
    }
}
