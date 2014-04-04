namespace RightpointLabs.Pourcast.Domain.Models
{
    public class Tap
    {
        public Tap(TapName name)
        {
            Name = name;
        }

        public int TapId { get; set; }
        public TapName Name { get; set; }
    }
}