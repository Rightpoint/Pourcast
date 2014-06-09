using System.Collections;
using System.Collections.Generic;
using RightpointLabs.Pourcast.Domain.Models;

namespace RightpointLabs.Pourcast.Application.Orchestrators.Abstract
{
    public interface IStyleOrchestrator
    {
        Style CreateStyle(string name, string color, string glass);
        Style GetStyleById(string id);
        Style GetStyleByColor(string hexValue);
        IEnumerable<Style> GetStyles();
        IEnumerable<Style> GetStylesByName(string name);
        void Save(Style style);
    }
}
