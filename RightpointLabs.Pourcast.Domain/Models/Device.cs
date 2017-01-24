namespace RightpointLabs.Pourcast.Domain.Models
{
    public class Device : Entity, IByOrganizationId
    {
        private Device() { }

        public Device(string id, string name)
            : base(id)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public string IotDeviceId { get; set; }

        public string OrganizationId { get; set; }
    }
}