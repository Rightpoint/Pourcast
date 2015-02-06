using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;
using RightpointLabs.Pourcast.Web.App_Start;

namespace RightpointLabs.Pourcast.Web
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            InitLogging();

            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var formatters = GlobalConfiguration.Configuration.Formatters;
            var jsonFormatter = formatters.JsonFormatter;
            var settings = jsonFormatter.SerializerSettings;
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        private void InitLogging()
        {
            // initialize log4net
            var file = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config");
            if (System.IO.File.Exists(file))
            {
                log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(file));
            }
        }
    }
}
