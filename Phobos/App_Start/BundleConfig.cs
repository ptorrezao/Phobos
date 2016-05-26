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
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));
            bundles.Add(new ScriptBundle("~/bundles/AdminLTE").Include(
                       "~/Content/themes/AdminLTE/js/phobos.js",
                       "~/Content/themes/AdminLTE/js/app.js",
                       "~/Content/plugins/ohSnap/ohsnap.js"));

            bundles.Add(new ScriptBundle("~/bundles/respond", "https://oss.maxcdn.com/respond/1.4.2/respond.min.js").Include(
                        "~/Scripts/respond.js"));
            bundles.Add(new ScriptBundle("~/bundles/html5shiv", "https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js").Include(
                        "~/Scripts/html5shiv.js"));

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                        "~/Content/css/bootstrap.css"));

            bundles.Add(new StyleBundle("~/Content/MvcGrid").Include(
                        "~/Content/MvcGrid/mvc-grid.css"));

            bundles.Add(new StyleBundle("~/Content/font-awesome", "https://maxcdn.bootstrapcdn.com/font-awesome/4.4.0/css/font-awesome.min.css").Include(
                        "~/Content/themes/font-awesome.css"));

            bundles.Add(new StyleBundle("~/Content/ionicons", "https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css").Include(
                        "~/Content/themes/ionicons.css"));

            bundles.Add(new StyleBundle("~/Content/AdminLTE").Include(
                       "~/Content/themes/AdminLTE/css/AdminLTE.css",
                       "~/Content/themes/AdminLTE/css/skins/skin-blue.css"));

            bundles.Add(new StyleBundle("~/Content/iCheck").Include(
                    "~/Content/plugins/iCheck/square/blue.css"));

            bundles.Add(new StyleBundle("~/Content/select2").Include(
        "~/Content/plugins/select2/select2.css"));

            bundles.Add(new StyleBundle("~/Content/daterangepicker").Include(
                    "~/Content/plugins/daterangepicker/daterangepicker-bs3.css"));

            bundles.Add(new StyleBundle("~/Content/datepicker").Include(
                    "~/Content/plugins/datepicker/datepicker3.css"));

            bundles.Add(new ScriptBundle("~/bundles/iCheck").Include(
                     "~/Content/plugins/iCheck/icheck.js"));

            bundles.Add(new ScriptBundle("~/bundles/timeago").Include(
                    "~/Content/plugins/timeago/jquery.timeago.js"));

            bundles.Add(new ScriptBundle("~/bundles/MvcGrid").Include(
                    "~/Scripts/MvcGrid/mvc-grid.js"));

            bundles.Add(new ScriptBundle("~/bundles/input-mask").Include(
                    "~/Content/plugins/input-mask/jquery.inputmask.js",
                    "~/Content/plugins/input-mask/jquery.inputmask.date.extensions.js",
                    "~/Content/plugins/input-mask/jquery.inputmask.extensions.js",
                    "~/Content/plugins/input-mask/jquery.inputmask.numeric.extensions.js",
                    "~/Content/plugins/input-mask/jquery.inputmask.phone.extensions.js",
                    "~/Content/plugins/input-mask/jquery.inputmask.regex.extensions.js"));

            bundles.Add(new ScriptBundle("~/bundles/daterangepicker").Include(
                    "~/Content/plugins/daterangepicker/moment.js",
                    "~/Content/plugins/daterangepicker/daterangepicker.js"));

            bundles.Add(new ScriptBundle("~/bundles/datepicker").Include(
                    "~/Content/plugins/datepicker/bootstrap-datepicker.js"));

            bundles.Add(new ScriptBundle("~/bundles/shuffleLetters").Include(
                   "~/Content/plugins/shuffleLetters/jquery.shuffleLetters.js"));

            bundles.Add(new ScriptBundle("~/bundles/wysihtml5").Include(
                    "~/Content/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.all.js"));

            bundles.Add(new ScriptBundle("~/bundles/select2").Include(
               "~/Content/plugins/select2/select2.js"));

            bundles.Add(new ScriptBundle("~/bundles/typeahead").Include(
                    "~/Content/plugins/typeahead/handlebars-v4.0.5.js",
                    "~/Content/plugins/typeahead/bloodhound.js",
                    "~/Content/plugins/typeahead/typeahead.bundle.js",
                    "~/Content/plugins/typeahead/typeahead.jquery.js"));

            bundles.Add(new StyleBundle("~/Content/wysihtml5").Include(
                "~/Content/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.css"));
        }
    }
}
