using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RightpointLabs.Pourcast.Domain.Events
{
    public class PictureTaken : IDomainEvent
    {
        public string DataUrl { get; set; }
        public string TapId { get; set;  }
    }
}
