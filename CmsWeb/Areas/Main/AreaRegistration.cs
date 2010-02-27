using System.Web.Mvc;

namespace CMSWeb.Areas.Main
{
    public class MainAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Main";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            AddRoute(context, "Checkin");
            AddRoute(context, "SearchPeople");
            AddRoute(context, "SearchPeopleDialog");
            AddRoute(context, "TaskList", "Task", "Task/{action}/{id}", "List");
            AddRoute(context, "Task");
            AddRoute(context, "OrgMemberDialog");
            AddRoute(context, "Reports");
            AddRoute(context, "Person");
            AddRoute(context, "TaskDetail", "Task", "Task/Detail/{id}/Row/{rowid}", "Detail");
            AddRoute(context, "QueryBuilderMain", "QueryBuilder", "QueryBuilder/{action}/{id}", "Main");
            AddRoute(context, "QueryBuilder");

            context.MapRoute(
                "Main_default",
                "Main/{controller}/{action}/{id}",
                new { action = "Index", id = "" }
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
