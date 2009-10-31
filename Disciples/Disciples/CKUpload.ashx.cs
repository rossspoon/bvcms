using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using UtilityExtensions;
using System.IO;

namespace Disciples
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CKUpload : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var Request = context.Request;
            var Server = context.Server;
            var Response = context.Response;
            if (Request.RequestType != "POST")
                return;
            var CKEditorFuncNum = Request.QueryString["CKEditorFuncNum"];
            var baseurl = Util.ResolveServerUrl("~/pictures/");
            var error = string.Empty;
            var fn = string.Empty;
            try
            {
                var file = Request.Files[0];
                fn = Path.GetFileName(file.FileName);
                var path = Server.MapPath("/pictures/" + fn);

                while (System.IO.File.Exists(path))
                {
                    var ext = Path.GetExtension(path);
                    fn = Path.GetFileNameWithoutExtension(path) + "a" + ext;
                    var dir = Path.GetDirectoryName(path);
                    path = Path.Combine(dir, fn);
                }
                file.SaveAs(path);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                baseurl = string.Empty;
            }
            Response.ContentType = "text/html";
            Response.Write(string.Format(
"<script type='text/javascript'>window.parent.CKEDITOR.tools.callFunction( {0}, '{1}', '{2}' );</script>",
CKEditorFuncNum, baseurl + fn, error));
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
