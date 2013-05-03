using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilityExtensions;

namespace CmsData
{
    public partial class Tag
    {
        public void DeleteTag(CMSDataContext Db)
        {
            Db.TagPeople.DeleteAllOnSubmit(PersonTags);
            Db.TagShares.DeleteAllOnSubmit(TagShares);
            Db.Tags.DeleteOnSubmit(this);
        }
        public IQueryable<Person> People(CMSDataContext Db)
        {
            return Db.People.Where(p => p.Tags.Any(tp => tp.Id == Id));
        }
    }
}
