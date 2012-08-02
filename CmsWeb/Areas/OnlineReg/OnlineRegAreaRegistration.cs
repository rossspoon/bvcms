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
                "OnlineReg/GetVolSub/{aid}/{pid}",
                new { controller = "OnlineReg", action = "GetVolSub", aid=0, pid=0 }
            );
           context.MapRoute( "VolSubReport",
                "OnlineReg/VolSubReport/{aid}/{pid}/{ticks}",
                new { controller = "OnlineReg", action = "VolSubReport", aid=0, pid=0, ticks=0 }
            );
           context.MapRoute( "ClaimVolSub",
                "OnlineReg/ClaimVolSub/{ans}/{guid}",
                new { controller = "OnlineReg", action = "ClaimVolSub", ans="", guid="" }
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
