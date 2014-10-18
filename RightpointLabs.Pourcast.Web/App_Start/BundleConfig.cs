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
            const string bootstrap = "//netdna.bootstrapcdn.com/bootstrap/3.1.1/js/bootstrap.min.js";
            const string modernizer = "http://ajax.aspnetcdn.com/ajax/modernizr/modernizr-2.7.2.js";
            const string knockout = "/Scripts/libs/knockout-3.2.0.js";

            bundles.Add(new ScriptBundle("~/bundles/jquery", jquery).Include("~/Scripts/libs/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryUI", jqueryUI).Include("~/Scripts/libs/jquery-ui-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap", bootstrap).Include("~/Scripts/libs/bootstrap.js"));
            bundles.Add(new ScriptBundle("~/bundles/knockout", knockout).Include("~/Scripts/libs/knockout-{version}.debug.js"));
            bundles.Add(new ScriptBundle("~/bundles/modernizer", modernizer).Include("~/Scripts/libs/modernizer-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/toastr").Include("~/Scripts/libs/toastr.js"));
            bundles.Add(new ScriptBundle("~/bundles/moment").Include("~/Scripts/libs/moment.js"));
            bundles.Add(new ScriptBundle("~/bundles/signalr").Include("~/Scripts/libs/jquery.signalR-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/app")
                .Include("~/Scripts/app/model/brewery.js",
                "~/Scripts/app/model/beer.js",
                "~/Scripts/app/model/keg.js",
                "~/Scripts/app/model/tap.js",
                "~/Scripts/app/model/style.js",
                "~/Scripts/app/events.js",
                "~/Scripts/app/bindings.js", 
                "~/Scripts/app/bubbles.js", 
                "~/Scripts/app/pourcast.js",
                "~/Scripts/app.js"));

            bundles.Add(new StyleBundle("~/Content/css/bootstrap", "//netdna.bootstrapcdn.com/bootstrap/3.1.1/css/bootstrap.min.css")
                .Include("~/Content/bootstrap.css"));

            bundles.Add(new StyleBundle("~/Content/css/bootstrap-theme", "//netdna.bootstrapcdn.com/bootstrap/3.1.1/css/bootstrap-theme.min.css")
                .Include("~/Content/bootstrap-theme.css"));

            bundles.Add(new StyleBundle("~/Content/css")
                .Include("~/Content/toastr.css",
                "~/Content/pourcast.css"));
        }
    }
}