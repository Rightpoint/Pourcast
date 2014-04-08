using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;
using RightpointLabs.Pourcast.Web.App_Start;

namespace RightpointLabs.Pourcast.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
