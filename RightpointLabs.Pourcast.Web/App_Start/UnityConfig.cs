using Microsoft.Practices.Unity;

namespace RightpointLabs.Pourcast.Web
{
    using System;
    using System.Net.Mail;

    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Infrastructure;
    using Microsoft.Practices.ServiceLocation;

    using RightpointLabs.Pourcast.Application.EventHandlers;
    using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;
    using RightpointLabs.Pourcast.Application.Orchestrators.Concrete;
    using RightpointLabs.Pourcast.Domain.Events;
    using RightpointLabs.Pourcast.Domain.Repositories;
    using RightpointLabs.Pourcast.Domain.Services;
    using RightpointLabs.Pourcast.Infrastructure.Data;
    using RightpointLabs.Pourcast.Infrastructure.Data.Repositories;
    using RightpointLabs.Pourcast.Infrastructure.Services;
    using RightpointLabs.Pourcast.Web.SignalR;

    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        public static void RegisterTypes(IUnityContainer container)
        {
            var connectionString =
                System.Web.Configuration.WebConfigurationManager.ConnectionStrings["Mongo"].ConnectionString;
            var database = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["Mongo"].ProviderName;

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IMongoConnectionHandler, MongoConnectionHandler>(
                new PerRequestLifetimeManager(),
                new InjectionConstructor(connectionString, database));

            // orchestrators
            container.RegisterType<IBreweryOrchestrator, BreweryOrchestrator>(new PerRequestLifetimeManager());
            container.RegisterType<IBeerOrchestrator, BeerOrchestrator>(new PerRequestLifetimeManager());
            container.RegisterType<IKegOrchestrator, KegOrchestrator>(new PerRequestLifetimeManager());
            container.RegisterType<ITapOrchestrator, TapOrchestrator>(new PerRequestLifetimeManager());

            // repositories
            container.RegisterType<IKegRepository, KegRepository>(new PerRequestLifetimeManager());
            container.RegisterType<IBeerRepository, BeerRepository>(new PerRequestLifetimeManager());
            container.RegisterType<IBreweryRepository, BreweryRepository>(new PerRequestLifetimeManager());
            container.RegisterType<ITapRepository, TapRepository>(new PerRequestLifetimeManager());
            container.RegisterType<IStoredEventRepository, StoredEventRepository>(new PerRequestLifetimeManager());

            // domain services
            container.RegisterType<IEmailService, SmtpEmailService>(new PerRequestLifetimeManager());
            container.RegisterType<IDateTimeProvider, CurrentDateTimeProvider>(new ContainerControlledLifetimeManager());

            // event handlers (must be named!)
            container.RegisterType(typeof(IEventHandler<>), typeof(EventStoreHandler<>), "EventStore", new PerRequestLifetimeManager());
            container.RegisterType<IEventHandler<BeerPourStopped>, KegNearingEmptyNotificationHandler>("KegNearingEmptyNotification", new PerRequestLifetimeManager());
            container.RegisterType<IEventHandler<KegEmptied>, KegEmptiedNotificationHandler>("KegEmptiedNotification", new PerRequestLifetimeManager());
            
            // signalr event handlers
            container.RegisterType<IEventHandler<BeerPourStarted>, BeerPourStartedClientHandler>("BeerPourStartedClientHandler", new PerRequestLifetimeManager());
            container.RegisterType<IEventHandler<BeerPourStopped>, BeerPourEndedClientHandler>("BeerPourStoppedClientHandler", new PerRequestLifetimeManager());

            // misc
            container.RegisterType<SmtpClient>(new PerRequestLifetimeManager(), new InjectionConstructor());
            container.RegisterType<IConnectionManager>(new ContainerControlledLifetimeManager(), new InjectionFactory(c => GlobalHost.ConnectionManager));

            var locator = new App_Start.UnityServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => locator);
        }
    }
}