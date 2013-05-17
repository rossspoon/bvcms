using System.Web.Mvc;

namespace CmsWeb.Areas.People
{
    public class PeopleAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "People";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //AddRoute(context, "Person", "Person2");
            context.MapRoute(
                "People_default",
                "People/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
        private void AddRoute(AreaRegistrationContext context, string controller)
        {
            context.MapRoute(controller, controller + "/{action}/{id}",
                new { controller, action = "Index", id = "" });
        }
        private static void AddRoute(AreaRegistrationContext context, string controller, string path)
        {
            context.MapRoute(path, path + "/{action}/{id}",
                new { controller, action = "Index", id = "" });
        }
        private static void AddRoute(AreaRegistrationContext context, string name, string controller, string path, string action)
        {
            context.MapRoute(name, path,
                new { controller, action = action, id = "" });
        }
    }
}
