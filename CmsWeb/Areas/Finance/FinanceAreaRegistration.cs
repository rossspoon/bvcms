using System.Web.Mvc;

namespace CmsWeb.Areas.Finance
{
	public class FinanceAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "Finance";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			AddRoute(context, "QuickBooks");
            AddRoute(context, "PostBundle");
            AddRoute(context, "Bundle");
            AddRoute(context, "Bundles");
            AddRoute(context, "FinanceReports");
            AddRoute(context, "Statements");
			context.MapRoute(
				"Finance_default",
				"Finance/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}
        private void AddRoute(AreaRegistrationContext context, string controller)
        {
            context.MapRoute(controller, controller + "/{action}/{id}",
                new { controller = controller, action = "Index", id = "" });
        }
        private static void AddRoute(AreaRegistrationContext context, string name, string controller, string path, string action)
        {
            context.MapRoute(name, path,
                new { controller = controller, action = action, id = "" });
        }
	}
}
