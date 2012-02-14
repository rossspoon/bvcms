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
        public IEnumerable<Organization> GetOrgsInDiv()
        {
            return from o in DbUtil.Db.Organizations
                   where o.DivOrgs.Any(di => di.DivId == divid)
                   select o;
        }
        private bool RegistrationType(int typ)
        {
            if (divid == null)
                return false;
            var q = from o in GetOrgsInDiv()
                    where o.RegistrationTypeId == typ
                    select o;
            return q.Count() > 0;
        }
        public bool UserSelectsOrganization()
        {
            if (masterorgid.HasValue && masterorg.RegistrationTypeId == RegistrationTypeCode.UserSelectsOrganization2)
                return true;
            return RegistrationType(RegistrationTypeCode.UserSelectsOrganization);
        }
        public bool ComputesOrganizationByAge()
        {
            if (masterorgid.HasValue && masterorg.RegistrationTypeId == RegistrationTypeCode.ComputeOrganizationByAge2)
                return true;
            return RegistrationType(RegistrationTypeCode.ComputeOrganizationByAge);
        }
        public bool ManageSubscriptions()
        {
            if (masterorgid.HasValue && masterorg.RegistrationTypeId == RegistrationTypeCode.ManageSubscriptions2)
                return true;
            return RegistrationType(RegistrationTypeCode.ManageSubscriptions);
        }
        public bool ManageGiving()
        {
            return RegistrationType(RegistrationTypeCode.ManageGiving);
        }
        public bool OnlineGiving()
        {
            if (org != null)
                return org.RegistrationTypeId == RegistrationTypeCode.OnlineGiving;
            return false;
        }
        public bool OnlinePledge()
        {
            if (org != null)
                return org.RegistrationTypeId == RegistrationTypeCode.OnlinePledge;
            return false;
        }
        public bool MemberOnly()
        {
            if (org != null)
                return setting.MemberOnly == true;
            if (divid == null)
                return false;
            return settings.Values.Any(o => o.MemberOnly);
        }
        [NonSerialized]
        private CmsData.Organization _org;
        public CmsData.Organization org
        {
            get
            {
                if (_org == null && orgid.HasValue)
                    if (orgid == Util.CreateAccountCode)
                        _org = OnlineRegModel.CreateAccountOrg();
                    //else if (orgid == Util.OnlineGivingCode)
                    //    _org = OnlineRegModel.CreateGivingOrg();
                    else
                        _org = DbUtil.Db.LoadOrganizationById(orgid.Value);
                if (_org == null && classid.HasValue)
                    _org = DbUtil.Db.LoadOrganizationById(classid.Value);
                if (_org == null && (divid.HasValue || masterorgid.HasValue) && (Found == true || IsValidForNew))
                    if (ComputesOrganizationByAge())
                        _org = GetAppropriateOrg();
                //if(_org != null && _settings == null)
                //    ParseSettings();
                return _org;
            }
        }
        [NonSerialized]
        private CmsData.Organization _masterorg;
        public CmsData.Organization masterorg
        {
            get
            {
                if (_masterorg != null)
                    return _masterorg;
                if (masterorgid.HasValue)
                    _masterorg = DbUtil.Db.LoadOrganizationById(masterorgid.Value);
                else
                {
                    if (org.RegistrationTypeId == RegistrationTypeCode.UserSelectsOrganization2
                        || org.RegistrationTypeId == RegistrationTypeCode.ComputeOrganizationByAge2
                        || org.RegistrationTypeId == RegistrationTypeCode.ManageSubscriptions2)
                    {
                        _masterorg = org;
                        masterorgid = orgid;
                        orgid = null;
                        _org = null;
                    }
                }
                return _masterorg;
            }
        }
        [NonSerialized]
        private RegSettings _setting;
        public RegSettings setting
        {
            get
            {
                if (_setting == null)
                {
                    if (org == null)
                        throw new Exception("no valid org");
                    if (settings == null)
                        throw new Exception("settings are null");
                    _setting = settings[org.OrganizationId];
                }
                return _setting;
            }
        }
        private CmsData.Organization GetAppropriateOrg()
        {
            List<Organization> list = null;
            if (masterorgid.HasValue)
            {
                var cklist = masterorg.OrgPickList.Split(',').Select(oo => oo.ToInt()).ToList();
                var q = from o in DbUtil.Db.Organizations
                        where cklist.Contains(o.OrganizationId)
                        where gender == null || o.GenderId == gender || o.GenderId == 0
                        select o;
                list = q.ToList();
            }
            else
            {
                var q = from o in DbUtil.Db.Organizations
                        where o.RegistrationTypeId == RegistrationTypeCode.ComputeOrganizationByAge
                        where o.DivOrgs.Any(di => di.DivId == divid)
                        where gender == null || o.GenderId == gender || o.GenderId == 0
                        select o;
                list = q.ToList();
            }
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
                if (org.RegistrationTypeId == RegistrationTypeCode.CreateAccount)
                    return false;
                else if (org.RegistrationTypeId == RegistrationTypeCode.ChooseSlot)
                    return false;
            return settings.Values.Any(setting =>
                setting.AskShirtSize == true ||
                setting.AskRequest == true ||
                //setting.AskGrade == true ||
                setting.AskEmContact == true ||
                setting.AskInsurance == true ||
                setting.AskDoctor == true ||
                setting.AskAllergies == true ||
                setting.AskTylenolEtc == true ||
                setting.AskParents == true ||
                setting.AskCoaching == true ||
                setting.AskChurch == true ||
                setting.AskTickets == true ||
                setting.ExtraQuestions != null ||
                setting.MenuItems != null ||
                setting.Dropdown1 != null ||
                setting.Dropdown2 != null ||
                setting.Dropdown3 != null ||
                setting.YesNoQuestions != null ||
                setting.Deposit > 0);
        }
        public static void CheckNotifyDiffEmails(Person person, string fromemail, string regemail, string orgname, string phone)
        {
            MailAddress ma = null;
            try { ma = new MailAddress(regemail); }
            catch (Exception) { }
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
<p>You registered for {org} using a different email address than the one we have on record.
It is important that you call the church <strong>{phone}</strong> to update our records
so that you will receive future important notices regarding this registration.</p>";
                    c.Title = "{org}, different email address than one on record";
                }
                var msg = c.Body.Replace("{name}", person.Name);
                msg = msg.Replace("{first}", person.PreferredName);
                msg = msg.Replace("{org}", orgname);
                msg = msg.Replace("{phone}", phone.FmtFone());
                var subj = c.Title.Replace("{org}", orgname);
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
<p>You registered for {org}, and we found your record, 
but there was no email address on your existing record in our database.
If you would like for us to update your record with this email address or another,
Please contact the church at <strong>{phone}</strong> to let us know.
It is important that we have your email address so that
you will receive future important notices regarding this registration.
But we won't add that to your record without your permission.

Thank you</p>";
                    c.Title = "{org}, no email on your record";
                }
                var msg = c.Body.Replace("{name}", person.Name);
                msg = msg.Replace("{first}", person.PreferredName);
                msg = msg.Replace("{org}", orgname);
                msg = msg.Replace("{phone}", phone.FmtFone());
                var subj = c.Title.Replace("{org}", orgname);
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
        public IEnumerable<SelectListItem> Countries()
        {
            var list = QueryModel.ConvertToSelect(CodeValueController.GetCountryList(), null);
            list.Insert(0, new SelectListItem { Text = "(not specified)", Value = "" });
            return list;
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
        public static SelectListItem[] Funds()
        {
            var q = from f in DbUtil.Db.ContributionFunds
                    where f.FundStatusId == 1
                    where f.OnlineSort > 0
                    orderby f.OnlineSort
                    select new SelectListItem
                    {
                        Text = "{0}".Fmt(f.FundName),
                        Value = f.FundId.ToString()
                    };
            return q.ToArray();
        }
    }
}