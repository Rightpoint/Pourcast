namespace RightpointLabs.Pourcast.Domain.Models
{
    public abstract class Entity
    {
        protected Entity(string id)
        {
            Id = id;
        }

        public string Id { get; private set; }
    }
}
