using System.Web.Http;

using Microsoft.Practices.Unity;

using RightpointLabs.Pourcast.Web.App_Start;

namespace RightpointLabs.Pourcast.Web
{
    using System.Web.Mvc;

    using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;
    using RightpointLabs.Pourcast.Application.Orchestrators.Concrete;
    using RightpointLabs.Pourcast.Domain.Repositories;
    using RightpointLabs.Pourcast.Infrastructure.Data;
    using RightpointLabs.Pourcast.Infrastructure.Data.Repositories;

    public static class UnityConfig
    {
        public static void RegisterComponents(HttpConfiguration config)
        {
			var container = new UnityContainer();
            var connectionString =
                System.Web.Configuration.WebConfigurationManager.ConnectionStrings["Mongo"].ConnectionString;
            var database = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["Mongo"].ProviderName;
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType(typeof(IMongoConnectionHandler<>), typeof(MongoConnectionHandler<>),
                new InjectionConstructor(connectionString, database));
            container.RegisterType<IKegRepository, KegRepository>();
            container.RegisterType<IBeerRepository, BeerRepository>();
            container.RegisterType<IBreweryRepository, BreweryRepository>();
            container.RegisterType<IBreweryOrchestrator, BreweryOrchestrator>();
            container.RegisterType<IKegOrchestrator, KegOrchestrator>();


            config.DependencyResolver = new UnityResolver(container);
            DependencyResolver.SetResolver(new UnityResolver(container));
        }
    }
}