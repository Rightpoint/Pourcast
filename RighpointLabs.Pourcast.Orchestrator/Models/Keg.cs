using System.Collections.Generic;
using RightpointLabs.Pourcast.DataModel.Entities;

namespace RighpointLabs.Pourcast.Orchestrator.Models
{
    public class Keg
    {
        public string Id { get; set; }
        public Beer Beer { get; set; }
        public Status Status { get; set; }
        public Tap Tap { get; set; }
        public IEnumerable<Pour> Pours { get; set; }
 
    }
}