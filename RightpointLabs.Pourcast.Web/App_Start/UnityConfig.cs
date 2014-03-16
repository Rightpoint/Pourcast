using System.Web.Http;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using RighpointLabs.Pourcast.Orchestrator.Abstract;
using RighpointLabs.Pourcast.Orchestrator.Concrete;
using RightpointLabs.Pourcast.DataModel;
using RightpointLabs.Pourcast.Repository.Abstract;
using RightpointLabs.Pourcast.Repository.Concrete;
using RightpointLabs.Pourcast.Web.App_Start;
using RightpointLabs.Pourcast.Web.Controllers;
using Unity.Mvc5;

namespace RightpointLabs.Pourcast.Web
{
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
        }
    }
}