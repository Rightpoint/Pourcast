using System;
using RightpointLabs.Pourcast.Domain.Models;

namespace RightpointLabs.Pourcast.Application.Payloads.Analytics
{
    public class Pour
    {
        public DateTime OccuredOn { get; set; }
        public Tap Tap { get; set; }
        public Keg Keg { get; set; }
        public Beer Beer { get; set; }
        public double PercentRemaining { get; set; }
        public double Volume { get; set; }
    }
}