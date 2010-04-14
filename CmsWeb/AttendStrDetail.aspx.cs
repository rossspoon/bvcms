/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UtilityExtensions;
using CmsData;

namespace CMSWeb
{
    public partial class AttendStrDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var id = this.QueryString<int?>("id");
            var oid = this.QueryString<int?>("oid");
            var q = from a in DbUtil.Db.Attends
                    where a.PeopleId == id && a.OrganizationId == oid
                    orderby a.MeetingDate descending
                    select new
                    {
                        Indicator = Indicator(a.AttendanceTypeId, a.EffAttendFlag),
                        AttendanceFlag = a.EffAttendFlag.HasValue ? (a.EffAttendFlag.Value ? "1" : "0") : "_",
                        a.MeetingDate,
                        a.MeetingId,
                        AttendType = a.AttendType.Description,
                        MemberType = a.MemberType.Description,
                        a.OtherAttends,
                        a.OtherOrgId,
                    };
            GridView1.DataSource = q.Take(60);
            GridView1.DataBind();
            //Name.NavigateUrl = "/Person/Index/" + id;
            Name.Text = DbUtil.Db.LoadPersonById(id.Value).Name;
            Org.Text = DbUtil.Db.LoadOrganizationById(oid.Value).FullName;


            var q2 = from et in DbUtil.Db.EnrollmentTransactions
                     where et.PeopleId == id && et.OrganizationId == oid
                     where et.TransactionStatus == false
                     orderby et.TransactionDate descending
                     select new
                     {
                         et.TransactionId,
                         et.TransactionDate,
                         et.TransactionTypeId,
                         MemberType = et.MemberType.Description,
                         et.TransactionStatus,
                         et.NextTranChangeDate
                     };
            GridView2.DataSource = q2;
            GridView2.DataBind();
        }
        private string Indicator(int? type, bool? flag)
        {
            if (flag == null) // attended elsewhere or Group
                switch (type)
                {
                    case 20: return "V";
                    case 70: return "I";
                    case 90: return "G";
                    case 80: return "O";
                    case 110: return "*";
                    default: return "*";
                }
            else if (flag.Value) // attended here
                    return "P";
            else // absent
                return ".";
        }
    }
}
