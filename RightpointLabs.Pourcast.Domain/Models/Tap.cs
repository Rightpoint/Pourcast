namespace RightpointLabs.Pourcast.Domain.Models
{
    public class Tap : Entity
    {
        public Tap(string id, TapName name)
            : base(id)
        {
            Name = name;
        }

        public TapName Name { get; set; }
    }
}