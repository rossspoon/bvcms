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
using CMSWebCommon.Models;

namespace CMSWeb.Models
{
    public class RegisterModel
    {
        public int? thisday { get; set; }
        public int? familyid { get; set; }
        public string first { get; set; }
        public string nickname { get; set; }
        public string last { get; set; }
        public string dob { get; set; }
        private DateTime _Birthday;
        private DateTime birthday
        {
            get
            {
                if (_Birthday == DateTime.MinValue)
                    Util.DateValid(dob, out _Birthday);
                return _Birthday;
            }
        }
        public int? gender { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string phone { get; set; }
        public string hcellphone { get; set; }
        public string cellphone { get; set; }
        public string email { get; set; }
        public int? married { get; set; }
        public string School { get; set; }
        public string Grade { get; set; }
        public int? campusid { get; set; }
        public int? org { get; set; }
        private bool? _Existingfamily;
        public bool? existingfamily
        {
            get
            {
                return _Existingfamily;
            }
            set
            {
                _Existingfamily = value;
            }
        }
        private Person _person;
        public Person person
        {
            get
            {
                return _person;
            }
        }
        private Person _headofhousehold;
        public Person HeadOfHousehold
        {
            get
            {
                if (_headofhousehold == null && familyid.HasValue)
                {
                    var q = from f in DbUtil.Db.Families
                            where f.FamilyId == familyid
                            select f.HeadOfHousehold;
                    _headofhousehold = q.Single();
                }
                return _headofhousehold;
            }
        }

        public string PrepareSummaryText()
        {
            var sb = new StringBuilder();
            sb.Append("<table>\n");
            sb.AppendFormat("<tr><td>First</td><td>{0}</td></tr>\n", person.FirstName);
            sb.AppendFormat("<tr><td>Nick</td><td>{0}</td></tr>\n", person.NickName);
            sb.AppendFormat("<tr><td>Last</td><td>{0}</td></tr>\n", person.LastName);
            sb.AppendFormat("<tr><td>DOB</td><td>{0:d}</td></tr>\n", person.GetBirthdate().Value);
            sb.AppendFormat("<tr><td>Gender</td><td>{0}</td></tr>\n", person.GenderId == 1 ? "M" : "F");
            var MarStatus = (from ms in MaritalStatus()
                             where ms.Value == person.MaritalStatusId.ToString()
                             select ms.Text).Single();
            sb.AppendFormat("<tr><td>Marital Status</td><td>{0}</td></tr>\n", MarStatus);
            sb.AppendFormat("<tr><td>Addr1</td><td>{0}</td></tr>\n", person.PrimaryAddress);
            sb.AppendFormat("<tr><td>Addr2</td><td>{0}</td></tr>\n", person.PrimaryAddress2);
            sb.AppendFormat("<tr><td>City</td><td>{0}</td></tr>\n", person.PrimaryCity);
            sb.AppendFormat("<tr><td>State</td><td>{0}</td></tr>\n", person.PrimaryState);
            sb.AppendFormat("<tr><td>Zip</td><td>{0}</td></tr>\n", person.PrimaryZip.FmtZip());
            sb.AppendFormat("<tr><td>HomePhone</td><td>{0}</td></tr>\n", person.Family.HomePhone.FmtFone());
            sb.AppendFormat("<tr><td>CellPhone</td><td>{0}</td></tr>\n", person.CellPhone.FmtFone());
            sb.AppendFormat("<tr><td>Email</td><td>{0}</td></tr>\n", person.EmailAddress);
            sb.Append("</table>\n");

            return sb.ToString();
        }

        public IEnumerable<SelectListItem> MaritalStatus()
        {
            return new List<SelectListItem> 
            {
                new SelectListItem { Value="0", Text="(not specified)" },
                new SelectListItem { Value="10", Text="Single" },
                new SelectListItem { Value="20", Text="Married" },
                new SelectListItem { Value="30", Text="Separated" },
                new SelectListItem { Value="40", Text="Divorced" },
                new SelectListItem { Value="50", Text="Widowed" },
            };
        }
        public static IEnumerable<SelectListItem> StateList()
        {
            string defstate = DbUtil.Settings("DefaultState", "TN");
            var q = from r in DbUtil.Db.StateLookups
                    select new SelectListItem
                    {
                        Text = r.StateCode,
                        Selected = r.StateCode == defstate,
                    };
            return q;
        }
        public IEnumerable<SelectListItem> OrgList()
        {
            var q = from o in DbUtil.Db.Organizations
                    let Hour = DbUtil.Db.GetTodaysMeetingHour(o.OrganizationId, thisday)
                    where o.OrganizationStatusId == (int)CmsData.Organization.OrgStatusCode.Active
                    where campusid == null || campusid == o.CampusId
                    where o.CanSelfCheckin == true
                    where Hour != null
                    orderby o.OnLineCatalogSort, o.OrganizationName
                    select new SelectListItem
                    {
                        Text = "{0} ({1:h:mm})".Fmt(o.FullName, Hour),
                        Value = o.OrganizationId.ToString(),
                    };
            var list = q.ToList();
            list.Insert(0, new SelectListItem { Text = "(not specified)", Value = "0" });
            return list;
        }
        public int FindMember()
        {
            int count;
            _person = SearchPeopleModel.FindPerson(phone, first, last, birthday, out count);
            if (count > 1)
                _person = SearchPeopleModel.FindPerson(cellphone, first, last, birthday, out count);
            return count;
        }
        public int FindFamily()
        {
            var ph = Util.GetDigits(phone).PadLeft(10, '0');
            var p7 = ph.Substring(3);
            var ac = ph.Substring(0, 3);

            if (last.HasValue())
                last = last.Trim();
            var q = from f in DbUtil.Db.Families
                    where f.HeadOfHousehold.LastName == last
                    where f.People.Any(p => p.CellPhoneLU == p7)
                            || f.HomePhoneLU == p7
                    select f.HeadOfHousehold;
            var count = q.Count();
            _headofhousehold = null;
            if (count >= 1)
            {
                _headofhousehold = q.First();
                return 1;
            }
            return 0;
        }

        public void ValidateModel1(ModelStateDictionary modelState)
        {
            ValidateModel2(modelState);
            if (!address1.HasValue())
                modelState.AddModelError("address1", "address1 required");
            if (!city.HasValue())
                modelState.AddModelError("city", "city required");
            if (!zip.HasValue())
                modelState.AddModelError("zip", "zip required");
            if (!(phone.HasValue() || cellphone.HasValue()))
                modelState.AddModelError("phone", "need at least one phone #");
        }
        public void ValidateModel2(ModelStateDictionary modelState)
        {
            SearchPeopleModel.ValidateFindPerson(modelState, first, last, birthday, phone);
            if (modelState.IsValid && FindMember() >= 1)
                modelState.AddModelError("first", "Already Registered");

            if (!gender.HasValue)
                modelState.AddModelError("gender", "gender required");
            if ((married ?? 0) == 0)
                modelState.AddModelError("married", "select marital status");
            var d = cellphone.GetDigits().Length;
            if (cellphone.HasValue() && d != 10)
                modelState.AddModelError("cellphone", "need 10 digits");

            if (!Util.ValidEmail(email))
                modelState.AddModelError("email", "Please specify a valid email address.");
            if (org == 0)
                modelState.AddModelError("org", "Please choose an organization");
        }
        public void ValidateModel3(ModelStateDictionary ModelState)
        {
            if (!last.HasValue())
                ModelState.AddModelError("last", "last name required");
            var d = phone.GetDigits().Length;
            if (d != 10)
                ModelState.AddModelError("phone", "need 10 digits");
        }
        public void SaveFirstPerson()
        {
            var f = new Family
            {
                AddressLineOne = address1,
                AddressLineTwo = address2,
                CityName = city,
                StateCode = state,
                ZipCode = zip,
                HomePhone = phone.GetDigits(),
            };
            var p = Person.Add(f, (int)Family.PositionInFamily.PrimaryAdult,
                null, first, nickname, last, dob, false, gender.Value,
                DbUtil.Settings("RegOrigin", "0").ToInt(), null);
            var age = p.GetAge();
            var pos = (int)Family.PositionInFamily.PrimaryAdult;
            if (age < 18 && p.MaritalStatusId == (int)Person.MaritalStatusCode.Single)
                pos = (int)Family.PositionInFamily.Child;
            p.PositionInFamilyId = pos;
            p.MaritalStatusId = married.Value;
            p.FixTitle();
            p.CellPhone = cellphone.GetDigits();
            p.EmailAddress = email.Trim();
            p.CampusId = campusid ?? DbUtil.Settings("DefaultCampusId", "").ToInt2();
            DbUtil.Db.SubmitChanges();
            if (org.HasValue)
                RecordAttend(p.PeopleId, org.Value);
            _person = p;
        }
        public void SavePerson(int FamilyId)
        {
            var f = DbUtil.Db.Families.Single(fam => fam.FamilyId == FamilyId);
            var p = Person.Add(f, (int)Family.PositionInFamily.PrimaryAdult,
                null, first, nickname, last, dob, false, gender.Value, 0, null);
            var age = p.GetAge();
            var pos = (int)Family.PositionInFamily.PrimaryAdult;
            if (age < 18 && p.MaritalStatusId == (int)Person.MaritalStatusCode.Single)
                pos = (int)Family.PositionInFamily.Child;
            p.PositionInFamilyId = pos;

            p.CellPhone = cellphone.GetDigits();
            p.MaritalStatusId = married.Value;
            p.FixTitle();
            p.CellPhone = cellphone.GetDigits();
            email = email.Trim();
            if (email.HasValue())
                p.EmailAddress = email;
            else
                p.EmailAddress = (string)HttpContext.Current.Session["email"];
            p.CampusId = campusid ?? DbUtil.Settings("DefaultCampusId", "").ToInt2();
            DbUtil.Db.SubmitChanges();
            if (org.HasValue)
                RecordAttend(p.PeopleId, org.Value);
            _person = p;
        }
        public void RecordAttend(int PeopleId, int OrgId)
        {
            var ret = (from o in DbUtil.Db.Organizations
                       where o.OrganizationId == OrgId
                       select new { o.SchedTime.Value.TimeOfDay, o.Location }).Single();
            var dt = Util.Now.Date;
            dt = dt.Add(ret.TimeOfDay);

            var meeting = (from m in DbUtil.Db.Meetings
                           where m.OrganizationId == OrgId
                           where m.MeetingDate == dt
                           select m).SingleOrDefault();

            if (meeting == null)
            {
                meeting = new CmsData.Meeting
                {
                    OrganizationId = OrgId,
                    MeetingDate = dt,
                    CreatedDate = DateTime.Now,
                    CreatedBy = Util.UserId1,
                    GroupMeetingFlag = false,
                    Location = ret.Location,
                };
                DbUtil.Db.Meetings.InsertOnSubmit(meeting);
                DbUtil.Db.SubmitChanges();
            }
            Attend.RecordAttendance(PeopleId, meeting.MeetingId, true);
            DbUtil.Db.UpdateMeetingCounters(meeting.MeetingId);
        }
    }
}
