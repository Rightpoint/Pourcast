using System;

namespace RightpointLabs.Pourcast.Domain.Models
{
    public class Sensor : Entity, IByOrganizationId
    {
        private Sensor() { }

        public Sensor(string id)
            : base(id)
        {
        }

        public string DeviceId { get; set; }

        public string OrganizationId { get; set; }

        public SensorType Type { get; set; }

        public DateTime LastData { get; set; }
    }
}