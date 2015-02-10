using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RightpointLabs.Pourcast.Domain.Models
{
    public class TapNotificationState : Entity
    {
        private TapNotificationState()
        {
        }

        public TapNotificationState(string id) : base(id)
        {
            
        }

        public string TapId { get; set;  }
        public string KegId { get; set; }
        public DateTime Today { get; set; }
        public int? TodaysNotificationThreadId { get; set; }
        public double? TodaysBiggestPour { get; set; }
    }
}
