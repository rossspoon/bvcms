using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using CmsData;
using System.Web.Mvc;
using System.Text;
using System.Configuration;
using UtilityExtensions;
using CMSPresenter;
using System.Threading;
using System.Text.RegularExpressions;

namespace CMSWeb.Models
{
    public class LoveRespectModel
    {
        public int? LoveRespectId { get; set; }
        public LoveRespectModel(int id)
        {
            LoveRespectId = id;
            var q = from sm in DbUtil.Db.LoveRespects
                    where sm.Id == id
                    select new
                    {
                        sm.Him,
                        sm.Her,
                        sm.Organization,
                        sm.HisEmail,
                        sm.HerEmail,
                        sm.HisEmailPreferred,
                        sm.HerEmailPreferred,
                        sm.PreferNight
                    };
            var s = q.Single();
            _Person1 = s.Him;
            _Person2 = s.Her;
            night = s.PreferNight;
            email1 = s.HisEmail;
            email2 = s.HerEmail;
            preferredEmail1 = s.HisEmailPreferred.Value;
            preferredEmail2 = s.HerEmailPreferred.Value;
        }
        public LoveRespectModel()
        {

        }
        public int? night { get; set; }
        public string location { get; set; }
        public int OrgId
        {
            get
            {
                if (night == 3)
                    return DbUtil.Settings("LRWedOrgId").ToInt();
                return DbUtil.Settings("LROthOrgId").ToInt();
            }
        }
        public CmsData.Organization organization
        {
            get { return DbUtil.Db.Organizations.Single(o => o.OrganizationId == OrgId); }
        }

        public int? gender { get; set; }
        public string first1 { get; set; }
        public string lastname1 { get; set; }
        public string dob1 { get; set; }
        private DateTime _dob1;
        public DateTime DOB1 { get { return _dob1; } }
        public string phone1 { get; set; }
        public string homecell1 { get; set; }
        public string email1 { get; set; }
        public bool preferredEmail1 { get; set; }
        public bool shownew1 { get; set; }
        public string addr1 { get; set; }
        public string zip1 { get; set; }
        public string city1 { get; set; }
        public string state1 { get; set; }
        private Person _Person1;
        public Person person1
        {
            get { return _Person1; }
        }

        public string first2 { get; set; }
        public string lastname2 { get; set; }
        public string dob2 { get; set; }
        private DateTime _dob2;
        public DateTime DOB2 { get { return _dob2; } }
        public string phone2 { get; set; }
        public string homecell2 { get; set; }
        public string email2 { get; set; }
        public bool preferredEmail2 { get; set; }
        public bool shownew2 { get; set; }
        public string addr2 { get; set; }
        public string zip2 { get; set; }
        public string city2 { get; set; }
        public string state2 { get; set; }
        private Person _Person2;
        public Person person2
        {
            get { return _Person2; }
        }

        public int Relation { get; set; }

        public int FindMember1()
        {
            return FindMember(phone1, lastname1, first1, DOB1, out _Person1);
        }
        public int FindMember2()
        {
            return FindMember(phone2, lastname2, first2, DOB2, out _Person2);
        }
        internal int FindMember(string phone, string last, string first, DateTime DOB,
            out Person person)
        {
            first = first.Trim();
            last = last.Trim();
            var fone = Util.GetDigits(phone);
            var q = from p in DbUtil.Db.People
                    where (p.FirstName == first || p.NickName == first || p.MiddleName == first)
                    where (p.LastName == last || p.MaidenName == last)
                    where p.BirthDay == DOB.Day && p.BirthMonth == DOB.Month && p.BirthYear == DOB.Year
                    select p;
            var count = q.Count();
            if (count > 1)
                q = from p in q
                    where p.CellPhone.Contains(fone)
                            || p.WorkPhone.Contains(fone)
                            || p.Family.HomePhone.Contains(fone)
                    select p;
            count = q.Count();
            person = null;
            if (count == 1)
                person = q.Single();
            return count;
        }

