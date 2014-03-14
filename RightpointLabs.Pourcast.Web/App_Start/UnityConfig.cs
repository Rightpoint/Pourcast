using System.Web.Mvc;
using Microsoft.Practices.Unity;
using RightpointLabs.Pourcast.DataModel;
using Unity.Mvc5;

namespace RightpointLabs.Pourcast.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
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
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}