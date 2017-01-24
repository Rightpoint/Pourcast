namespace RightpointLabs.Pourcast.Domain.Models
{
    public class KegType : Entity
    {
        public string Name { get; set; }
        public double Capacity { get; set; }
        public double EmptyWeight { get; set; }
        public double FullWeight { get; set; }
    }
}