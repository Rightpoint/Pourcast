using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.SignalR.Internal.Protocol;
using Microsoft.AspNetCore.Sockets.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json.Linq;
using RightpointLabs.Pourcast.Infrastructure;

namespace RightpointLabs.Pourcast.Functions
{
    public static class SignalREvent
    {
        [FunctionName("SignalREvent")]
        public static async Task Run(
            [ServiceBusTrigger(Constants.QueueNames.SignalR, AccessRights.Listen)]BrokeredMessage msg, 
            TraceWriter log)
        {
            var builder = new HubConnectionBuilder().WithUrl(new Uri(ConfigurationManager.AppSettings["SignalREventHubUrl"]));
            var cn = builder.Build();
            await cn.StartAsync();

            var obj = JObject.FromObject(msg.Properties);
            await cn.InvokeAsync("PublishEvent", obj);

            await cn.DisposeAsync();
            log.Info($"Message sent");
        }
    }
}
