using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace RightpointLabs.Pourcast.Infrastructure
{
    public class ServiceBusSetup
    {
        private readonly string _serviceBusConnectionString;

        public ServiceBusSetup(string serviceBusConnectionString)
        {
            _serviceBusConnectionString = serviceBusConnectionString;
        }

        public async Task Setup()
        {
            var mgr = NamespaceManager.CreateFromConnectionString(_serviceBusConnectionString);

            var eventSource = await mgr.TopicExistsAsync(Constants.TopicNames.EventSource)
                ? await mgr.GetTopicAsync(Constants.TopicNames.EventSource)
                : await mgr.CreateTopicAsync(Constants.TopicNames.EventSource);

            await ConfigureEventSourceLogger(mgr, eventSource);
            await ConfigureEventSourceSignalR(mgr, eventSource);
        }

        private async Task ConfigureEventSourceLogger(NamespaceManager mgr, TopicDescription eventSource)
        {
            var logger = await mgr.QueueExistsAsync(Constants.QueueNames.Logging)
                ? await mgr.GetQueueAsync(Constants.QueueNames.Logging)
                : await mgr.CreateQueueAsync(Constants.QueueNames.Logging);

            var eventSourceLogger = await mgr.SubscriptionExistsAsync(eventSource.Path, logger.Path)
                ? await mgr.GetSubscriptionAsync(eventSource.Path, logger.Path)
                : await mgr.CreateSubscriptionAsync(eventSource.Path, logger.Path);

            eventSourceLogger.ForwardTo = logger.Path;
            await mgr.UpdateSubscriptionAsync(eventSourceLogger);
            // no filters needed
        }

        private async Task ConfigureEventSourceSignalR(NamespaceManager mgr, TopicDescription eventSource)
        {
            var signalR = await mgr.QueueExistsAsync(Constants.QueueNames.SignalR)
                ? await mgr.GetQueueAsync(Constants.QueueNames.SignalR)
                : await mgr.CreateQueueAsync(Constants.QueueNames.SignalR);

            var signalRLogger = await mgr.SubscriptionExistsAsync(eventSource.Path, signalR.Path)
                ? await mgr.GetSubscriptionAsync(eventSource.Path, signalR.Path)
                : await mgr.CreateSubscriptionAsync(eventSource.Path, signalR.Path);

            signalRLogger.ForwardTo = signalR.Path;
            await mgr.UpdateSubscriptionAsync(signalRLogger);
            // no filters needed
        }
    }
}
