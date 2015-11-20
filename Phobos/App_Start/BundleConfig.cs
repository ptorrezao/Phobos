using System.Web;
using System.Web.Optimization;

namespace Phobos
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));
            bundles.Add(new ScriptBundle("~/bundles/AdminLTE").Include(
               "~/Content/themes/AdminLTE/js/app.js"));
            bundles.Add(new ScriptBundle("~/bundles/respond", "https://oss.maxcdn.com/respond/1.4.2/respond.min.js").Include(
                     "~/Scripts/respond.js"));
            bundles.Add(new ScriptBundle("~/bundles/html5shiv", "https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js").Include(
                     "~/Scripts/html5shiv.js"));

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                      "~/Content/css/bootstrap.css"));


            bundles.Add(new StyleBundle("~/Content/font-awesome", "https://maxcdn.bootstrapcdn.com/font-awesome/4.4.0/css/font-awesome.min.css").Include(
              "~/Content/themes/font-awesome.css"));

            bundles.Add(new StyleBundle("~/Content/ionicons", "https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css").Include(
                "~/Content/themes/ionicons.css"));

            bundles.Add(new StyleBundle("~/Content/AdminLTE").Include(
                    "~/Content/themes/AdminLTE/css/AdminLTE.css",
                    "~/Content/themes/AdminLTE/css/skins/skin-blue.css"));

            bundles.Add(new StyleBundle("~/Content/iCheck").Include(
                    "~/Content/plugins/iCheck/square/blue.css"));

            bundles.Add(new ScriptBundle("~/bundles/iCheck").Include(
                     "~/Content/plugins/iCheck/icheck.js"));

            bundles.Add(new ScriptBundle("~/bundles/shuffleLetters").Include(
                   "~/Content/plugins/shuffleLetters/jquery.shuffleLetters.js"));
        }
    }
}
