using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilityExtensions;
using System.Text.RegularExpressions;
using System.Data.Linq;
using System.Xml.Linq;
using System.Data.Linq.SqlClient;
using IronPython.Hosting;
using System.IO;
using CmsData.Codes;
using System.Web;

namespace CmsData
{
    public class PythonEvents
    {
        private CMSDataContext Db;

        public PythonEvents()
        {
            Db = new CMSDataContext("Data Source=.;Initial Catalog=CMS_bellevue;Integrated Security=True");
        }
        public PythonEvents(CMSDataContext Db)
        {
            this.Db = Db;
        }
        public static string RunEventScript(CMSDataContext Db, string script, dynamic o)
        {
            var evclass = new PythonEvents(Db);
#if DEBUG2
            var options = new Dictionary<string, object>();
            options["Debug"] = true;
            var engine = Python.CreateEngine(options);
            var paths = engine.GetSearchPaths();
            paths.Add(path);
            engine.SetSearchPaths(paths);
            var sc = engine.CreateScriptSourceFromFile(HttpContext.Current.Server.MapPath("/MembershipAutomation2.py"));
#else
            var engine = Python.CreateEngine();
            var sc = engine.CreateScriptSourceFromString(script);
#endif

            try
            {
                var code = sc.Compile();
                var scope = engine.CreateScope();
                code.Execute(scope);

                dynamic Event = scope.GetVariable("Event");
                dynamic m = Event();
                return m.Run(o, evclass);
            }
            catch (Exception ex)
            {
                return "Error in event script: " + ex.Message;
            }
        }
    }
}
