using System;
using System.Collections.Generic;
using System.Xml;
using System.Web.Mvc;
using System.Xml.Linq;
using UtilityExtensions;
using System.Linq;
using CmsData;

namespace CmsWeb.Models
{
	public class FindResult : ActionResult
	{
		private readonly string name;
		private readonly string id;
		private readonly int? page;
		public FindResult(string id, string name, int? page)
		{
			this.name = name;
			this.page = page;
			this.id = id;
		}
		public override void ExecuteResult(ControllerContext context)
		{
			context.HttpContext.Response.ContentType = "text/xml";
			var settings = new XmlWriterSettings();
			settings.Encoding = new System.Text.UTF8Encoding(false);
			settings.Indent = true;
			using (var w = XmlWriter.Create(context.HttpContext.Response.OutputStream, settings))
			{
				w.WriteStartElement("Results");

				var q = DbUtil.Db.People.Select(p => p);

				if (name.HasValue())
				{
					w.WriteAttributeString("name", name);
					string first;
					string last;
					Person.NameSplit(name, out first, out last);
					if (first.HasValue())
						q = from p in q
						    where (p.LastName.StartsWith(last) || p.MaidenName.StartsWith(last))
						          && (p.FirstName.StartsWith(first) || p.NickName.StartsWith(first) || p.MiddleName.StartsWith(first))
						    select p;
					else
						q = from p in q
						    where p.LastName.StartsWith(last) || p.MaidenName.StartsWith(last)
						    select p;
				}
				else
				{
					var ph = id.GetDigits().PadLeft(10, '0');
					var p7 = ph.Substring(3);
					var ac = ph.Substring(0, 3);
					var qa = from p in DbUtil.Db.People
							 where p.DeceasedDate == null
							 where p.Family.HomePhoneLU.StartsWith(p7)
								|| p.CellPhoneLU.StartsWith(p7)
							 where p.CellPhoneAC == ac || p.Family.HomePhoneAC == ac

							 select p;
					var qb = from kc in DbUtil.Db.CardIdentifiers
							 where kc.Id == id
							 select kc.Person;
					q = qa.Union(qb);
				}

				var q2 = from p in DbUtil.Db.People
						 where q.Any(pp => pp.FamilyId == p.FamilyId)
						 orderby p.Family.HeadOfHousehold.Name2, p.FamilyId, p.Name2
						 select new SearchInfo2
						 {
							 cell = p.CellPhone,
							 home = p.HomePhone,
							 addr = p.Family.AddressLineOne,
							 age = p.Age,
							 first = p.FirstName,
							 email = p.EmailAddress,
							 last = p.LastName,
							 goesby = p.NickName,
							 gender = p.GenderId,
							 marital = p.MaritalStatusId,
							 dob = p.BirthDate.FormatDate2(),
							 fid = p.FamilyId,
							 pid = p.PeopleId,
							 zip = p.Family.ZipCode,
						 };

				var count = q.Count();
				const int INT_PageSize = 10;
				var startrow = (page ?? 1 - 1) * INT_PageSize;
				if (count > startrow + INT_PageSize)
					w.WriteAttributeString("next", (page + 1).ToString());
				else
					w.WriteAttributeString("next", "");
				if (page > 1)
					w.WriteAttributeString("prev", (page - 1).ToString());
				else
					w.WriteAttributeString("prev", "");

				foreach (var p in q2.Skip(startrow).Take(INT_PageSize))
				{
					w.WriteStartElement("person");
					w.WriteAttributeString("first", p.first);
					w.WriteAttributeString("last", p.last);
					w.WriteAttributeString("goesby", p.goesby);
					w.WriteAttributeString("gender", p.gender.ToString());
					w.WriteAttributeString("marital", p.marital.ToString());
					w.WriteAttributeString("dob", p.dob);
					w.WriteAttributeString("addr", p.addr);
					w.WriteAttributeString("zip", p.zip);
					w.WriteAttributeString("email", p.email);
					w.WriteAttributeString("cell", p.cell.FmtFone());
					w.WriteAttributeString("home", p.home.FmtFone());
					w.WriteAttributeString("age", p.age.ToString());
					w.WriteAttributeString("pid", p.pid.ToString());

					w.WriteEndElement();
				}
				w.WriteEndElement();
			}
		}
	}
}