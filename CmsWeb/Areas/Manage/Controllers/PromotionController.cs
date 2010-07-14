using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using CmsWeb.Models;
using UtilityExtensions;
using System.Collections;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace CmsWeb.Areas.Manage.Controllers
{
    public class PromotionController : CmsStaffController
    {
        public ActionResult Index()
        {
            var m = new PromotionModel();
            UpdateModel(m);
            return View(m);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AssignPending()
        {
            var m = new PromotionModel();
            UpdateModel(m);
            m.AssignPending();
            return RedirectToAction("Index");
        }
        public ActionResult List()
        {
            var m = new PromotionModel();
            UpdateModel(m);
            return PartialView("List", m);
        }
        public ActionResult Export()
        {
            var m = new PromotionModel();
            UpdateModel(m);
            return new ExcelResult { Data = m.Export(), FileName = "promotion.xls" };
        }
    }
    public class ExcelResult : ActionResult
    {
        public IEnumerable Data { get; set; }
        public string FileName { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            var r = context.HttpContext.Response;
            r.Clear();
            r.ContentType = "application/vnd.ms-excel";
            if (!string.IsNullOrEmpty(FileName))
                r.AddHeader("content-disposition",
                        "attachment;filename=" + FileName);
            string header =
@"<html xmlns:x=""urn:schemas-microsoft-com:office:excel"">
<head>
    <style>
    <!--table
    br {mso-data-placement:same-cell;}
    tr {vertical-align:top;}
    -->
    </style>
</head>
<body>";
            r.Write(header);
            r.Charset = "";

            var dg = new DataGrid();
            dg.EnableViewState = false;
            dg.DataSource = Data;
            dg.DataBind();
            dg.RenderControl(new HtmlTextWriter(r.Output));
            r.Write("</body></HTML>");
        }
    }
}
