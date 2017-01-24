namespace RightpointLabs.Pourcast.Domain.Models
{
    public class Organization : Entity
    {
        private Organization() { }

        public Organization(string id, string name)
            : base(id)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public string[] UserDomains { get; set; }

        public string[] AdminUserIds { get; set; }
    }
}