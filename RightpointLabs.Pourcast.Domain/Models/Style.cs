namespace RightpointLabs.Pourcast.Domain.Models
{
    public class Style : Entity
    {
        public Style(string id, string name)
            : base(id)
        {
            Name = name;
        }

        public string Name { get; set; }
        public string Color { get; set; }
        public string Glass { get; set; }
    }
}
