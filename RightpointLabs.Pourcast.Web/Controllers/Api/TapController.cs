using System;
using System.Collections.Generic;
using System.Web.Http;

namespace RightpointLabs.Pourcast.Web.Controllers.Api
{
    using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;
    using RightpointLabs.Pourcast.Domain.Models;

    public class TapController : ApiController
    {
        private readonly ITapOrchestrator _tapOrchestrator;

        public TapController(ITapOrchestrator tapOrchestrator)
        {
            if (tapOrchestrator == null) throw new ArgumentNullException("tapOrchestrator");

            _tapOrchestrator = tapOrchestrator;
        }

        public IEnumerable<Tap> Get()
        {
            return _tapOrchestrator.GetTaps();
        }

        public Tap Get(string id)
        {
            return _tapOrchestrator.GetTapById(id);
        }

        [HttpPost]
        public void StartPour(string id)
        {
            _tapOrchestrator.StartPourFromTap(id);
        }

        [HttpPost]
        public void EndPour([FromUri]string id, [FromUri]double volume)
        {
            _tapOrchestrator.EndPourFromTap(id, volume);
        }
    }
}