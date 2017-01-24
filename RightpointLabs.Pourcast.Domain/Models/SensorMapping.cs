namespace RightpointLabs.Pourcast.Domain.Models
{
    public class SensorMapping : Entity, IByOrganizationId
    {
        private SensorMapping() { }

        public SensorMapping(string id)
            : base(id)
        {
        }

        public string SensorId { get; set; }

        public string OrganizationId { get; set; }

        public string TapId { get; set; }

        public string LocationId { get; set; }
    }
}