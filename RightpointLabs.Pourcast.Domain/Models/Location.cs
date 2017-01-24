namespace RightpointLabs.Pourcast.Domain.Models
{
    public class Location : Entity, IByOrganizationId
    {
        private Location() { }

        public Location(string id, string name)
            : base(id)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public string OrganizationId { get; set; }

        public string ParentLocationId { get; set; }

        public string[] UserDomains { get; set; }

        public string[] AdminUserIds { get; set; }
    }
}