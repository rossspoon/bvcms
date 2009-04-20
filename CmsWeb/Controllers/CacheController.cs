using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Drawing;
using System.Web.Configuration;
using System.Reflection;
using System.Web.Routing;
using System.IO;
using System.Web.Caching;
using CMSWeb;

namespace CMSWeb.Controllers
{
    public class CacheResult : ActionResult
    {
        private string _keyname;
        private string _version;
        private string _type;

        public CacheResult(string keyname, string version)
        {
            this._keyname = keyname;
            this._version = version;
            if (keyname.EndsWith("css"))
                this._type = @"text/css";

            if (keyname.EndsWith("js"))
                this._type = @"text/javascript";
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            var c = new ScriptCombiner
                (this._keyname, this._version, this._type);

            c.ProcessRequest(context.HttpContext);
        }
    }

    public class CacheController : Controller
    {
        public CacheResult Content(string key, string version)
        {
            return new CacheResult(key, version);
        }
    }
}