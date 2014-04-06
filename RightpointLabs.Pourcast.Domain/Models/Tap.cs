namespace RightpointLabs.Pourcast.Domain.Models
{
    public class Tap : Entity
    {
        public Tap(TapName name)
        {
            Name = name;
        }

        public TapName Name { get; set; }
    }
}