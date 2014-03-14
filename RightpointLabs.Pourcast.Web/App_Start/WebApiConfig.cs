using System.Linq;
using System.Web.Http;

namespace RightpointLabs.Pourcast.Web
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration configuration)
        {
            configuration.Routes.MapHttpRoute("API Default", "api/{controller}/{id}", new {id = RouteParameter.Optional});

            var appXmlType =
                configuration.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(
                    t => t.MediaType == "application/xml");
            configuration.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
        } 
    }
}