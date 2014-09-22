namespace RightpointLabs.Pourcast.Web.Controllers.Api
{
    using System.Web.Http;

    using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;

    public class StatusController : ApiController
    {
        private readonly IStatusOrchestrator _statusOrchestrator;

        public StatusController(IStatusOrchestrator statusOrchestrator)
        {
            _statusOrchestrator = statusOrchestrator;
        }

        [HttpGet]
        public void Heartbeat()
        {
            _statusOrchestrator.Heartbeat();
        }

        [HttpGet]
        public void LogMessage(string message)
        {
            _statusOrchestrator.LogMessage(message);
        }
    }
}