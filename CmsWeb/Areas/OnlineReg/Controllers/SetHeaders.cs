using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using UtilityExtensions;
using System.Text.RegularExpressions;

namespace CmsWeb.Areas.OnlineReg.Controllers
{
    public partial class OnlineRegController : CmsController
    {
        private void SetHeaders(int id)
        {
            var org = DbUtil.Db.LoadOrganizationById(id);
            var s = DbUtil.Content(org != null ? org.Shell : null, DbUtil.Content("ShellDefault", ""));
            if (s.HasValue())
            {
                ViewData["hasshell"] = true;
                var re = new Regex(@"(.*<!--FORM START-->\s*).*(<!--FORM END-->.*)", RegexOptions.Singleline);
                var t = re.Match(s).Groups[1].Value;
                ViewData["top"] = t;
                var b = re.Match(s).Groups[2].Value;
                ViewData["bottom"] = b;
            }
            else
            {
                ViewData["hasshell"] = false;
                ViewData["header"] = DbUtil.Content("OnlineRegHeader-" + id,
                    DbUtil.Content("OnlineRegHeader", ""));
                ViewData["top"] = DbUtil.Content("OnlineRegTop-" + id,
                    DbUtil.Content("OnlineRegTop", ""));
                ViewData["bottom"] = DbUtil.Content("OnlineRegBottom-" + id,
                    DbUtil.Content("OnlineRegBottom", ""));
            }
        }
    }
}