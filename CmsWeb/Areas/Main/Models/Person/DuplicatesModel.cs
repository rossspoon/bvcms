using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using UtilityExtensions;

namespace CmsWeb.Models.PersonPage
{
    public class DuplicatesModel
    {
        private int PeopleId;
        public Person person { get; set; }
        public DuplicatesModel(int id)
        {
            PeopleId = id;
            person = DbUtil.Db.LoadPersonById(id);
        }
    }
}
