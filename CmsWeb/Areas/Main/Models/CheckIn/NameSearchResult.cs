using System;
using System.Collections.Generic;
using System.Xml;
using System.Web.Mvc;
using System.Xml.Linq;
using UtilityExtensions;
using System.Linq;
using CmsData;

namespace CMSWeb.Models
{
    public class NameSearchResult : ActionResult
    {
        private string name;
        private int page;
        public NameSearchResult(string name, int page)
        {
            this.name = name;
            this.page = page;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "text/xml";
            var settings = new XmlWriterSettings();
            settings.Encoding = new System.Text.UTF8Encoding(false);
            using (var w = XmlWriter.Create(context.HttpContext.Response.OutputStream, settings))
            {
                w.WriteStartElement("Results");
                w.WriteAttributeString("name", name);

                string first;
                string last;
                var q = DbUtil.Db.People.Select(p => p);
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

                var q2 = from p in q
                         orderby p.Name2, p.PeopleId
                         select new SearchInfo
                         {
                             cell = p.CellPhone,
                             home= p.HomePhone,
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
                             zip = p.Family.ZipCode,
                         };

                var count = q.Count();
                const int INT_PageSize = 10;
                var startrow = (page - 1) * INT_PageSize;
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
                    w.WriteAttributeString("fid", p.fid.ToString());
                    w.WriteAttributeString("display", p.GetDisplay());
                    w.WriteEndElement();
                }
                w.WriteEndElement();
            }
        }
    }
}