using System.Web;
using System.Web.Optimization;
using CmsData;
using UtilityExtensions;

namespace CmsWeb
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/styles/css").Include(
                "~/Content/styles/jquery-ui-1.10.0.custom.css",
                "~/Content/styles/dropdown.css",
                "~/Content/styles/site.css",
                "~/Content/styles/style2.css",
                "~/Content/styles/pager.css",
                "~/Content/styles/jquery.jscrollpane.css",
//                "~/Content/styles/jquery.tooltip.css",
                "~/Content/styles/joyride-1.0.5.css",
                "~/Content/styles/jquery.multiselect.css"
                            ));

            bundles.Add(new StyleBundle("~/Content/styles/css2").Include(
                "~/Content/styles/jquery-ui-1.10.0.custom.css",
                "~/Content/styles/jquery.jscrollpane.css",
//                "~/Content/styles/jquery.tooltip.css",
                "~/Content/styles/joyride-1.0.5.css",
                "~/Content/styles/jquery.multiselect.css"
                            ));

            bundles.Add(new StyleBundle("~/Content/styles/org-css").Include(
                "~/Content/styles/organization.css"));

            bundles.Add(new StyleBundle("~/Content/dialog-css").Include(
                "~/Content/styles/Dialog.css"));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                "~/Content/js/jquery-1.9.0.js",
                "~/Content/js/jquery-ui-1.10.0.custom.js",
//                "~/Content/js/jquery.bgiframe-2.1.2.js",
                "~/Content/js/jquery.cookie.js",
                "~/Content/js/jquery.blockUI.js",
//                "~/Content/js/jquery.tooltip.js",
                "~/Content/js/jquery.mousewheel.js",
                "~/Content/js/jquery.jscrollpane.js",
                "~/Content/js/jquery.jeditable.js",
                "~/Content/js/jquery.validate.js",
                "~/Content/js/jquery.joyride-1.0.5.js",
                "~/Content/js/modernizr.mq.js",
                "~/Content/js/tutorial.js",
                "~/Content/js/jquery.multiSelect.js",
                "~/Content/js/jquery.sortElements.js",
                "~/Content/js/jquery.textarea.js",
                "~/Content/js/jquery.transpose.js",
                "~/Scripts/dropdown.js",
                "~/Scripts/ExportToolBar.js",
                "~/Scripts/Pager.js",
                "~/Scripts/headermenu.js"
                            ));
            bundles.Add(new ScriptBundle("~/bundles/main-js").Include(
                "~/Content/js/jquery-1.9.0.js",
                "~/Content/js/jquery-ui-1.10.0.custom.js",
//                "~/Content/js/jquery.bgiframe-2.1.2.js",
                "~/Content/js/jquery.cookie.js",
                "~/Content/js/jquery.blockUI.js",
//                "~/Content/js/jquery.tooltip.js",
                "~/Content/js/jquery.mousewheel.js",
                "~/Content/js/jquery.jscrollpane.js",
                "~/Content/js/jquery.jeditable.js",
                "~/Content/js/jquery.validate.js",
                "~/Content/js/jquery.joyride-1.0.5.js",
                "~/Content/js/modernizr.mq.js",
                "~/Content/js/tutorial.js",
                "~/Content/js/jquery.multiSelect.js",
                "~/Content/js/jquery.sortElements.js",
                "~/Content/js/jquery.textarea.js",
                "~/Content/js/jquery.transpose.js"
                            ));

            bundles.Add(new ScriptBundle("~/bundles/onlineregister-js").Include(
                "~/Content/js/jquery-1.9.0.js",
                "~/Content/js/jquery-ui-1.10.0.custom.js",
                "~/Content/js/jquery.validate.js",
                "~/Content/js/jquery.idle-timer.js",
                "~/Content/js/jquery.blockUI.js",
//                "~/Content/js/jquery.tooltip.js",
                "~/Content/js/jquery.showpassword.js"
                            ));

            bundles.Add(new ScriptBundle("~/bundles/org-js").Include(
                "~/Content/js/jquery-ui-timepicker-addon.js",
                "~/Content/js/jquery-ui-dialog-patch.js",
                "~/Scripts/Organization.js",
                "~/Scripts/SearchUsers.js",
                "~/Scripts/RegSetting.js"
                            ));

            bundles.Add(new ScriptBundle("~/bundles/task-js").Include(
                "~/Content/js/jquery.form.js",
                "~/Content/js/jquery.form2.js",
                "~/Scripts/Task.js"
                            ));
            bundles.Add(new ScriptBundle("~/bundles/querybuilder-js").Include(
                "~/Content/js/jquery.contextMenu.js",
                "~/Scripts/QueryBuilder.js"
                            ));

            bundles.Add(new ScriptBundle("~/bundles/edit-js").Include(
                "~/ckeditor/adapters/jquery.js",
                "~/Content/js/jquery-ui-dialog-patch.js",
                "~/Scripts/Edit.js"
                            ));

            bundles.Add(new ScriptBundle("~/bundles/meeting-ipad-js").Include(
                "~/Content/js/jquery-1.9.0.js",
                "~/Content/js/jquery.mobile-1.2.0.min.js",
                "~/scripts/meeting-ipad.js"
                ));

            bundles.AddOneScript("AdvancedSearch");
            bundles.AddOneScript("Bundle");
            bundles.AddOneScript("Bundles");
            bundles.AddOneScript("Calendar");
            bundles.AddOneScript("CheckIn");
            bundles.AddOneScript("CheckinActivity");
            bundles.AddOneScript("compose");
            bundles.AddOneScript("ContactSearch");
            bundles.AddOneScript("Contributions");
            bundles.AddOneScript("Divisions");
            bundles.AddOneScript("Email");
            bundles.AddOneScript("family");
            bundles.AddOneScript("Funds");
            bundles.AddOneScript("ManageVolunteer");
            bundles.AddOneScript("meeting-ticket");
            bundles.AddOneScript("meeting");
            bundles.AddOneScript("Meetings");
            bundles.AddOneScript("merge");
            bundles.AddOneScript("OnlineReg");
            bundles.AddOneScript("OnlineRegPayment");
            bundles.AddOneScript("OrgChildren");
            bundles.AddOneScript("OrgGroups");
            bundles.AddOneScript("OrgMembers");
            bundles.AddOneScript("OrgMemberDialog");
            bundles.AddOneScript("OrgMembersDialog");
            bundles.AddOneScript("OrgSearch");
            bundles.AddOneScript("PeopleSearch");
            bundles.AddOneScript("Person");
            bundles.AddOneScript("PostBundle");
            bundles.AddOneScript("QuickSearch");
            bundles.AddOneScript("RegSettings");
            bundles.AddOneScript("SavedQuery");
            bundles.AddOneScript("SearchAdd");
            bundles.AddOneScript("SearchDivisions");
            bundles.AddOneScript("SearchOrgs");
            bundles.AddOneScript("SearchUsers");
            bundles.AddOneScript("Tags");
            bundles.AddOneScript("TestAPI");
            bundles.AddOneScript("TotalsByFund");
            bundles.AddOneScript("TotalsByRange");
            bundles.AddOneScript("Transactions");
            bundles.AddOneScript("TransactionHistory");
            bundles.AddOneScript("UserDialog");
            bundles.AddOneScript("Users");
        }

        private static void AddOneScript(this BundleCollection bundles, string f)
        {
            var file = f + ".js";
            bundles.Add(new ScriptBundle("~/bundles/single/{0}-js".Fmt(f)).Include("~/Scripts/" + file));
        }
    }
}