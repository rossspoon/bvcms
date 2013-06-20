using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CmsWeb.Models;
using UtilityExtensions;

namespace CmsWeb
{
    public partial class BulkMail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var context = HttpContext.Current;
            int? qid = context.Request.QueryString["id"].ToInt2();
            var labelNameFormat = context.Request.QueryString["format"];
            var ctl = new MailingController();
            var sortZip = context.Request.QueryString["sortZip"];
	        var sort = "Name";
			var useTitles = context.Request.QueryString["titles"];
				ctl.UseTitles = useTitles == "true";
			if (sortZip == "true")
				sort = "Zip";
            IEnumerable<MailingController.MailingInfo> q = null;
            switch (labelNameFormat)
            {
                case "Individual":
                    q = ctl.FetchIndividualList(sort, qid.Value);
                    break;
                case "FamilyMembers":
                    q = ctl.FetchFamilyMembers(sort, qid.Value);
                    break;
                case "Family":
                    q = ctl.FetchFamilyList(sort, qid.Value);
                    break;
                case "ParentsOf":
                    q = ctl.FetchParentsOfList(sort, qid.Value);
                    break;
                case "CouplesEither":
                    q = ctl.FetchCouplesEitherList(sort, qid.Value);
                    break;
                case "CouplesBoth":
                    q = ctl.FetchCouplesBothList(sort, qid.Value);
                    break;
            }
            var r = context.Response;
            if (q == null)
            {
                r.Write("no format");
                return;
            }

            r.Clear();
            r.ContentType = "text/plain";
            r.AddHeader("Content-Disposition", "attachment;filename=CMSPeople.csv");
            r.Charset = "";
            if (!qid.HasValue)
            {
                r.Write("no queryid");
                r.Flush();
                r.End();
            }
            foreach (var mi in q)
                r.Write(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",{5},{6}\r\n",
                    mi.LabelName, mi.Address, mi.Address2, mi.City, mi.State, mi.Zip.FmtZip(), mi.PeopleId));

        }
    }
}
