using Glimpse.Core.ClientScript;
using Microsoft.AspNet.SignalR;
using Microsoft.Practices.ServiceLocation;
using RightpointLabs.Pourcast.Domain.Events;

namespace RightpointLabs.Pourcast.Web.SignalR
{
    public class EventsHub : Hub
    {
        public void PictureTaken(string dataUrl)
        {
            DomainEvents.Raise(new PictureTaken(){ DataUrl = dataUrl });
        }
    }
}