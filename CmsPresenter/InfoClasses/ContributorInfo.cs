using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using CMSPresenter.InfoClasses;
using CmsData;
using CmsData.View;

namespace CMSPresenter.InfoClasses
{
    public class ContributorInfo
    {
        public string Name { get; set;}
        public string Address1 {get; set;}
        public string Address2 {get; set;}
        public string City {get; set;}
        public string State {get; set;}
        public string Zip {get; set;}
        public int PeopleId {get; set;}
        public int? SpouseID { get; set; }
        public int FamilyId { get; set; }
        public DateTime? DeacesedDate { get; set; }
        public string CityStateZip { get { return UtilityExtensions.Util.FormatCSZ4(City,State,Zip);}}
        public int hohInd { get; set; }
        public int FamilyPositionId { get; set; }
        public int? Age { get; set; }

	}
}
