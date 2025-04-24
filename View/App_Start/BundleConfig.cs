using System.Web;
using System.Web.Optimization;

namespace View
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/validator").Include(
                   "~/Content/plugins/jQuery/jQuery-2.1.4.min.js",
                   "~/Content/plugins/bootstrap/js/bootstrap.min.js",
                   "~/Content/plugins/validation/js/formValidation.js",
                   "~/Content/plugins/validation/js/framework/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/IE9").Include(
                      "~/Content/html5shiv.min.js",
                      "~/Content/respond.min.js"));

            bundles.Add(new StyleBundle("~/bundles/tema/css").Include(
                      "~/Content/plugins/bootstrap/css/bootstrap.min.css",
                      "~/Content/plugins/validation/css/formValidation.css",
                      "~/Content/plugins/toastmessage/resources/css/jquery.toastmessage.css",
                      "~/Content/plugins/datepicker/css/bootstrap-datepicker3.min.css",
                      "~/Content/plugins/select2/css/select2.min.css",
                      "~/Content/tema/css/font-awesome.min.css",
                      "~/Content/tema/css/adminLTE.min.css",
                      "~/Content/tema/css/skins/skin-blue.min.css",
                      "~/Content/plugins/Grid/css/grid.css"));

            bundles.Add(new ScriptBundle("~/bundles/tema/js").Include(
                      "~/Content/plugins/bootstrap/fonts/glyphicons-halflings-regular*",
                      "~/Content/plugins/bootstrap/js/bootstrap-filestyle.min.js",
                      "~/Content/plugins/toastmessage/javascript/jquery.toastmessage.js",
                      "~/Content/plugins/datepicker/js/bootstrap-datepicker.js",
                      "~/Content/plugins/select2/js/select2.full.min.js",
                      "~/Content/plugins/bootstrap/js/bootbox.min.js",
                      "~/Content/plugins/fastclick/fastclick.min.js",
                      "~/Content/plugins/slimScroll/jquery.slimscroll.min.js",
                      "~/Content/jfunciones.js",
                      "~/Content/tema/js/app.min.js",
                      "~/Content/plugins/bootstrap/js/bootstrap-modal-mult.js",
                      "~/Content/plugins/jQuery-Mask/jquery.mask.js",
                      "~/Content/plugins/dataformat/date.format.js",
                      "~/Content/plugins/Grid/js/grid.js",
                      "~/Content/jquery.inputmask.bundle.js"));

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                      "~/Content/plugins/bootstrap/css/bootstrap.min.css",
                      "~/Content/plugins/validation/css/formValidation.css",
                      "~/Content/tema/css/adminLTE.min.css",
                      "~/Content/plugins/toastmessage/resources/css/jquery.toastmessage.css"));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                      "~/Content/plugins/toastmessage/javascript/jquery.toastmessage.js",
                      "~/Content/plugins/fastclick/fastclick.min.js"));
        }
    }
}
