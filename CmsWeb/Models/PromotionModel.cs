using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using CmsData;
using UtilityExtensions;

namespace CMSWeb.Models
{
    public class PromotionModel
    {
        public int PromotionId { get; set; }
        public Promotion Promotion { get; set; }
        public int TargetClassId { get; set; }
        public bool FilterUnassigned { get; set; }

        public IEnumerable<PromoteInfo> FetchStudents()
        {
            var q = from p in DbUtil.Db.People
                    let bfc = DbUtil.Db.Organizations.SingleOrDefault(o => o.OrganizationId == p.BibleFellowshipClassId)
                    let om = DbUtil.Db.OrganizationMembers.SingleOrDefault(om => 
                        om.OrganizationId == bfc.OrganizationId 
                        && om.PeopleId == p.PeopleId)
                    let pc = DbUtil.Db.OrganizationMembers.SingleOrDefault(op => 
                        op.Pending == true 
                        && op.PeopleId == p.PeopleId 
                        && op.Organization.DivOrgs.Any(d => d.Division.Program.BFProgram == true))
                    where bfc != null && bfc.DivOrgs.Any(d => d.DivId == Promotion.FromDivId)
                    select new PromoteInfo
                    {
                         AttendPct = om.AttendPct,
                          Birthday = Util.FormatBirthday(p.BirthYear, p.BirthMonth, p.BirthDay),
                           CurrClassId = bfc.OrganizationId,
                            CurrClassName = bfc.OrganizationName,
                             Gender = p.GenderId == 1 ? "M" : "F",
                              PendingClassId = pc == null ? (int?)null : pc.OrganizationId,



                    };
            return q;

        }
    }
}
