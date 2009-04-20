using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilityExtensions;

namespace CmsData
{
    public partial class Tag
    {
        private CMSDataContext _Db;
        public CMSDataContext Db
        {
            get
            {
                if (_Db == null)
                    _Db = this.GetDataContext() as CMSDataContext;
                return _Db;
            }
        }
        public void DeleteTag()
        {
            Db.TagPeople.DeleteAllOnSubmit(PersonTags);
            Db.TagShares.DeleteAllOnSubmit(TagShares);
            Db.Tags.DeleteOnSubmit(this);
        }
        public IQueryable<Person> People()
        {
            return Db.People.Where(p => p.Tags.Any(tp => tp.Id == Id));
        }
        public string SharedWithCountString()
        {
            return "Shared with {0} users".Fmt(TagShares.Count());
        }
    }
}
