using System.Web.Http;

namespace RightpointLabs.Pourcast.Web.Controllers.Api
{
    using Microsoft.AspNet.SignalR.Infrastructure;

    using RightpointLabs.Pourcast.Web.SignalR;

    public class RepourterTestController : ApiController
    {
        private readonly IConnectionManager _connectionManager;

        public RepourterTestController(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        [HttpPost]
        public void StartPour(string id)
        {
            var context = _connectionManager.GetHubContext<EventsHub>();

            context.Clients.All.StartPour(new { tapId = id });
        }

        [HttpPost]
        public void StopPour([FromUri]string id, [FromUri]double volume)
        {
            var context = _connectionManager.GetHubContext<EventsHub>();

            context.Clients.All.StopPour(new { tapId = id, volume });
        }
    }
}