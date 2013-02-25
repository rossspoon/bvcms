using System.Collections.Generic;
using System.Linq;
using CmsData;
using CmsWeb.Models;

namespace CmsWeb.Areas.People.Models.Person
{
    public class VolunteerModel
    {
        public Volunteer vol;
        public VolunteerModel(int? id)
        {
            vol = DbUtil.Db.Volunteers.FirstOrDefault(v => v.PeopleId == id.Value);
            if (vol == null)
				vol = new Volunteer { PeopleId = id.Value };
        }
        public IEnumerable<string> VolOpportunities()
        {
            return CodeValueModel.VolunteerOpportunities();
      //      var q = (from c in DbUtil.Db.VolInterestInterestCodes
					 //where c.PeopleId == vol.PeopleId
      //               group c by c.VolInterestCode.Org into g
      //               select g.Key);
        }
    }
}
