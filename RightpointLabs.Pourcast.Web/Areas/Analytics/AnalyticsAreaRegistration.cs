using System.Web.Mvc;

namespace RightpointLabs.Pourcast.Web.Areas.Analytics
{
    public class AnalyticsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Analytics";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Analytics_default",
                "Analytics/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new string[] { "RightpointLabs.Pourcast.Web.Areas.Analytics.Controllers" }
            );
        }
    }
}