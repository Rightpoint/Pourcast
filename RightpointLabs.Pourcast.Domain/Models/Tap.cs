namespace RightpointLabs.Pourcast.Domain.Models
{
    public class Tap
    {
        public Tap(TapName name)
        {
            Name = name;
        }

        public string TapId { get; set; }
        public TapName Name { get; set; }
    }
}