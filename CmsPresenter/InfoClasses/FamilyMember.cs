using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using CmsData;
using CmsData.View;
using System.Collections;
using UtilityExtensions;

namespace CMSPresenter
{
    public class FamilyMember
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Age { get; set; }
        public bool Deceased { get; set; }
        public string PositionInFamily { get; set; }
        public int PositionInFamilyId { get; set; }
        public string MemberStatus { get; set; }
        public int PeopleId { get; set; }
    }
}
