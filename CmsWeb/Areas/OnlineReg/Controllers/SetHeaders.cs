using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using UtilityExtensions;
using System.Text.RegularExpressions;
using CmsWeb.Models;

namespace CmsWeb.Areas.OnlineReg.Controllers
{
    public partial class OnlineRegController : CmsController
    {
        private void SetHeaders(OnlineRegModel m2)
        {
            Session["gobackurl"] = m2.URL;
            ViewData["timeout"] = INT_timeout;
            SetHeaders(m2.orgid ?? m2.divid ?? 0);
        }
        private void SetHeaders(int id)
        {
            var org = DbUtil.Db.LoadOrganizationById(id);
            var s = DbUtil.Content(org != null ? org.Shell : null, 
                        DbUtil.Content("ShellDiv-" + id, 
                                DbUtil.Content("ShellDefault", 
                                        "")));
            if (s.HasValue())
            {
                ViewData["hasshell"] = true;
                var re = new Regex(@"(.*<!--FORM START-->\s*).*(<!--FORM END-->.*)", RegexOptions.Singleline);
                var t = re.Match(s).Groups[1].Value.Replace("<!--FORM CSS-->", 
@"
<link href=""/Content/jquery-ui-1.8.9.custom.css"" rel=""stylesheet"" type=""text/css"" />
<link href=""/Content/onlinereg.css?v=4"" rel=""stylesheet"" type=""text/css"" />
<style type=""text/css"">
#username, #password {
width: 10em;
}</style>

");
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