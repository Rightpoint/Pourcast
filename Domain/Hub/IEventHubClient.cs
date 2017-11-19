using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace RightpointLabs.Pourcast.Domain.Hub
{
    public interface IEventHubClient
    {
        Task RecieveEvent(JObject obj);
    }
}
