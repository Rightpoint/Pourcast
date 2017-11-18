using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ServiceBus.Messaging;
using RightpointLabs.Pourcast.Infrastructure;

namespace RightpointLabs.Pourcast.Functions
{
    public static class LogEvent
    {
        [FunctionName("LogEvent")]
        public static void Run(
            [ServiceBusTrigger(Constants.QueueNames.Logging, AccessRights.Listen)]BrokeredMessage msg, 
            TraceWriter log)
        {
            log.Info($"C# ServiceBus queue trigger function processed message: {msg}");
        }
    }
}
