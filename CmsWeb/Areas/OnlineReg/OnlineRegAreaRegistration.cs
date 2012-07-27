using System.Web.Mvc;
using UtilityExtensions;
using CmsData;
using System.Linq;
using System.Data.Linq;
using System;

namespace CmsWeb.Areas.OnlineReg
{
    public class OnlineRegAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "OnlineReg";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            AddRoute(context, "OnlineReg");
            context.MapRoute(
                "Public_CreateAccount",
                "CreateAccount",
                new { controller = "OnlineReg", action = "Index", id = Util.CreateAccountCode.ToString() }
            );
           context.MapRoute( "GetVolSub",
                "OnlineReg/GetVolSub/{pid}/{oid}/{ticks}",
                new { controller = "OnlineReg", action = "GetVolSub", pid=0, oid=0, ticks=0 }
            );
           context.MapRoute( "ClaimVolSub",
                "OnlineReg/ClaimVolSub/{pid}/{oid}/{ticks}/{sid}",
                new { controller = "OnlineReg", action = "ClaimVolSub", pid=0, oid=0, ticks=0, sid=0 }
            );
            context.MapRoute(
                "Public_MyData",
                "MyData",
                new { controller = "OnlineReg", action = "Index", id = Util.CreateAccountCode.ToString() }
            );
            context.MapRoute(
                "OnlineReg_default",
                "OnlineReg/{controller}/{action}/{id}",
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
