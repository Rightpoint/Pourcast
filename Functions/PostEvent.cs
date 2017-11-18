using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ServiceBus.Messaging;
using RightpointLabs.Pourcast.Infrastructure;

namespace RightpointLabs.Pourcast.Functions
{
    public static class PostEvent
    {
        static PostEvent()
        {
            // setup job
            Task.Run(async () =>
                await new ServiceBusSetup(ConfigurationManager.AppSettings["AzureWebJobsServiceBus"]).Setup()).Wait();
        }

        [FunctionName("PostEvent")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req, 
            [ServiceBus(Constants.TopicNames.EventSource, AccessRights.Send)]IAsyncCollector<BrokeredMessage> eventSource, 
            TraceWriter log)
        {
            // get data
            var rawData = await req.Content.ReadAsFormDataAsync();

            // build and send message
            var msg = new BrokeredMessage();
            foreach (string key in rawData)
            {
                msg.Properties[key] = rawData[key];
            }
            await eventSource.AddAsync(msg);

            // give the OK back to the client
            return req.CreateResponse(HttpStatusCode.OK);
        }
    }
}
