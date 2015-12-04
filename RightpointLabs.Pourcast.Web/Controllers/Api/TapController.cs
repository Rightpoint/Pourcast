using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;
using log4net;

namespace RightpointLabs.Pourcast.Web.Controllers.Api
{
    using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;
    using RightpointLabs.Pourcast.Domain.Models;

    public class TapController : ApiController
    {
        private static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);  
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
            try
            {
                _tapOrchestrator.StartPourFromTap(id);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("StartPour({0})", id), ex);
                throw;
            }
        }

        [HttpGet]
        public void Pouring([FromUri]string id, [FromUri]double volume)
        {
            try
            {
                _tapOrchestrator.PouringFromTap(id, volume);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Pouring({0}, {1})", id, volume), ex);
                throw;
            }
        }

        [HttpGet]
        public void StopPour([FromUri]string id, [FromUri]double volume)
        {
            try
            {
                _tapOrchestrator.StopPourFromTap(id, volume);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("StopPour({0}, {1})", id, volume), ex);
                throw;
            }
        }

        [HttpGet]
        public void Temperature([FromUri]string id, [FromUri]double f)
        {
            try
            {
                _tapOrchestrator.UpdateTemperature(id, f);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Temperature({0}, {1})", id, f), ex);
                throw;
            }
        }
    }
}