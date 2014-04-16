using System.Web.Optimization;

namespace RightpointLabs.Pourcast.Web.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;
            const string jquery = "http://ajax.googleapis.com/ajax/libs/jquery/1.11.0/jquery.min.js";
            const string jqueryUI = "http://ajax.googleapis.com/ajax/libs/jqueryui/1.10.4/jquery-ui.min.js";
            const string bootstrap = "http://ajax.aspnetcdn.com/ajax/bootstrap/3.1.1/bootstrap.min.js";
            const string modernizer = "http://ajax.aspnetcdn.com/ajax/modernizr/modernizr-2.7.2.js";
            const string knockout = "/Scripts/knockout-3.1.0.js";

            bundles.Add(new ScriptBundle("~/bundles/jquery", jquery).Include("~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryUI", jqueryUI).Include("~/Scripts/jquery-ui-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap", bootstrap).Include("~/Scripts/bootstrap.js"));
            bundles.Add(new ScriptBundle("~/bundles/knockout", knockout).Include("~/Scripts/knockout-{version}.debug.js"));
            bundles.Add(new ScriptBundle("~/bundles/modernizer", modernizer).Include("~/Scripts/modernizer-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/toastr").Include("~/Scripts/toastr.js"));
            bundles.Add(new ScriptBundle("~/bundles/moment").Include("~/Scripts/moment.js"));
            bundles.Add(new ScriptBundle("~/bundles/signalr").Include("~/Scripts/jquery.signalR-2.0.0.js"));
            bundles.Add(new ScriptBundle("~/bundles/app").Include("~/Scripts/app/pourcast.events.js", "~/Scripts/app/pourcast.beer.js", "~/Scripts/app/pourcast.brewery.js", "~/Scripts/app/pourcast.breweryvm.js", "~/Scripts/app/pourcast.js"));
            bundles.Add(new StyleBundle("~/Content/themes/base/css")
                .Include("~/Content/bootstrap.css",
                "~/Content/bootstrap-theme.css",
                "~/Content/Site.css",
                "~/Content/themes/base/*.css",
                "~/Content/toastr.css"));
        }
    }
}