using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using System.Web.Mvc;
using UtilityExtensions;
using CmsWeb.Areas.Main.Models.Report;
using CmsData.Codes;
using System.Collections;

namespace CmsWeb.Models
{
	public class MeetingsModel
	{
	    public int OrgId { get; set; }
		public string OrgName { get; set; }
		public MeetingsModel(int id)
		{
		    OrgId = id;
			OrgName = (from o in DbUtil.Db.Organizations where o.OrganizationId == id select o.OrganizationName).Single();
		}
        public IEnumerable<Meeting> FutureMeetings()
        {
            var q = from m in DbUtil.Db.Meetings
                    where m.MeetingDate > DateTime.Today
                    select m;
            return q;
        }
	}
}