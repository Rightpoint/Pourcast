using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;
using RightpointLabs.Pourcast.Domain.Hub;

namespace RightpointLabs.Pourcast.Web
{
    public class EventsHub : Hub<IEventHubClient>, IEventHubServer
    {
        public async Task PublishEvent(JObject obj)
        {
            await Clients.All.RecieveEvent(obj);
        }
    }
}