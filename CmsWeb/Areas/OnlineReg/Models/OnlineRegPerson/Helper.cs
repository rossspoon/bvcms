using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using CmsData;
using CmsData.Registration;
using CmsWeb.Code;
using UtilityExtensions;
using System.Web.Mvc;
using System.Text;
using System.Net.Mail;
using CmsData.Codes;

namespace CmsWeb.Models
{
    public partial class OnlineRegPersonModel
    {
        private Dictionary<int, Settings> _settings;
        public Dictionary<int, Settings> settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = HttpContext.Current.Items["RegSettings"] as Dictionary<int, Settings>;
                    if (_settings == null)
                        Parent.ParseSettings();
                    _settings = HttpContext.Current.Items["RegSettings"] as Dictionary<int, Settings>;
                }
                return _settings;
            }
        }


        private Settings _setting;
        public Settings setting
        {
            get
            {
                if (_setting == null)
                {
                    if (org == null)
                        throw new Exception("no valid org");
                    if (settings == null)
                        throw new Exception("settings are null");
                    if (!settings.ContainsKey(org.OrganizationId))
                        settings[org.OrganizationId] = new Settings(org.RegSetting, DbUtil.Db, org.OrganizationId);
                    _setting = settings[org.OrganizationId];
                    AfterSettingConstructor();
                }
                return _setting;
            }
        }
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
            get { return gender == 1 ? "Male" : gender == 2 ? "Female" : "not specified"; }
        }
        public string marrieddisplay
        {
            get { return married == 10 ? "Single" : married == 20 ? "Married" : "not specified"; }
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
            return q.Any();
        }
        public bool UserSelectsOrganization()
        {
            return masterorgid.HasValue && masterorg.RegistrationTypeId == RegistrationTypeCode.UserSelectsOrganization2;
        }
        public bool ComputesOrganizationByAge()
        {
            return masterorgid.HasValue && masterorg.RegistrationTypeId == RegistrationTypeCode.ComputeOrganizationByAge2;
        }
        public bool ManageSubscriptions()
        {
            return masterorgid.HasValue && masterorg.RegistrationTypeId == RegistrationTypeCode.ManageSubscriptions2;
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
        private CmsData.Organization _org;
        public CmsData.Organization org
        {
            get
            {
                if (_org == null && orgid.HasValue)
                    if (orgid == Util.CreateAccountCode)
                        _org = OnlineRegModel.CreateAccountOrg();
                    else
                        _org = DbUtil.Db.LoadOrganizationById(orgid.Value);
                if (_org == null && classid.HasValue)
                    _org = DbUtil.Db.LoadOrganizationById(classid.Value);
                if (_org == null && (divid.HasValue || masterorgid.HasValue) && (Found == true || IsValidForNew))
                    if (ComputesOrganizationByAge())
                    {
                        _org = GetAppropriateOrg();
                    }
                if (_org != null)
                    orgid = _org.OrganizationId;
                return _org;
            }
        }
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
        private Organization GetAppropriateOrg()
        {
            List<Organization> list = null;
            if (!masterorgid.HasValue)
                return null;
            var cklist = masterorg.OrgPickList.Split(',').Select(oo => oo.ToInt()).ToList();
            var q = from o in DbUtil.Db.Organizations
                    where cklist.Contains(o.OrganizationId)
                    where gender == null || o.GenderId == gender || (o.GenderId ?? 0) == 0
                    select o;
            list = q.ToList();
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
            if (IsNew && IsValidForNew)
                return true;
            return false;
        }
        public bool AnyOtherInfo()
        {
            if (org != null)
                if (org.RegistrationTypeId == RegistrationTypeCode.CreateAccount)
                    return false;
                else if (org.RegistrationTypeId == RegistrationTypeCode.ChooseVolunteerTimes)
                    return false;
            return settings.Values.Any(s => s.AskItems.Any() || s.Deposit > 0);
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
                        string.Compare(ma.Address, person.EmailAddress, StringComparison.OrdinalIgnoreCase) == 0)
                    return;
                if (person.EmailAddress2.HasValue() &&
                        String.Compare(ma.Address, person.EmailAddress2, StringComparison.OrdinalIgnoreCase) == 0)
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
                            string.Compare(ma.Address, fm.EmailAddress, StringComparison.OrdinalIgnoreCase) == 0)
                        return;
                    if (fm.EmailAddress2.HasValue() &&
                            string.Compare(ma.Address, fm.EmailAddress2, StringComparison.OrdinalIgnoreCase) == 0)
                        return;
                }
            }
            if (!phone.HasValue())
                phone = DbUtil.Db.Setting("ChurchPhone", "(sorry, no phone # in settings)");

            /* so now we have a different email address than the one on record
             * we need to notify them */
            if (person.EmailAddress.HasValue() || person.EmailAddress2.HasValue())
            {
                var msg = DbUtil.Db.ContentHtml("DiffEmailMessage", Resource1.DiffEmailMessage);
                msg = msg.Replace("{name}", person.Name, ignoreCase: true);
                msg = msg.Replace("{first}", person.PreferredName, ignoreCase: true);
                msg = msg.Replace("{org}", orgname, ignoreCase: true);
                msg = msg.Replace("{phone}", phone, ignoreCase: true);
                var subj = "{0}, different email address than one on record".Fmt(orgname);
                DbUtil.Db.Email(fromemail, person, Util.ToMailAddressList(regemail), subj, msg, false);
            }
            else
            {
                var msg = DbUtil.Db.ContentHtml("NoEmailMessage", Resource1.NoEmailMessage);
                msg = msg.Replace("{name}", person.Name);
                msg = msg.Replace("{first}", person.PreferredName, ignoreCase: true);
                msg = msg.Replace("{org}", orgname, ignoreCase: true);
                msg = msg.Replace("{phone}", phone.FmtFone(), ignoreCase: true);
                var subj = "{0}, no email on your record".Fmt(orgname);
                DbUtil.Db.Email(fromemail, person, Util.ToMailAddressList(regemail), subj, msg, false);
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
            var cv = new CodeValueModel();
            return CodeValueModel.ConvertToSelect(cv.GetStateListUnknown(), "Code");
        }
        public IEnumerable<SelectListItem> Countries()
        {
            var list = CodeValueModel.ConvertToSelect(CodeValueModel.GetCountryList(), null);
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
        private PythonEvents _pythonEvents;
        public PythonEvents PythonEvents
        {
            get { return _pythonEvents ?? (_pythonEvents = HttpContext.Current.Items["PythonEvents"] as PythonEvents); }
        }
    }
}