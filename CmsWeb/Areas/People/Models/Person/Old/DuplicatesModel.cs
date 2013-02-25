using CmsData;

namespace CmsWeb.Areas.People.Models.Person
{
    public class DuplicatesModel
    {
        private int PeopleId;
        public CmsData.Person person { get; set; }
        public DuplicatesModel(int id)
        {
            PeopleId = id;
            person = DbUtil.Db.LoadPersonById(id);
        }
    }
}
