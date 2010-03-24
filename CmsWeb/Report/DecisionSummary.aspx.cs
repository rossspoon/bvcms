using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CmsData;
using UtilityExtensions;

namespace CMSWeb.StaffOnly
{
    public partial class DecisionSummary : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DbUtil.LogActivity("Viewing Decision Summary Rpt");
                var today = Util.Now.Date;
                FromDate.Text = new DateTime(today.Year, 1, 1).ToString("d");
                ToDate.Text = today.ToString("d");
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            DecisionsView.DataBind();
            BaptismsByAgeView.DataBind();
            BaptismsByTypeView.DataBind();
            NewMemberView.DataBind();
            DroppedMemberView.DataBind();
            DroppedMemberChurchView.DataBind();
        }

        protected void ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            var qb = DbUtil.Db.QueryBuilderScratchPad();
            qb.CleanSlate();
            DateTime dt;
            DateTime? FromDt = null, dt2 = null;
            if (DateTime.TryParse(FromDate.Text, out dt))
                FromDt = dt;
            if (DateTime.TryParse(ToDate.Text, out dt))
                dt2 = dt;
            DateTime ToDt = (dt2 ?? FromDt).Value.AddDays(1);

            bool NotAll = (string)e.CommandArgument != "All";

            switch (e.CommandName)
            {
                case "ForDecisionType":
                    qb.AddNewClause(QueryType.DecisionDate, CompareType.GreaterEqual, FromDt);
                    qb.AddNewClause(QueryType.DecisionDate, CompareType.Less, ToDt);
                    if (NotAll)
                        qb.AddNewClause(QueryType.DecisionTypeId, CompareType.Equal, e.CommandArgument);
                    break;
                case "ForBaptismGender":
                    qb.AddNewClause(QueryType.BaptismDate, CompareType.GreaterEqual, FromDt);
                    qb.AddNewClause(QueryType.BaptismDate, CompareType.Less, ToDt);
                    if (NotAll)
                        qb.AddNewClause(QueryType.GenderId, CompareType.Equal, e.CommandArgument);
                    break;
                case "ForBaptismType":
                    qb.AddNewClause(QueryType.BaptismDate, CompareType.GreaterEqual, FromDt);
                    qb.AddNewClause(QueryType.BaptismDate, CompareType.Less, ToDt);
                    if (NotAll)
                        qb.AddNewClause(QueryType.BaptismTypeId, CompareType.Equal, e.CommandArgument);
                    break;
                case "ForNewMemberType":
                    qb.AddNewClause(QueryType.JoinDate, CompareType.GreaterEqual, FromDt);
                    qb.AddNewClause(QueryType.JoinDate, CompareType.Less, ToDt);
                    if (NotAll)
                        qb.AddNewClause(QueryType.JoinCodeId, CompareType.Equal, e.CommandArgument);
                    break;
                case "ForDropType":
                    qb.AddNewClause(QueryType.DropDate, CompareType.GreaterEqual, FromDt);
                    qb.AddNewClause(QueryType.DropDate, CompareType.Less, ToDt);
                    if (NotAll)
                        qb.AddNewClause(QueryType.DropCodeId, CompareType.Equal, e.CommandArgument);
                    qb.AddNewClause(QueryType.IncludeDeceased, CompareType.Equal, "1,T");
                    break;
                case "DroppedForChurch":
                    qb.AddNewClause(QueryType.DropDate, CompareType.GreaterEqual, FromDt);
                    qb.AddNewClause(QueryType.DropDate, CompareType.Less, ToDt);
                    switch ((string)e.CommandArgument)
                    {
                        case "Unknown":
                            qb.AddNewClause(QueryType.OtherNewChurch, CompareType.IsNull, "");
                            qb.AddNewClause(QueryType.IncludeDeceased, CompareType.Equal, "1,T");
                            break;
                        case "Total":
                            break;
                        default:
                            qb.AddNewClause(QueryType.OtherNewChurch, CompareType.Equal, e.CommandArgument);
                            break;
                    }
                    qb.AddNewClause(QueryType.IncludeDeceased, CompareType.Equal, "1,T");
                    break;
            }
            DbUtil.Db.SubmitChanges();
            Response.Redirect("/QueryBuilder/Main/" + qb.QueryId);
        }
    }
}
