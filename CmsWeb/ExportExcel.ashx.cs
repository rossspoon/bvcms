/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using CMSPresenter;
using UtilityExtensions;
using System.Collections.Generic;
using System.Web.SessionState;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMSWeb
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ExportExcel : IHttpHandler, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            var r = context.Response;
            r.ContentType = "application/vnd.ms-excel";
            r.AddHeader("Content-Disposition", "attachment;filename=CMSPeople.xls");
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
            
            int? qid = context.Request.QueryString["id"].ToInt2();
            if (!qid.HasValue)
            {
                r.Write("no queryid");
                r.Flush();
                r.End();
            }
            var labelNameFormat = context.Request.QueryString["format"];
            var ctl = new MailingController();
            var useTitles = context.Request.QueryString["titles"];
            ctl.UseTitles = useTitles == "true";
            IEnumerable d = null;
            switch (labelNameFormat)
            {
                case "Individual":
                    d = PersonSearchController.FetchExcelList(qid.Value, maxExcelRows);
                    break;
                case "Family":
                    d = ctl.FetchExcelFamily(qid.Value, maxExcelRows);
                    break;
                case "ParentsOf":
                    d = ctl.FetchExcelParents(qid.Value, maxExcelRows);
                    break;
                case "CouplesEither":
                    d = ctl.FetchExcelCouplesEither(qid.Value, maxExcelRows);
                    break;
                case "CouplesBoth":
                    d = ctl.FetchExcelCouplesBoth(qid.Value, maxExcelRows);
                    break;
                case "Involvement":
                    d = InvolvementController.InvolvementList(qid.Value);
                    break;
                case "Children":
                    d = InvolvementController.ChildrenList(qid.Value, maxExcelRows);
                    break;
                case "Attend":
                    d = InvolvementController.AttendList(qid.Value, maxExcelRows);
                    break;
            }
            var dg = new DataGrid();
            dg.EnableViewState = false;
            dg.DataSource = d;
            dg.DataBind();
            dg.RenderControl(new HtmlTextWriter(r.Output));
            r.Write("</body></HTML>");
        }
        private static int maxExcelRows
        {
            get { return WebConfigurationManager.AppSettings["MaxExcelRows"].ToInt(); }
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
