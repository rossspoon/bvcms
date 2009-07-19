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
        public string workphone { get; set; }
        public string cellphone { get; set; }
        public string email { get; set; }
        public int? position { get; set; }
        public bool married { get; set; }

        public string PrepareSummaryText()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("First:\t{0}\n", first);
            sb.AppendFormat("Nick:\t{0}\n", nickname);
            sb.AppendFormat("Last:\t{0}\n", lastname);
            sb.AppendFormat("DOB:\t{0:d}\n", DOB);
            sb.AppendFormat("Gender:\t{0}\n", gender == 1 ? "M" : "F");
            sb.AppendFormat("Maried:\t{0}\n", married);
            sb.AppendFormat("Position:\t{0}\n", position == 10 ? "Primary" : position == 20 ? "Secondary" : "Child");
            sb.AppendFormat("Addr1:\t{0}\n", address1);
            sb.AppendFormat("Addr2:\t{0}\n", address2);
            sb.AppendFormat("City:\t{0}\n", city);
            sb.AppendFormat("State:\t{0}\n", state);
            sb.AppendFormat("Zip:\t{0}\n", zip);
            sb.AppendFormat("HomePhone:\t{0}\n", homephone.FmtFone());
            sb.AppendFormat("WorkPhone:\t{0}\n", workphone.FmtFone());
            sb.AppendFormat("CellPhone:\t{0}\n", cellphone.FmtFone());
            sb.AppendFormat("Email:\t{0}\n", email);

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
            homephone = Util.GetDigits(homephone);
            var q = from p in DbUtil.Db.People
                    where (p.LastName.StartsWith(lastname) || p.MaidenName.StartsWith(lastname))
                            && (p.FirstName.StartsWith(first)
                            || p.NickName.StartsWith(first)
                            || p.MiddleName.StartsWith(first))
                    where p.CellPhone.Contains(homephone)
                            || p.WorkPhone.Contains(workphone)
                            || p.Family.HomePhone.Contains(homephone)
                            || p.CellPhone.Contains(cellphone)
                            || p.WorkPhone.Contains(cellphone)
                            || p.Family.HomePhone.Contains(cellphone)
                    where p.BirthDay == DOB.Day && p.BirthMonth == DOB.Month && p.BirthYear == DOB.Year
                    select p;
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
            if (!(homephone.HasValue() || cellphone.HasValue() || workphone.HasValue()))
                ModelState.AddModelError("phone", "need at least one phone #");
        }
        public void ValidateModel2(ModelStateDictionary ModelState)
        {
            if (!first.HasValue())
                ModelState.AddModelError("first", "first name required");
            if (!lastname.HasValue())
                ModelState.AddModelError("lastname", "last name required");
            if (!DateTime.TryParse(dob, out _dob))
                ModelState.AddModelError("dob", "valid birth date required");
            else if (_dob.Year == DateTime.Now.Year)
                ModelState.AddModelError("dob", "valid birth year required");
            if (!gender.HasValue)
                ModelState.AddModelError("gender2", "gender required");
            if (!position.HasValue)
                ModelState.AddModelError("position2", "position required");
            var d = cellphone.GetDigits().Length;
            if (cellphone.HasValue() && (d != 7 && d != 10))
                ModelState.AddModelError("cellphone", "7 or 10 digits");
            d = workphone.GetDigits().Length;
            if (workphone.HasValue() && (d != 7 && d != 10))
                ModelState.AddModelError("workphone", "7 or 10 digits");
            if (email.HasValue() && !Util.ValidEmail(email))
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
                 HomePhone = homephone,
            };
            var p = Person.Add(f, position.Value, 
                null, first, nickname, lastname, dob, married, gender.Value, 0, null);
            DbUtil.Db.SubmitChanges();
            return p;
        }
        public void SavePerson(int FamilyId)
        {
            var f = DbUtil.Db.Families.Single(fam => fam.FamilyId == FamilyId);
            var p = Person.Add(f, position.Value, 
                null, first, nickname, lastname, dob, married, gender.Value, 0, null);
            DbUtil.Db.SubmitChanges();
        }
    }
}
