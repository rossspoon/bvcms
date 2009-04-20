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
                        Indicator = Indicator(a.AttendanceTypeId, a.AttendanceFlag),
                        AttendanceFlag = a.AttendanceFlag.HasValue ? (a.AttendanceFlag.Value ? "1" : "0") : "",
                        a.MeetingDate,
                        a.MeetingId,
                        AttendType = a.AttendType.Description,
                        MemberType = a.MemberType.Description,
                        a.OtherOrgId
                    };
            GridView1.DataSource = q.Take(60);
            GridView1.DataBind();
            Name.NavigateUrl = "/Person.aspx?id=" + id;
            Name.Text = DbUtil.Db.LoadPersonById(id.Value).Name;
            Org.NavigateUrl = "/Organization.aspx?id=" + oid;
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
        private string Indicator(int type, bool? flag)
        {
            switch (type)
            {
                case 70: return "I";
                case 90: return "G";
                case 80: return "O";
                case 110: return "*";
                case 0: return ".";
                default:
                    if (flag == null)
                        return "*";
                    else if (flag.Value)
                        return "P";
                    else
                        return ".";
            }
        }
    }
}

/*	DECLARE @mindt DATETIME

	SELECT @mindt = MIN(MeetingDate)
	FROM dbo.Attend
	WHERE OrganizationId = @orgid AND PeopleId = @pid

	SELECT TOP 52 @a = 
	CASE a.AttendanceTypeId
	WHEN 70 THEN 'I'
	WHEN 90 THEN 'G'
	WHEN 80 THEN 'O'
	WHEN 110 THEN '*'
	WHEN 0 THEN '.'
	ELSE CASE a.AttendanceFlag
		WHEN NULL THEN '*'
		WHEN 1 THEN 'P'
		WHEN 0 THEN '.'
		END
	END + @a
	FROM dbo.Attend a
	WHERE a.MeetingDate >= @mindt AND a.PeopleId = @pid AND a.OrganizationId = @orgid
	ORDER BY MeetingDate DESC
*/