        public void ValidateModel(ModelStateDictionary ModelState)
        {
            int d;

            if (night == 0)
                ModelState.AddModelError("night", "Select a preferred evening");
            if (!location.HasValue())
                ModelState.AddModelError("location", "Please tell us a location");

            if (!first1.HasValue())
                ModelState.AddModelError("first1", "first name required");
            if (!lastname1.HasValue())
                ModelState.AddModelError("lastname1", "last name required");
            else if (lastname1.ToUpper() == lastname1 || lastname1.ToLower() == lastname1)
                ModelState.AddModelError("lastname1", "Please use Proper Casing");
            if (!Util.DateValid(dob1, out _dob1))
                ModelState.AddModelError("dob1", "valid birth date required");
            d = phone1.GetDigits().Length;
            if (d != 7 && d != 10)
                ModelState.AddModelError("phone1", "7 or 10 digits");
            else if (!homecell1.HasValue())
                ModelState.AddModelError("phone1", "pick a phone type");
            if (!email1.HasValue() || !Util.ValidEmail(email1))
                ModelState.AddModelError("email1", "Please specify a valid email address.");
            if (shownew1)
            {
                if (!addr1.HasValue())
                    ModelState.AddModelError("addr1", "need address");
                if (zip1.GetDigits().Length != 5)
                    ModelState.AddModelError("zip1", "need 5 digit zip");
                if (!city1.HasValue())
                    ModelState.AddModelError("city1", "need city");
                if (!state1.HasValue())
                    ModelState.AddModelError("state1", "need state");
            }

            if (!first2.HasValue())
                ModelState.AddModelError("first2", "first name required");
            if (!lastname2.HasValue())
                ModelState.AddModelError("lastname2", "last name required");
            else if (lastname2.ToUpper() == lastname2 || lastname2.ToLower() == lastname2)
                ModelState.AddModelError("lastname2", "Please use Proper Casing");
            if (!Util.DateValid(dob2, out _dob2))
                ModelState.AddModelError("dob2", "valid birth date required");
            d = phone2.GetDigits().Length;
            if (d != 7 && d != 10)
                ModelState.AddModelError("phone2", "7 or 10 digits");
            else if (!homecell2.HasValue())
                ModelState.AddModelError("phone2", "pick a phone type");
            if (!email2.HasValue() || !Util.ValidEmail(email2))
                ModelState.AddModelError("email2", "Please specify a valid email address.");
            if (shownew2)
            {
                if (!addr2.HasValue())
                    ModelState.AddModelError("addr2", "need address");
                if (zip2.GetDigits().Length != 5)
                    ModelState.AddModelError("zip2", "need 5 digit zip");
                if (!city2.HasValue())
                    ModelState.AddModelError("city2", "need city");
                if (!state2.HasValue())
                    ModelState.AddModelError("state2", "need state");
            }

            if (Relation > 0 && Relation < 3
                && homecell1 == "h" && homecell2 == "h"
                && phone1.GetDigits() != phone2.GetDigits())
                ModelState.AddModelError("phone2", "Home phones cannot be different");
            if (Relation == 0)
                ModelState.AddModelError("Relation", "Please select a relationship");
        }
        public static IEnumerable<SelectListItem> Relations()
        {
            return new List<SelectListItem> 
            {
                new SelectListItem { Value="0", Text="(describe your relationship)" },
                new SelectListItem { Value="1", Text="Married under 5 years" },
                new SelectListItem { Value="2", Text="Married 5 years or more" },
                new SelectListItem { Value="3", Text="Engaged" },
                new SelectListItem { Value="4", Text="Might as well be married" },
                new SelectListItem { Value="5", Text="Seriously dating" },
            };
        }
        public int MaritalStatus
        {
            get
            {
                if (Relation <= 2)
                    return 20;
                return 10;
            }
        }
        public static IEnumerable<SelectListItem> Nights()
        {
            return new List<SelectListItem> 
            {
                new SelectListItem { Value="0", Text="(select a preferred night)" },
                new SelectListItem { Value="1", Text="Monday" },
                new SelectListItem { Value="2", Text="Tuesday" },
                new SelectListItem { Value="3", Text="Wednesday" },
                new SelectListItem { Value="4", Text="Thursday" },
                new SelectListItem { Value="5", Text="Friday" },
                new SelectListItem { Value="6", Text="Saturday" },
            };
        }
        internal void EnrollInClass(Person person)
        {
            var oid = OrgId;
            var member = DbUtil.Db.OrganizationMembers.SingleOrDefault(om =>
                om.OrganizationId == oid && om.PeopleId == person.PeopleId);
            if (member == null)
                OrganizationController.InsertOrgMembers(
                    oid,
                    person.PeopleId,
                    (int)OrganizationMember.MemberTypeCode.Member,
                    DateTime.Today, null, false);
        }

