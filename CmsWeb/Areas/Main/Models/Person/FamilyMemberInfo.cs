using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using System.Web.Mvc;
using UtilityExtensions;
using System.Data.Linq.SqlClient;
using System.Data.Linq;

namespace CmsWeb.Models.PersonPage
{
    public class FamilyMemberInfo
    {
            public int Id { get; set; }
            public string Name { get; set; }
            public int? Age { get; set; }
            public string Color { get; set; }
            public string PositionInFamily { get; set; }
            public string SpouseIndicator { get; set; }
            public string Email { get; set; }
    }
}
