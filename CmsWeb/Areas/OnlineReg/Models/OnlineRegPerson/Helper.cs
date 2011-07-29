using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using UtilityExtensions;
using CMSPresenter;
using System.Web.Mvc;
using System.Text;
using System.Net.Mail;
using CmsData.Codes;

namespace CmsWeb.Models
{
    public partial class OnlineRegPersonModel
    {
        public int age
        {
            get
            {
                if (_Person != null && _Person.BirthDate.HasValue)
                    return person.BirthDate.Value.AgeAsOf(Util.Now);
                if (birthday.HasValue)
                    return birthday.Value.AgeAsOf(Util.Now);
                return 0;
            }
        }
        public string genderdisplay
        {
            get { return gender == 1 ? "Male" : "Female"; }
        }
        public string marrieddisplay
        {
            get { return married == 10 ? "Single" : "Married"; }
        }
        public bool UserSelectsOrganization()
        {
            return divid != null && DbUtil.Db.Organizations.Any(o => o.DivOrgs.Any(di => di.DivId == divid) &&
                    o.RegistrationTypeId == RegistrationEnum.UserSelectsOrganization);
        }
        public bool ComputesOrganizationByAge()
        {
            return divid != null && DbUtil.Db.Organizations.Any(o => o.DivOrgs.Any(di => di.DivId == divid) &&
                    o.RegistrationTypeId == RegistrationEnum.ComputeOrganizationByAge);
        }
        public bool ManageSubscriptions()
        {
            return divid != null && DbUtil.Db.Organizations.Any(o => o.DivOrgs.Any(di => di.DivId == divid) &&
                    o.RegistrationTypeId == RegistrationEnum.ManageSubscriptions);
        }
        public bool MemberOnly()
        {
            if (org != null)
                return org.MemberOnly == true;
            return divid != null && DbUtil.Db.Organizations.Any(o => o.DivOrgs.Any(di => di.DivId == divid) &&
                    o.MemberOnly == true);
        }
        [NonSerialized]
        private CmsData.Organization _org;
        public CmsData.Organization org
        {
            get
            {
                if (_org == null && orgid.HasValue)
                    if (orgid == Util.CreateAccountCode)
                        _org = OnlineRegModel.CreateAccountOrg;
                    else
                        _org = DbUtil.Db.LoadOrganizationById(orgid.Value);
                if (_org == null && classid.HasValue)
                    _org = DbUtil.Db.LoadOrganizationById(classid.Value);
                if (_org == null && divid.HasValue && (Found == true || IsValidForNew))
                    if (ComputesOrganizationByAge())
                        _org = GetAppropriateOrg();
                return _org;
            }
        }
        private CmsData.Organization GetAppropriateOrg()
        {
            var q = from o in DbUtil.Db.Organizations
                    where o.RegistrationTypeId == RegistrationEnum.ComputeOrganizationByAge
                    where o.DivOrgs.Any(di => di.DivId == divid)
                    where gender == null || o.GenderId == gender || o.GenderId == 0
                    select o;
            var list = q.ToList();
            var q2 = from o in list
                     where birthday >= o.BirthDayStart || o.BirthDayStart == null
                     where birthday <= o.BirthDayEnd || o.BirthDayEnd == null
                     select o;
            return q2.FirstOrDefault();
        }
        public bool Finished()
        {
            return ShowDisplay() && OtherOK;
        }
        public bool ShowDisplay()
        {
            if (Found == true && IsValidForExisting)
                return true;
            if (org == null || IsFilled)
                return false;
            if (IsFamily)
                return IsValidForExisting;
            else if (IsNew && IsValidForNew)
                return true;
            return false;
        }
        public bool AnyOtherInfo()
        {
            if (org != null)
                if (org.RegistrationTypeId == RegistrationEnum.CreateAccount)
                    return false;
                else if (org.RegistrationTypeId == RegistrationEnum.ChooseSlot)
                    return false;
                else return (org.AskShirtSize == true ||
                    org.AskRequest == true ||
                    org.AskGrade == true ||
                    org.AskEmContact == true ||
                    org.AskInsurance == true ||
                    org.AskDoctor == true ||
                    org.AskAllergies == true ||
                    org.AskTylenolEtc == true ||
                    org.AskParents == true ||
                    org.AskCoaching == true ||
                    org.AskChurch == true ||
                    org.AskTickets == true ||
                    org.ExtraQuestions.HasValue() ||
                    org.MenuItems.HasValue() ||
                    org.AskOptions.HasValue() ||
                    org.YesNoQuestions.HasValue() ||
                    org.Deposit > 0);
            var q = from o in DbUtil.Db.Organizations
                    where o.DivOrgs.Any(di => di.DivId == divid)
                    where o.AskShirtSize == true ||
                        o.AskRequest == true ||
                        o.AskGrade == true ||
                        o.AskEmContact == true ||
                        o.AskInsurance == true ||
                        o.AskDoctor == true ||
                        o.AskAllergies == true ||
                        o.AskTylenolEtc == true ||
                        o.AskParents == true ||
                        o.AskCoaching == true ||
                        o.AskChurch == true ||
                        o.AskTickets == true ||
                        o.AskOptions.Length > 0 ||
                        o.ExtraQuestions.Length > 0 ||
                        o.MenuItems.Length > 0 ||
                        o.YesNoQuestions.Length > 0 ||
                        o.Deposit > 0
                    select o;
            return q.Count() > 0;
        }
        public static void CheckNotifyDiffEmails(Person person, string fromemail, string regemail, string orgname, string phone)
        {
            MailAddress ma = null;
            try { ma = new MailAddress(regemail); } catch (Exception) { }
            if (ma != null)
            {
                /* If one of the email addresses we have on record
                 * is the same as the email address given in registration
                 * then no problem, (not different) */
                if (person.EmailAddress.HasValue() && 
                        string.Compare(ma.Address, person.EmailAddress, true) == 0)
                    return;
                if (person.EmailAddress2.HasValue() && 
                        string.Compare(ma.Address, person.EmailAddress2, true) == 0)
                    return;
                /* So now we check to see if anybody in the famiy
                 * has the email address used in registration
                 * if so then that is OK too. */
                var flist = from fm in person.Family.People
                            where fm.PositionInFamilyId == PositionInFamily.PrimaryAdult
                            select fm;
                foreach (var fm in flist)
                {
                    if (fm.EmailAddress.HasValue() &&
                            string.Compare(ma.Address, fm.EmailAddress, true) == 0)
                        return;
                    if (fm.EmailAddress2.HasValue() &&
                            string.Compare(ma.Address, fm.EmailAddress2, true) == 0)
                        return;
                }
            }
            /* so now we have a different email address than the one on record
             * we need to notify them */
            if (person.EmailAddress.HasValue() || person.EmailAddress2.HasValue())
            {
                var c = DbUtil.Content("DiffEmailMessage");
                if (c == null)
                {
                    c = new Content();
                    c.Body = @"Hi {name},
<p>You registered for {orgname} using a different email address than the one we have on record.
It is important that you call the church <strong>{phone}</strong> to update our records
so that you will receive future important notices regarding this registration.</p>";
                    c.Title = "{orgname}, different email address than one on record";
                }
                var msg = c.Body.Replace("{name}", person.Name);
                msg = msg.Replace("{orgname}", orgname);
                msg = msg.Replace("{phone}", phone.FmtFone());
                var subj = c.Title.Replace("{orgname}", orgname);
                DbUtil.Db.Email(fromemail, 
                    person, Util.ToMailAddressList(regemail), 
                    subj, msg, false);
            }
            else
            {
                var c = DbUtil.Content("NoEmailMessage");
                if (c == null)
                {
                    c = new Content();
                    c.Body = @"Hi {name},
<p>You registered for {orgname}, and we found your record, 
but there was no email address on your existing record in our database.
If you would like for us to update your record with this email address or another,
Please contact the church at <strong>{phone}</strong> to let us know.
It is important that we have your email address so that
you will receive future important notices regarding this registration.
But we won't add that to your record without your permission.

Thank you</p>";
                    c.Title = "{orgname}, no email on your record";
                }
                var msg = c.Body.Replace("{name}", person.Name);
                msg = msg.Replace("{orgname}", orgname);
                msg = msg.Replace("{phone}", phone.FmtFone());
                var subj = c.Title.Replace("{orgname}", orgname);
                DbUtil.Db.Email(fromemail, 
                    person, Util.ToMailAddressList(regemail), 
                    subj, msg, false);
            }
        }
        private static string trim(string s)
        {
            if (s != null)
                return s.Trim();
            return s;
        }
        public OrganizationMember GetOrgMember()
        {
            if (org != null)
                return DbUtil.Db.OrganizationMembers.SingleOrDefault(m2 =>
                    m2.PeopleId == PeopleId && m2.OrganizationId == org.OrganizationId);
            return null;
        }
        public IEnumerable<SelectListItem> StateCodes()
        {
            var cv = new CodeValueController();
            return QueryModel.ConvertToSelect(cv.GetStateListUnknown(), "Code");
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            if (org == null)
                return string.Empty;
            sb.AppendFormat("Org: {0}<br/>\n", org.OrganizationName);
            if (PeopleId.HasValue)
            {
                sb.AppendFormat("{0}({1},{2},{3}), Birthday: {4}({5}), Phone: {6}, {7}, {8}<br />\n".Fmt(
                    person.Name, person.PeopleId, person.Gender.Code, person.MaritalStatus.Code,
                    person.DOB, person.Age, phone.FmtFone(), person.EmailAddress, email));
                if (ShowAddress)
                    sb.AppendFormat("&nbsp;&nbsp;{0}; {1}<br />\n", person.PrimaryAddress, person.CityStateZip);
            }
            else
            {
                sb.AppendFormat("{0} {1}({2},{3}), Birthday: {4}({5}), Phone: {6}, {7}<br />\n".Fmt(
                    first, last, gender, married,
                    dob, age, phone.FmtFone(), email));
                if (ShowAddress)
                    sb.AppendFormat("&nbsp;&nbsp;{0}; {1}<br />\n", this.address, city);
            }
            return sb.ToString();
        }
    }
}