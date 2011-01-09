using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using UtilityExtensions;
using System.Text;
using CmsData;
using LumenWorks.Framework.IO.Csv;
using System.IO;

namespace CmsWeb.Areas.Setup.Controllers
{
    [Authorize(Roles = "Admin")]
    [ValidateInput(false)]
    public class BatchController : AsyncController
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MoveAndDelete()
        {
            return View();
        }
        [HttpPost]
        [AsyncTimeout(600000)]
        public void MoveAndDeleteAsync(string text)
        {
            AsyncManager.OutstandingOperations.Increment();
            string host = Util.Host;
            ThreadPool.QueueUserWorkItem((e) =>
            {
                var sb = new StringBuilder();
                sb.Append("<h2>done</h2>\n<p><a href='/'>home</a></p>\n");
                using (var csv = new CsvReader(new StringReader(text), false, '\t'))
                {
                    while (csv.ReadNextRecord())
                    {
                        if (csv.FieldCount != 2)
                        {
                            sb.AppendFormat("expected two ids, row {0}<br/>\n", csv[0]);
                            continue;
                        }

                        var fromid = csv[0].ToInt();
                        var toid = csv[1].ToInt();
                        var Db = new CMSDataContext(Util.GetConnectionString(host));
                        var p = Db.LoadPersonById(fromid);
                        if (p == null)
                        {
                            sb.AppendFormat("fromid {0} not found<br/>\n", fromid);
                            Db.Dispose();
                            continue;
                        }
                        var tp = Db.LoadPersonById(toid);
                        if (tp == null)
                        {
                            sb.AppendFormat("toid {0} not found<br/>\n", toid);
                            Db.Dispose();
                            continue;
                        }
                        try
                        {
                            p.MovePersonStuff(Db, toid);
                            Db.SubmitChanges();
                        }
                        catch (Exception ex)
                        {
                            sb.AppendFormat("error on move ({0}, {1}): {2}<br/>\n", fromid, toid, ex.Message);
                            Db.Dispose();
                            continue;
                        }
                        try
                        {
                            Db.PurgePerson(fromid);
                            sb.AppendFormat("moved ({0}, {1}) successful<br/>\n", fromid, toid);
                        }
                        catch (Exception ex)
                        {
                            sb.AppendFormat("error on delete ({0}): {1}<br/>\n", fromid, ex.Message);
                        }
                        finally
                        {
                            Db.Dispose();
                        }
                    }
                }
                AsyncManager.Parameters["results"] = sb.ToString();
                AsyncManager.OutstandingOperations.Decrement();
            });
        }
        public ActionResult MoveAndDeleteCompleted(string results)
        {
            return Content(results);
        }
    }
}
