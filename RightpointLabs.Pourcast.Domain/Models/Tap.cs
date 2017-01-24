namespace RightpointLabs.Pourcast.Domain.Models
{
    public class Tap : Entity, IByOrganizationId
    {
        private Tap() { }

        public Tap(string id, string name)
            : base(id)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public string OrganizationId { get; set; }

        public string KegId { get; private set; }
    }
}