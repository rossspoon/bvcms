using System.Web.Mvc;
using System.Web.Routing;

namespace CMSRegCustom
{
    public class Routes : AreaRegistration
    {
        public override string AreaName
        {
            get { return "CMSRegCustom"; }
        }
        public override void RegisterArea(AreaRegistrationContext context)
        {
           AddRoute(context, "DiscipleLife");
           AddRoute(context, "LoveRespect");
           AddRoute(context, "SoulMate");
           AddRoute(context, "StepClass");
           AddRoute(context, "VBS");
           AddRoute(context, "VBSReg");
           AddRoute(context, "MOBS");
           AddRoute(context, "Sales");
           AddRoute(context, "GODisciples");
        }
        private void AddRoute(AreaRegistrationContext context, string controller)
        {
            context.MapRoute(controller, controller + "/{action}/{id}",
                new { controller = controller, action = "Index", id = "" });
        }
    }
}
