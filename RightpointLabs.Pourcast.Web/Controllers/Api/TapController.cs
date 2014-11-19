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

        [HttpGet]
        public void Heartbeat()
        {
        }

        [HttpGet]
        public void LogMessage(string message)
        {
        }

        [HttpGet]
        public void StartPour(string id)
        {
            _tapOrchestrator.StartPourFromTap(id);
        }

        [HttpGet]
        public void Pouring([FromUri]string id, [FromUri]double volume)
        {
            _tapOrchestrator.PouringFromTap(id, volume);
        }

        [HttpGet]
        public void StopPour([FromUri]string id, [FromUri]double volume)
        {
            _tapOrchestrator.StopPourFromTap(id, volume);
        }

        [HttpGet]
        public void Temperature([FromUri]string id, [FromUri]double f)
        {
            _tapOrchestrator.UpdateTemperature(id, f);
        }
    }
}