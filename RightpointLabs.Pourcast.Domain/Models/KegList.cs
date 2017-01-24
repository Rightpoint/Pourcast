using System;

namespace RightpointLabs.Pourcast.Domain.Models
{
    public class KegList : Entity, IByOrganizationId
    {
        private KegList() { }
        public KegList(string id)
            : base(id)
        {
        }

        public string OrganizationId { get; set; }

        public class Item
        {
            public string KegId { get; set; }
            public DateTime? Start { get; set; }
            public DateTime? End { get; set; }
        }

        public Item[] History { get; set; }

        public Item[] Schedule { get; set; }
    }
}
