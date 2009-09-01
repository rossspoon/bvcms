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

namespace CMSWeb.Models
{
    public class RegisterModel
    {
        public string first {get; set;}
        public string nickname {get; set;}
        public string lastname {get; set;}
        public string dob {get; set;}
        private DateTime _dob;
        public DateTime DOB { get { return _dob;} }
		public int? gender { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip {get; set;}
        public string homephone {get; set;}
        public string cellphone { get; set; }
        public string email { get; set; }
        public int? position { get; set; }
        public bool married { get; set; }
        public int? campusid { get; set; }

        public string PrepareSummaryText()
        {
            var sb = new StringBuilder();
            sb.Append("<table>\n");
            sb.AppendFormat("<tr><td>First</td><td>{0}</td></tr>\n", first);
            sb.AppendFormat("<tr><td>Nick</td><td>{0}</td></tr>\n", nickname);
            sb.AppendFormat("<tr><td>Last</td><td>{0}</td></tr>\n", lastname);
            sb.AppendFormat("<tr><td>DOB</td><td>{0:d}</td></tr>\n", DOB);
            sb.AppendFormat("<tr><td>Gender</td><td>{0}</td></tr>\n", gender == 1 ? "M" : "F");
            sb.AppendFormat("<tr><td>Maried</td><td>{0}</td></tr>\n", married);
            sb.AppendFormat("<tr><td>Position</td><td>{0}</td></tr>\n", position == 10 ? "Primary" : position == 20 ? "Secondary" : "Child");
            sb.AppendFormat("<tr><td>Addr1</td><td>{0}</td></tr>\n", address1);
            sb.AppendFormat("<tr><td>Addr2</td><td>{0}</td></tr>\n", address2);
            sb.AppendFormat("<tr><td>City</td><td>{0}</td></tr>\n", city);
            sb.AppendFormat("<tr><td>State</td><td>{0}</td></tr>\n", state);
            sb.AppendFormat("<tr><td>Zip</td><td>{0}</td></tr>\n", zip);
            sb.AppendFormat("<tr><td>HomePhone</td><td>{0}</td></tr>\n", homephone.FmtFone());
            sb.AppendFormat("<tr><td>CellPhone</td><td>{0}</td></tr>\n", cellphone.FmtFone());
            sb.AppendFormat("<tr><td>Email</td><td>{0}</td></tr>\n", email);
            sb.Append("</table>\n");

            return sb.ToString();
        }

        public IEnumerable<SelectListItem> StateList()
        {
            var q = from r in DbUtil.Db.StateLookups
                    select new SelectListItem
                    {
                        Text = r.StateCode,
                        Selected = r.StateCode == "TN",
                    };
            return q;
        }
        public IQueryable<CmsData.Person> FindMember()
        {
            first = first.Trim();
            lastname = lastname.Trim();
            homephone = Util.GetDigits(homephone);
            var q = from p in DbUtil.Db.People
                    where (p.FirstName == first || p.NickName == first || p.MiddleName == first)
                    where (p.LastName == lastname || p.MaidenName == lastname)
                    where p.BirthDay == DOB.Day && p.BirthMonth == DOB.Month && p.BirthYear == DOB.Year
                    where p.GenderId == gender
                    select p;
            var count = q.Count();
            if (count > 1)
                q = from p in q
                    where p.CellPhone.Contains(homephone)
                            || p.WorkPhone.Contains(homephone)
                            || p.Family.HomePhone.Contains(homephone)
                            || p.CellPhone.Contains(cellphone)
                            || p.WorkPhone.Contains(cellphone)
                            || p.Family.HomePhone.Contains(cellphone)
                    select p;
            count = q.Count();

            return q;
        }

        public void ValidateModel1(ModelStateDictionary ModelState)
        {
            ValidateModel2(ModelState);
            if (!address1.HasValue())
                ModelState.AddModelError("address1", "address1 required");
            if (!city.HasValue())
                ModelState.AddModelError("city", "city required");
            if (!zip.HasValue())
                ModelState.AddModelError("zip", "zip required");
            if (!(homephone.HasValue() || cellphone.HasValue()))
                ModelState.AddModelError("phone", "need at least one phone #");
        }
        public void ValidateModel2(ModelStateDictionary ModelState)
        {
            if (!first.HasValue())
                ModelState.AddModelError("first", "first name required");
            if (!lastname.HasValue())
                ModelState.AddModelError("lastname", "last name required");
            if (!Util.DateValid(dob, out _dob))
                ModelState.AddModelError("dob", "valid birth date required");

            if (!gender.HasValue)
                ModelState.AddModelError("gender2", "gender required");
            if (!position.HasValue)
                ModelState.AddModelError("position2", "position required");
            var d = cellphone.GetDigits().Length;
            if (cellphone.HasValue() && (d != 7 && d != 10))
                ModelState.AddModelError("cellphone", "7 or 10 digits");
            if ((((string)HttpContext.Current.Session["email"]).HasValue() == false || email.HasValue()) 
                    && !Util.ValidEmail(email))
                ModelState.AddModelError("email", "Please specify a valid email address.");
        }
        public Person SaveFirstPerson()
        {
            var f = new Family
            {
                 AddressLineOne = address1,
                 AddressLineTwo = address2,
                 CityName = city,
                 StateCode = state,
                 ZipCode = zip,
                 HomePhone = homephone.GetDigits(),
            };
            var p = Person.Add(f, position.Value, 
                null, first, nickname, lastname, dob, married, gender.Value, 0, null);
            p.CellPhone = cellphone.GetDigits();
            p.EmailAddress = email;
            p.CampusId = campusid ?? DbUtil.Settings("DefaultCampusId").ToInt2();
            DbUtil.Db.SubmitChanges();
            return p;
        }
        public void SavePerson(int FamilyId)
        {
            var f = DbUtil.Db.Families.Single(fam => fam.FamilyId == FamilyId);
            var p = Person.Add(f, position.Value, 
                null, first, nickname, lastname, dob, married, gender.Value, 0, null);
            p.CellPhone = cellphone.GetDigits();
            p.EmailAddress = email;
            p.CampusId = campusid ?? DbUtil.Settings("DefaultCampusId").ToInt2();
            RecRegModel.FixTitle(p);
            DbUtil.Db.SubmitChanges();
        }
    }
}
