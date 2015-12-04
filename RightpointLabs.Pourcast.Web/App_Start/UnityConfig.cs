using System.Configuration;
using System.Drawing;
using System.IO;
using System.Web;
using Glimpse.AspNet.Tab;
using Microsoft.Practices.Unity;

namespace RightpointLabs.Pourcast.Web
{
    using System;
    using System.Linq;
    using System.Net.Mail;

    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Infrastructure;
    using Microsoft.Practices.ServiceLocation;
    using Microsoft.Practices.Unity.InterceptionExtension;

    using RightpointLabs.Pourcast.Application.EventHandlers;
    using RightpointLabs.Pourcast.Domain.Events;
    using RightpointLabs.Pourcast.Domain.Services;
    using RightpointLabs.Pourcast.Infrastructure.Persistence;
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
            container.AddNewExtension<Interception>();

            var connectionString =
                System.Web.Configuration.WebConfigurationManager.ConnectionStrings["Mongo"].ConnectionString;
            var database = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["Mongo"].ProviderName;

            container.RegisterType<IMongoConnectionHandler, MongoConnectionHandler>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(connectionString, database));

            // orchestrators
            container.RegisterTypes(
                AllClasses.FromLoadedAssemblies().Where(
                  t => t.Namespace == "RightpointLabs.Pourcast.Application.Orchestrators.Concrete"),
                WithMappings.FromAllInterfaces,
                WithName.Default,
                WithLifetime.Custom<PerRequestLifetimeManager>,
                getInjectionMembers: t => new InjectionMember[]
                {
                    new InterceptionBehavior<PolicyInjectionBehavior>(),
                    new Interceptor<InterfaceInterceptor>()
                });

            // repositories
            container.RegisterTypes(
                AllClasses.FromLoadedAssemblies().Where(
                  t => t.Namespace == "RightpointLabs.Pourcast.Infrastructure.Persistence.Repositories"),
                WithMappings.FromAllInterfaces,
                WithName.Default,
                WithLifetime.Custom<PerRequestLifetimeManager>,
                getInjectionMembers: t => new InjectionMember[]
                {
                    new InterceptionBehavior<PolicyInjectionBehavior>(),
                    new Interceptor<InterfaceInterceptor>()
                });
            container.RegisterTypes(
                AllClasses.FromLoadedAssemblies().Where(
                  t => t.Namespace == "RightpointLabs.Pourcast.Infrastructure.Persistence.Collections"),
                WithMappings.FromAllInterfaces,
                WithName.Default,
                WithLifetime.Custom<ContainerControlledLifetimeManager>);

            // domain services
            container.RegisterType<IEmailService, SmtpEmailService>(new PerRequestLifetimeManager());
            container.RegisterType<IDateTimeProvider, CurrentDateTimeProvider>(new ContainerControlledLifetimeManager());
            container.RegisterType<IMessagePoster, YammerMessagePoster>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    new InjectionParameter<string>(ConfigurationManager.AppSettings["YammerApiCode"]),
                    new InjectionParameter<int>(int.Parse(ConfigurationManager.AppSettings["YammerGroupId"] ?? "0"))));
            container.RegisterType<IFaceRecognitionService, FaceRecognitionService>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    new InjectionParameter<string>(ConfigurationManager.AppSettings["SkyBiometryApiKey"]),
                    new InjectionParameter<string>(ConfigurationManager.AppSettings["SkyBiometryApiSecret"]),
                    new InjectionParameter<string>(ConfigurationManager.AppSettings["SkyBiometryTagNamespace"]),
                    typeof(IImageCleanupService),
                    typeof(IOverlayImageProvider)));
            container.RegisterType<IImageCleanupService, ImageCleanupService>(new PerRequestLifetimeManager());

            // StateTracker
            container.RegisterType<StateTracker>(new ContainerControlledLifetimeManager());
            container.RegisterType<IEventHandler<PictureTaken>, StateTracker.StateTrackerEventHandler>("StateTrackerEventHandler_PictureTaken", new PerRequestLifetimeManager());
            container.RegisterType<IEventHandler<PourStopped>, StateTracker.StateTrackerEventHandler>("StateTrackerEventHandler_PourStopped", new PerRequestLifetimeManager());

            // event handlers (must be named!)
            container.RegisterType(typeof(IEventHandler<>), typeof(EventStoreHandler<>), "EventStore", new PerRequestLifetimeManager());
            container.RegisterType<IEventHandler<PourStopped>, KegNearingEmptyNotificationHandler>("KegNearingEmptyNotification", new PerRequestLifetimeManager());
            container.RegisterType<IEventHandler<KegRemainingChanged>, KegNearingEmptyNotificationHandler>("KegNearingEmptyNotification", new PerRequestLifetimeManager());
            container.RegisterType<IEventHandler<KegEmptied>, KegEmptiedNotificationHandler>("KegEmptiedNotification", new PerRequestLifetimeManager());

            // signalr event handlers (must be named!)
            container.RegisterType<IEventHandler<PourStarted>, PourStartedClientHandler>("BeerPourStartedClientHandler", new PerRequestLifetimeManager());
            container.RegisterType<IEventHandler<Pouring>, PouringClientHandler>("BeerPouringClientHandler", new PerRequestLifetimeManager());
            container.RegisterType<IEventHandler<PourStopped>, PourStoppedClientHandler>("BeerPourStoppedClientHandler", new PerRequestLifetimeManager());
            container.RegisterType<IEventHandler<KegRemainingChanged>, KegRemainingChangedClientHandler>("KegRemainingChangedClientHandler", new PerRequestLifetimeManager());
            container.RegisterType<IEventHandler<KegRemovedFromTap>, KegRemovedFromTapClientHandler>("KegRemovedFromTapClientHandler", new PerRequestLifetimeManager());
            container.RegisterType<IEventHandler<KegTapped>, KegTappedClientHandler>("KegTappedClientHandler", new PerRequestLifetimeManager());
            container.RegisterType<IEventHandler<KegTemperatureChanged>, KegTemperatureChangedClientHandler>("KegTemperatureChangedClientHandler", new PerRequestLifetimeManager());
            container.RegisterType<IEventHandler<Heartbeat>, HeartbeatClientHandler>("HeartbeatClientHandler", new PerRequestLifetimeManager());
            container.RegisterType<IEventHandler<LogMessage>, LogMessageClientHandler>("LogMessageClientHandler", new PerRequestLifetimeManager());
            container.RegisterType<IEventHandler<PictureTaken>, PictureTakenClientHandler>("PictureTakenClientHandler", new PerRequestLifetimeManager());
            container.RegisterType<IEventHandler<PictureRequested>, PictureRequestedClientHandler>("PictureRequestedClientHandler", new PerRequestLifetimeManager());

            // misc
            container.RegisterType<SmtpClient>(new PerRequestLifetimeManager(), new InjectionConstructor());
            container.RegisterType<IConnectionManager>(new ContainerControlledLifetimeManager(), new InjectionFactory(c => GlobalHost.ConnectionManager));

            container.RegisterType<IOverlayImageProvider, OverlayImageProvider>(new ContainerControlledLifetimeManager());

            var locator = new App_Start.UnityServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => locator);
        }
    }

    public class OverlayImageProvider : IOverlayImageProvider
    {
        private Random _rnd = new Random();
        public Image GetRandomOverlayImage()
        {
            var dir = HttpContext.Current.Server.MapPath("~/OverlayImages");
            if (!Directory.Exists(dir))
            {
                return null;
            }
            var files = Directory.GetFiles(dir, "*");
            if (files.Length == 0)
            {
                return null;
            }
            return Image.FromFile(files[_rnd.Next(files.Length)]);
        }
    }
}