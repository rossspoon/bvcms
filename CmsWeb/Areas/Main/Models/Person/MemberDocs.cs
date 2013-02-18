using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using System.Web.Mvc;
using UtilityExtensions;
using System.Data.Linq.SqlClient;
using System.Data.Linq;
using System.Text;

namespace CmsWeb.Models
{
    public class MemberDocs
    {
		public MemberDocs()
		{
			Db = DbUtil.Db;
		}
		public string Purpose { get; set; }
		private CMSDataContext Db;
        public int PeopleId { get; set; }
		private Person _Person;
		public Person person
		{
			get
			{
				if (_Person == null)
					_Person = DbUtil.Db.LoadPersonById(PeopleId);
				return _Person;
			}
		}
		public int Count { get; set; }
		public class DocInfo
		{
			public int Id { get; set; }
			public DateTime? DocDate { get; set; }
			public int? ThumbId { get; set; }
			public int? Docid { get; set; }
			public string Uploader { get; set; }
			public bool? IsDocument { get; set; }
			public int PeopleId { get; set; }
			public string Name { get; set; }
			public string FormName { get; set; }
		}
        public IEnumerable<DocInfo> DocForms()
        {
            var q = Db.MemberDocForms.Where(f => f.PeopleId == PeopleId && (Purpose == null || f.Purpose == Purpose));
            Count = q.Count();
            q = q.OrderBy(f => f.DocDate);
            var q2 = q.Select(f =>
                   new DocInfo
                   {
                       DocDate = f.DocDate,
                       Id = f.Id,
                       ThumbId = f.SmallId,
                       Docid = f.MediumId,
                       IsDocument = f.IsDocument,
					   PeopleId = f.PeopleId,
					   Name = f.Person.Name,
                       FormName = f.Name
                   });
            return q2;
        }

    }
}