        internal void AddPeople()
        {
            var married = Relation < 3;
            if (person1 != null && person2 != null)
                return;
            if (married && person1 != null && person2 == null)
                _Person2 = AddPersonToPerson(person1, first2, lastname2, dob2, phone2, homecell2, email2);
            else if (married && person1 == null && person2 != null)
                _Person1 = AddPersonToPerson(person2, first1, lastname1, dob1, phone1, homecell1, email1);
            else if (married && person1 == null && person2 == null)
            {
                _Person1 = AddPerson(1, first1, lastname1, dob1, addr1, city1, state1, zip1, phone1, homecell1, married, email1);
                _Person2 = AddPersonToPerson(person1, first2, lastname2, dob2, phone2, homecell2, email2);
            }
            else if (!married && person1 != null && person2 == null)
                _Person2 = AddPerson(2, first2, lastname2, dob2, addr2, city2, state2, zip2, phone2, homecell2, false, email2);
            else if (!married && person1 == null && person2 != null)
                _Person1 = AddPerson(1, first1, lastname1, dob1, addr1, city1, state1, zip1, phone1, homecell1, false, email1);
            else if (!married && person1 == null && person2 == null)
            {
                _Person1 = AddPerson(1, first1, lastname1, dob1, addr1, city1, state1, zip1, phone1, homecell1, married, email1);
                _Person2 = AddPerson(2, first2, lastname2, dob2, addr2, city2, state2, zip2, phone2, homecell2, married, email2);
            }
        }
        internal Person AddPersonToPerson(Person p, string first, string last, string dob, string phone, string homecell, string email)
        {
            var np = Person.Add(p.Family, 10,
                null, first, null, last, dob, true, p.GenderId == 1 ? 2 : 1,
                    DbUtil.Settings("SmlOrigin").ToInt(),
                    DbUtil.Settings("SmlEntry").ToInt());
            switch (homecell)
            {
                case "h":
                    p.Family.HomePhone = phone.GetDigits();
                    break;
                case "c":
                    np.CellPhone = phone.GetDigits();
                    break;
            }
            np.EmailAddress = email;
            RecRegModel.FixTitle(np);
            DbUtil.Db.SubmitChanges();
            return np;
        }
        internal Person AddPerson(int gender, string first, string last, string dob,
            string addr, string city, string state, string zip, string phone, string homecell, bool married, string email)
        {
            var f = new Family
            {
                AddressLineOne = addr,
                CityName = city,
                StateCode = state,
                ZipCode = zip,
            };

            var np = Person.Add(f, 10, null,
                    first,
                    null,
                    last,
                    dob, married, gender,
                    DbUtil.Settings("SmlOrigin").ToInt(),
                    DbUtil.Settings("SmlEntry").ToInt());
            switch (homecell)
            {
                case "h":
                    f.HomePhone = phone.GetDigits();
                    break;
                case "c":
                    np.CellPhone = phone.GetDigits();
                    break;
            }
            np.EmailAddress = email;
            RecRegModel.FixTitle(np);
            DbUtil.Db.SubmitChanges();
            return np;
        }
        public string PrepareSummaryText()
        {
            var sb = new StringBuilder();
            sb.Append("<table>");
            sb.AppendFormat("<tr><td>Preferred night:</td><td>{0}</td></tr>\n", NightWord(night));
            sb.AppendFormat("<tr><td>Church Location:</td><td>{0}</td></tr>\n", location);
            sb.Append("<tr><th>---------</th></tr>\n");

            sb.AppendFormat("<tr><th colspan='2'>His Information:</th></tr>\n");
            sb.AppendFormat("<tr><td>First:</td><td>{0}</td></tr>\n", first1);
            sb.AppendFormat("<tr><td>Last:</td><td>{0}</td></tr>\n", lastname1);
            sb.AppendFormat("<tr><td>Birthday:</td><td>{0:d}</td></tr>\n", DOB1);
            sb.AppendFormat("<tr><td>Addr:</td><td>{0}</td></tr>\n", addr1);
            sb.AppendFormat("<tr><td>City:</td><td>{0}</td></tr>\n", city1);
            sb.AppendFormat("<tr><td>State:</td><td>{0}</td></tr>\n", state1);
            sb.AppendFormat("<tr><td>Zip:</td><td>{0}</td></tr>\n", zip1);
            sb.AppendFormat("<tr><td>Phone:</td><td>{0}</td></tr>\n", phone1);
            sb.AppendFormat("<tr><td>Home/Cell:</td><td>{0}</td></tr>\n", homecell1);
            sb.AppendFormat("<tr><td>Email:</td><td>{0}</td></tr>\n", email1);
            sb.Append("<tr><th>---------</th></tr>\n");

            sb.AppendFormat("<tr><th colspan='2'>Her Information:</th></tr>\n");
            sb.AppendFormat("<tr><td>First:</td><td>{0}</td></tr>\n", first2);
            sb.AppendFormat("<tr><td>Last:</td><td>{0}</td></tr>\n", lastname2);
            sb.AppendFormat("<tr><td>Birthday:</td><td>{0:d}</td></tr>\n", DOB2);
            sb.AppendFormat("<tr><td>Addr:</td><td>{0}</td></tr>\n", addr2);
            sb.AppendFormat("<tr><td>City:</td><td>{0}</td></tr>\n", city2);
            sb.AppendFormat("<tr><td>State:</td><td>{0}</td></tr>\n", state2);
            sb.AppendFormat("<tr><td>Zip:</td><td>{0}</td></tr>\n", zip2);
            sb.AppendFormat("<tr><td>Phone:</td><td>{0}</td></tr>\n", phone2);
            sb.AppendFormat("<tr><td>Home/Cell:</td><td>{0}</td></tr>\n", homecell2);
            sb.AppendFormat("<tr><td>Email:</td><td>{0}</td></tr>\n", email2);
            sb.Append("<tr><th>---------</th></tr>\n");

            sb.AppendFormat("<tr><td>Relationship:</td><td>{0}</td></tr>\n", RelationDesc(Relation));
            sb.Append("</table>");

            return sb.ToString();
        }
        public static string NightWord(int? d)
        {
            var q = from i in Nights()
                    where i.Value == d.ToString()
                    select i.Text;
            return q.SingleOrDefault();
        }
        public static string RelationDesc(int? rel)
        {
            var q = from i in Relations()
                    where i.Value == rel.ToString()
                    select i.Text;
            return q.SingleOrDefault();
        }
        public static System.Collections.IEnumerable LoveRespectList(int queryid, int maximumRows)
        {
            var Db = DbUtil.Db;
            var qB = Db.LoadQueryById(queryid);
            var q = Db.People.Where(qB.Predicate());
            var q2 = from p in q
                     let bfm = Db.OrganizationMembers.SingleOrDefault(om => om.OrganizationId == Util.CurrentOrgId && om.PeopleId == p.PeopleId)
                     let lr2 = p.HerLoveRespects.OrderByDescending(sm => sm.Created).FirstOrDefault()
                     let lr1 = p.HisLoveRespects.OrderByDescending(sm => sm.Created).FirstOrDefault()
                     let loverespect = lr1 != null ? lr1 : lr2 != null ? lr2 : null
                     orderby loverespect.Id, p.FamilyId, p.GenderId
                     select new
                     {
                         LoveRespectId = loverespect != null ? (int?)loverespect.Id : null,
                         Relationship = loverespect != null? RelationDesc(loverespect.Relationship) : "",
                         Night = NightWord(loverespect.PreferNight),
                         Married = p.MaritalStatus.Description,
                         PeopleId = p.PeopleId,
                         Title = p.TitleCode,
                         FirstName = p.NickName == null ? p.FirstName : p.NickName,
                         LastName = p.LastName,
                         Address = p.PrimaryAddress,
                         Address2 = p.PrimaryAddress2,
                         City = p.PrimaryCity,
                         State = p.PrimaryState,
                         Zip = p.PrimaryZip.FmtZip(),
                         Email = p.EmailAddress,
                         BirthDate = Util.FormatBirthday(p.BirthYear, p.BirthMonth, p.BirthDay),
                         EnrollDate = bfm.EnrollmentDate.FormatDate(),
                         RegDate = loverespect != null? loverespect.Created.FormatDateTm() : "",
                         HomePhone = p.HomePhone.FmtFone(),
                         CellPhone = p.CellPhone.FmtFone(),
                         WorkPhone = p.WorkPhone.FmtFone(),
                         MemberStatus = p.MemberStatus.Description,
                         Age = p.Age.ToString(),
                         MemberType = bfm.MemberType.Description,
                     };
            return q2.Take(maximumRows);
        }
    }
}
