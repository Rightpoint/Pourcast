using System;
using System.Collections.Generic;
using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;
using RightpointLabs.Pourcast.Domain.Models;
using RightpointLabs.Pourcast.Domain.Repositories;

namespace RightpointLabs.Pourcast.Application.Orchestrators.Concrete
{
    public class StyleOrchestrator : BaseOrchestrator, IStyleOrchestrator
    {
        private readonly IStyleRepository _styleRepository;

        public StyleOrchestrator(IStyleRepository styleRepository)
        {
            if(null == styleRepository)
                throw new ArgumentNullException(nameof(styleRepository));

            _styleRepository = styleRepository;
        }

        public Style CreateStyle(string name, string color, string glass)
        {
            if(string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            if(string.IsNullOrEmpty(color))
                throw new ArgumentNullException(nameof(color));
            if(string.IsNullOrEmpty(glass))
                throw new ArgumentNullException(nameof(glass));

            var id = _styleRepository.NextIdentity();
            var style = new Style(id, name);
            style.Color = color;
            style.Glass = glass;
            _styleRepository.Insert(style);
            return style;
        }

        public void Save(Style style)
        {
            _styleRepository.Update(style);
        }

        public IEnumerable<Style> GetStyles()
        {
            return _styleRepository.GetAll();
        }

        public Style GetStyleById(string id)
        {
            return _styleRepository.GetById(id);
        }
    }
}