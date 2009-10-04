using System.Web.Mvc;
using System.Web.Routing;

namespace CMSWebCommon
{
    public class Routes : AreaRegistration
    {
        public override string AreaName
        {
            get { return "CMSWebCommon"; }
        }
        public override void RegisterArea(AreaRegistrationContext context)
        {
            AddRoute(context, "SearchPeople");
        }
        private void AddRoute(AreaRegistrationContext context, string controller)
        {
            context.MapRoute(controller, 
                controller + "/{action}/{id}",
                new { controller = controller, action = "Index", id = "" });
        }
    }
}
