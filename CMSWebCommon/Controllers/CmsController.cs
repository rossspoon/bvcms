using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UtilityExtensions;

namespace CMSWebCommon.Controllers
{
    public class CmsController : System.Web.Mvc.Controller
    {
        protected override void HandleUnknownAction(string actionName)
        {
            base.HandleUnknownAction(actionName);
            throw new HttpException(404, "");
        }
    }
}
