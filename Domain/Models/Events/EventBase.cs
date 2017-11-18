using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RightpointLabs.Pourcast.Domain.Models.Events
{
    public class EventBase
    {
        public string Id { get; set; }
        public string SourceId { get; set; }
    }
}
