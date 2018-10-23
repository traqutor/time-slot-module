using System.Web;
using System.Web.Optimization;

namespace TimeSlotting
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                      "~/Scripts/Angular/angular.min.js",
                      "~/Scripts/Angular/moment-with-locales.min.js",
                      "~/Scripts/Angular/angular-moment-picker.min.js",
                      "~/Scripts/Angular/angular-resource.min.js",
                      "~/Scripts/Controllers/_init.js",
                      "~/Scripts/Controllers/common.js",
                      "~/Scripts/Controllers/users.js",
                      "~/Scripts/Controllers/customers.js",
                      "~/Scripts/Controllers/sites.js",
                      "~/Scripts/Controllers/fleets.js",
                      "~/Scripts/Controllers/vehicles.js",
                      "~/Scripts/Controllers/vendors.js",
                      "~/Scripts/Controllers/contracts.js",
                      "~/Scripts/Controllers/suppliers.js",
                      "~/Scripts/Controllers/commodities.js",
                      "~/Scripts/Controllers/status-types.js",
                      "~/Scripts/Controllers/delivery-time-slots.js",
                      "~/Scripts/Controllers/time-slots.js",
                      "~/Scripts/Controllers/email-log.js",
                      "~/Scripts/Controllers/error-log.js",
                      "~/Scripts/nya-bs-select.min.js",
                      "~/Scripts/ng-table.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/fontawesome.min.js",
                      "~/Scripts/custom.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/nya-bs-select.min.css",
                      "~/Content/angular-moment-picker.min.css",
                      "~/Content/ng-table.min.css",
                      "~/Content/fontawesome.min.css",
                      "~/Content/site.css"));

            //BundleTable.EnableOptimizations = true;
        }
    }
}
