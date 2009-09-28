using System.Web.Mvc;
using System.Web.Routing;

namespace CMSRegCustom
{
    public static class Routes
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.AddRoute("DiscipleLife");
            routes.AddRoute("LoveRespect");
            routes.AddRoute("SoulMate");
            routes.AddRoute("StepClass");
            routes.AddRoute("VBS");
            routes.AddRoute("VBSReg");
        }
        private static void AddRoute(this RouteCollection routes, string controller)
        {
            routes.MapAreaRoute("CMSRegCustom", controller, controller + "/{action}/{id}",
                new { controller = controller, action = "Index", id = "" },
                new string[] { "CMSRegCustom.Controllers" });
        }
    }
}
