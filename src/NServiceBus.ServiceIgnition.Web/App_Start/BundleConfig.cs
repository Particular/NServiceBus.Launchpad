using System.Web;
using System.Web.Optimization;

namespace NServiceBus.ServiceIgnition.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/libraries").Include(
                      //"~/Scripts/bootstrap.js",
                      "~/Scripts/angular.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/javascript").Include(
                      "~/Angular/*.js",
                      "~/Angular/Services/*.js",
                      "~/Angular/Controllers/*.js",
                      "~/Angular/Directives/*.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
