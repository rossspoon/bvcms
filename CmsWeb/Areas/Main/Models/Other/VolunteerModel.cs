/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */

using System.Collections.Generic;
using System.Linq;
using CmsData;
using UtilityExtensions;

namespace CmsWeb.Areas.Main.Models.Other
{
    public class VolunteerModel
    {
        public Volunteer V { get; set; }
        public List<VolunteerCode> Approvals { get; set; }
        public List<VolunteerCode> NonApprovals { get; set; }

        public VolunteerModel(int id)
        {
            var q = from v in DbUtil.Db.Volunteers
                    where v.PeopleId == id
                    select new 
                               {
                                   v,
                                   Approvals = (from a in v.VoluteerApprovalIds
                                                select a.VolunteerCode).ToList(),
                                   NonApprovals = (from n in DbUtil.Db.VolunteerCodes
                                                   where !v.VoluteerApprovalIds.Select(vv => vv.ApprovaiId).Contains(n.Id)
                                                   select n).ToList()
                               };
            var i = q.SingleOrDefault() ?? new 
                                               {
                                                   v = initVolunteer(id),
                                                   Approvals = new List<VolunteerCode>(),
                                                   NonApprovals = (from n in DbUtil.Db.VolunteerCodes
                                                                   select n).ToList()
                                               };
            V = i.v;
            Approvals = i.Approvals;
            NonApprovals = i.NonApprovals;
        }
        private Volunteer initVolunteer(int id)
        {
            var vol = new Volunteer { PeopleId = id };
            DbUtil.Db.Volunteers.InsertOnSubmit(vol);
            DbUtil.Db.SubmitChanges();
            return vol;
        }

        internal void Update(System.DateTime processDate, int statusId, string comments, List<int> approvals)
        {
            V.ProcessedDate = processDate;
            V.StatusId = statusId;
            V.Comments = comments;

            var adds = from a in approvals
                       join b in Approvals on a equals b.Id into j
                       from v in j.DefaultIfEmpty()
                       where v == null
                       select a;
            foreach (var a in adds)
                V.VoluteerApprovalIds.Add(new VoluteerApprovalId { ApprovaiId = a });

            var removes = from b in Approvals
                       join a in approvals on b.Id equals a into j
                       from v in j.DefaultIfEmpty(-1)
                       where v == -1
                       select b;

            DbUtil.Db.VolunteerCodes.DeleteAllOnSubmit(removes);
            DbUtil.Db.SubmitChanges();
        }
    }
}
