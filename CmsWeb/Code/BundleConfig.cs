using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using Cassette.Scripts;
using Cassette.Stylesheets;
using CmsData;
using UtilityExtensions;
using Cassette;
using Cassette.Views;

namespace CmsWeb
{

    public class CassetteConfiguration : IConfiguration<BundleCollection>
    {
        public void Configure(BundleCollection bundles)
        {
            bundles.Add<StylesheetBundle>("css", new[] 
            { 
                  "Content/styles/jquery-ui-1.10.0.custom.css"
                , "Content/styles/dropdown.css"
                , "Content/styles/site.css"
                , "Content/styles/style2.css"
                , "Content/styles/pager.css"
                , "Content/styles/jquery.jscrollpane.css"
                , "Content/styles/joyride-1.0.5.css"
                , "Content/styles/jquery.multiselect.css"
                , "Content/styles/jquery.tooltip.css"
        	});

            bundles.Add<StylesheetBundle>("newcss", new[]
            {
                  "Content/styles/jquery-ui-1.10.0.custom.css"
                , "Content/styles/jquery.jscrollpane.css"
                , "Content/styles/joyride-1.0.5.css"
                , "Content/styles/jquery.multiselect.css"
                , "Content/styles/jquery.tooltip.css"
                , "Content/styles/jquery.alerts.css"
                , "Content/styles/layout.css"
                , "Content/styles/Main.css"
                , "Content/styles/pager.css"
//              , "Content/awesome/css/font-awesome.css"
                , "Content/chosen.css"
            });
            bundles.Add<StylesheetBundle>("new2css", new[]
            {
                  "Content/normalize.css"
                , "Content/bvcms.css"
                , "Content/bootstrap-modal/css/bootstrap-modal.css"
              //, "Content/styles/joyride-1.0.5.css"
                // Dave : need example of bootstrap tooltip
              //, "Content/styles/jquery.tooltip.css"
                // Dave : need example of alert
              //, "Content/styles/jquery.alerts.css"
                // Dave : where should I put any tweaks in css?
              //, "Content/styles/Main.css"
                , "Content/styles/pager.css"
                , "Content/font-awesome.css"
                , "Content/chosen.css"
                , "Content/moment-datepicker/datepicker.css"
                // Dave : this is here because of autocomplete
                , "Content/autocomplete/jquery-ui-1.10.2.custom.css"
            });
            bundles.Add<StylesheetBundle>("orgcss", new[]
            {
                "Content/styles/organization.css"
            });
            bundles.Add<StylesheetBundle>("dialogcss", new[]
            {
                "Content/styles/Dialog.css"
            });
            bundles.Add<ScriptBundle>("js", new[]
            {
                  "Scripts/jquery-1.9.1.js"
                , "Scripts/jquery-migrate-1.1.1.js"
                , "Scripts/jquery-ui-1.10.0.custom.js"
                , "Scripts/jquery.cookie.js"
                , "Scripts/jquery.blockUI.js"
                , "Scripts/jquery.mousewheel.js"
                , "Scripts/jquery.jscrollpane.js"
                , "Scripts/jquery.jeditable.js"
                , "Scripts/jquery.validate.js"
                , "Scripts/jquery.joyride-1.0.5.js"
                , "Scripts/modernizr-2.6.2.js"
                , "Scripts/tutorial.js"
                , "Scripts/jquery.multiSelect.js"
                , "Scripts/jquery.sortElements.js"
                , "Scripts/jquery.textarea.js"
                , "Scripts/jquery.transpose.js"
                , "Scripts/jquery.tooltip.js"
                , "Scripts/js/dropdown.js"
                , "Scripts/js/ExportToolBar.js"
                , "Scripts/js/Pager.js"
                , "Scripts/js/headermenu.js"
            });
            bundles.Add<ScriptBundle>("main", new[]
            {
                  "Scripts/jquery-1.9.1.js"
                , "Scripts/jquery-migrate-1.1.1.js"
                , "Scripts/jquery-ui-1.10.0.custom.js"
                , "Scripts/jquery.cookie.js"
                , "Scripts/jquery.blockUI.js"
                , "Scripts/jquery.mousewheel.js"
                , "Scripts/jquery.jscrollpane.js"
                , "Scripts/jquery.jeditable.js"
                , "Scripts/jquery.validate.js"
                , "Scripts/jquery.joyride-1.0.5.js"
                , "Scripts/modernizr-2.6.2.js"
                , "Scripts/tutorial.js"
                , "Scripts/jquery.multiSelect.js"
                , "Scripts/jquery.sortElements.js"
                , "Scripts/jquery.textarea.js"
                , "Scripts/jquery.tooltip.js"
                , "Scripts/jquery.transpose.js"
                , "Scripts/jquery.alerts.js"
                , "Scripts/chosen.jquery.js"
                , "Scripts/js/dropdown.js"
                , "Scripts/js/Pager.js"
                , "Scripts/js/ExportToolBar.js"
                , "Scripts/js/headermenu.js"
            });
            bundles.Add<ScriptBundle>("main2", new[]
            {
                  "Scripts/jquery-1.9.1.js"
                , "Scripts/jquery-migrate-1.1.1.js"
                , "Scripts/bootstrap.js"
                , "Scripts/bootstrap-modalmanager.js"
                , "Scripts/bootstrap-modal.js"
                , "Scripts/jquery.cookie.js"
                , "Scripts/jquery.blockUI.js"
                , "Scripts/jquery.mousewheel.js"
                , "Scripts/jquery.jeditable.js"
                , "Scripts/jquery.validate.js"
                , "Scripts/jquery.joyride-1.0.5.js"
                , "Scripts/modernizr-2.6.2.js"
                , "Scripts/tutorial.js"
                , "Scripts/jquery.sortElements.js"
                , "Scripts/jquery.textarea.js"
                , "Scripts/jquery.tooltip.js"
                , "Scripts/jquery.alerts.js"
                , "Scripts/moment.js"
                , "Scripts/moment-datepicker.js"
                , "Scripts/Autocomplete/jquery-ui-1.10.2.custom.js"
                , "Scripts/chosen.jquery.js"
                , "Scripts/js/Pager.js"
              //, "Scripts/ExportToolBar.js"
                , "Scripts/js/headermenu2.js"
            });
            bundles.Add<ScriptBundle>("onlineregister", new[]
            {
                  "Scripts/jquery-1.9.1.js"
                , "Scripts/jquery-migrate-1.1.1.js"
                , "Scripts/jquery-ui-1.10.0.custom.js"
                , "Scripts/jquery.validate.js"
                , "Scripts/jquery.idle-timer.js"
                , "Scripts/jquery.blockUI.js"
                , "Scripts/jquery.sortElements.js"
                , "Scripts/jquery.showpassword.js"
            });
            bundles.Add<ScriptBundle>("org", new[]
            {
                "Scripts/jquery-ui-timepicker-addon.js",
                "Scripts/js/Organization.js",
                "Scripts/js/SearchUsers.js",
                "Scripts/js/RegSetting.js"
            });
            bundles.Add<ScriptBundle>("task", new[]
            {
                "Scripts/jquery.form.js",
                "Scripts/jquery.form2.js",
                "Scripts/js/Task.js"
            });
            bundles.Add<ScriptBundle>("querybuilder", new[]
            {
                "Scripts/jquery.contextmenu.r2.js",
                "Scripts/js/QueryBuilder.js"
            });
            bundles.Add<ScriptBundle>("person12", new[]
            {
                 "Scripts/jquery.address-1.5.js"
                ,"Scripts/js/person1.js"
//              ,"Scripts/js/person2.js"
            });
            AddOneScript(bundles, "AdvancedSearch");
            AddOneScript(bundles, "Bundle");
            AddOneScript(bundles, "Bundles");
            AddOneScript(bundles, "Calendar");
            AddOneScript(bundles, "Calendar2");
            AddOneScript(bundles, "CheckIn");
            AddOneScript(bundles, "CheckinActivity");
            AddOneScript(bundles, "ChurchAttendance");
            AddOneScript(bundles, "compose");
            AddOneScript(bundles, "ContactSearch");
            //AddOneScript(bundles, "Contributions");
            AddOneScript(bundles, "Divisions");
            AddOneScript(bundles, "Email");
            AddOneScript(bundles, "family");
            AddOneScript(bundles, "Funds");
            AddOneScript(bundles, "HomeTest");
            AddOneScript(bundles, "ManageVolunteer");
            AddOneScript(bundles, "ManageVolunteer2");
            AddOneScript(bundles, "meeting-ticket");
            AddOneScript(bundles, "meeting");
            AddOneScript(bundles, "Meetings");
            AddOneScript(bundles, "merge");
            AddOneScript(bundles, "OnlineReg");
            AddOneScript(bundles, "OnlineRegPayment");
            AddOneScript(bundles, "OrgChildren");
            AddOneScript(bundles, "OrgGroups");
            AddOneScript(bundles, "OrgMembers");
            AddOneScript(bundles, "OrgMemberDialog");
            AddOneScript(bundles, "OrgMembersDialog");
            AddOneScript(bundles, "OrgSearch");
            AddOneScript(bundles, "PeopleSearch");
            AddOneScript(bundles, "Person");
            AddOneScript(bundles, "Person2");
            AddOneScript(bundles, "PostBundle");
            AddOneScript(bundles, "QuickSearch");
            AddOneScript(bundles, "RegSettings");
            AddOneScript(bundles, "SavedQuery");
            AddOneScript(bundles, "SearchAdd");
            AddOneScript(bundles, "SearchDivisions");
            AddOneScript(bundles, "SearchOrgs");
            AddOneScript(bundles, "SearchUsers");
            AddOneScript(bundles, "Tags");
            AddOneScript(bundles, "TestAPI");
            AddOneScript(bundles, "TotalsByFund");
            AddOneScript(bundles, "TotalsByRange");
            AddOneScript(bundles, "Transactions");
            AddOneScript(bundles, "TransactionHistory");
            AddOneScript(bundles, "UserDialog");
            AddOneScript(bundles, "Users");
            AddOneScript(bundles, "volunteering");
        }
        private static void AddOneScript(BundleCollection bundles, string f)
        {
            bundles.Add<ScriptBundle>(f, new[] { "Scripts/js/{0}.js".Fmt(f) });
        }
        public static void BundleRefJs()
        {
            var newlook = DbUtil.Db.UserPreference("newlook3", "false") == "true";
            Bundles.Reference(newlook ? "main" : "js");
        }

        public static void BundleRefCss()
        {
            var newlook = DbUtil.Db.UserPreference("newlook3", "false") == "true";
            Bundles.Reference(newlook ? "newcss" : "css");
        }

    }
}
