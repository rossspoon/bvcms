using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UtilityExtensions;
using CMSPresenter;

namespace CMSWeb
{
    public partial class BulkMail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var context = HttpContext.Current;
            int? qid = context.Request.QueryString["id"].ToInt2();
            var labelNameFormat = context.Request.QueryString["format"];
            var ctl = new MailingController();
            var useTitles = context.Request.QueryString["titles"];
            ctl.UseTitles = useTitles == "true";
            IEnumerable<MailingInfo> q = null;
            switch (labelNameFormat)
            {
                case "Individual":
                    q = ctl.FetchIndividualList("Name", qid.Value);
                    break;
                case "Family":
                    q = ctl.FetchFamilyList("Name", qid.Value);
                    break;
                case "ParentsOf":
                    q = ctl.FetchParentsOfList("Name", qid.Value);
                    break;
                case "CouplesEither":
                    q = ctl.FetchCouplesEitherList("Name", qid.Value);
                    break;
                case "CouplesBoth":
                    q = ctl.FetchCouplesBothList("Name", qid.Value);
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
                r.Write(string.Format("{0},{1},{2},{3},{4},{5},{6}\n",
                    mi.LabelName, mi.Address, mi.Address2, mi.City, mi.State, mi.Zip.FmtZip(), mi.PeopleId));

        }
    }
}
