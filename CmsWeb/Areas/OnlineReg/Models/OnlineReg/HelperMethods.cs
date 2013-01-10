using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Xml.Serialization;
using CmsData;
using System.Web.Mvc;
using System.Text;
using System.Configuration;
using CmsData.Registration;
using UtilityExtensions;
using System.Data.Linq.SqlClient;
using CMSPresenter;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Collections;
using System.Runtime.Serialization;
using CmsData.Codes;

namespace CmsWeb.Models
{
    public partial class OnlineRegModel
    {
        public static Organization CreateAccountOrg()
        {
            var settings = HttpContext.Current.Items["RegSettings"] as Dictionary<int, Settings>;
            if (settings == null)
            {
                settings = new Dictionary<int, Settings>();
                HttpContext.Current.Items.Add("RegSettings", settings);
            }
            var o = new Organization { OrganizationId = Util.CreateAccountCode, OrganizationName = "My Data" };
            o.RegistrationTypeId = RegistrationTypeCode.CreateAccount;
            if (!settings.ContainsKey(Util.CreateAccountCode))
                settings.Add(Util.CreateAccountCode, ParseSetting("AllowOnlyOne: true", Util.CreateAccountCode));
            return o;
        }
        private Dictionary<int, Settings> _settings;
        public Dictionary<int, Settings> settings
        {
            get
            {
                if (_settings == null)
                    _settings = HttpContext.Current.Items["RegSettings"] as Dictionary<int, Settings>;
                return _settings;
            }
        }
        public bool DisplayLogin()
        {
            return (List.Count == 0 && !UserPeopleId.HasValue && nologin == false);
        }
        public string LoginName
        {
            get
            {
                if (user != null)
                    return user.Name;
                return "anonymous";
            }
        }
        public string MeetingTime
        {
            get { return meeting().MeetingDate.ToString2("f"); }
        }
        public OnlineRegPersonModel last
        {
            get
            {
                if (list.Count > 0)
                    return list[list.Count - 1];
                return null;
            }
        }
        public string qtesting
        {
            get { return testing == true ? "?testing=true" : ""; }
        }
        public bool IsCreateAccount()
        {
            if (org != null)
                return org.RegistrationTypeId == RegistrationTypeCode.CreateAccount;
            return false;
        }
        public bool IsEnded()
        {
            if (div != null && UserSelectsOrganization())
                return UserSelectClasses(div.Id).Count() == 0;
            else if (org != null)
                return org.ClassFilled == true;
            return false;
        }
        public string Filled()
        {
            var msg = "";
            if (div != null && UserSelectsOrganization())
                msg = UserSelectClasses(div.Id).Count() == 0 ? "all registration options are full" : "";
            else if (org != null)
            {
                msg = ((org.ClassFilled ?? false) || (org.Limit > 0 && org.Limit <= org.MemberCount)) ? "registration is full" : "";
                if (msg.HasValue())
                {
                    org.ClassFilled = true;
                    DbUtil.Db.SubmitChanges();
                }
            }
            return msg;
        }
        public bool NotAvailable()
        {
        	var dt = DateTime.Now;
        	var dt1 = DateTime.Parse("1/1/1900");
        	var dt2 = DateTime.Parse("1/1/2200");
			if (divid != null)
				return DbUtil.Db.Organizations.Any(o =>
					   o.DivOrgs.Any(di => di.DivId == divid) &&
					   (o.RegistrationClosed == true
						|| dt < (o.RegStart ?? dt1)
						|| dt > (o.RegEnd ?? dt2)
					   ));
			if (masterorgid.HasValue)
				return masterorg.RegistrationClosed == true
				       || masterorg.OrganizationStatusId == OrgStatusCode.Inactive
				       || dt < (masterorg.RegStart ?? dt1)
				       || dt > (masterorg.RegEnd ?? dt2);
        	return org.RegistrationClosed == true
        	       || org.OrganizationStatusId == OrgStatusCode.Inactive
        	       || dt < (org.RegStart ?? dt1)
        	       || dt > (org.RegEnd ?? dt2);
        }
        public IEnumerable<Organization> GetOrgsInDiv()
        {
            return from o in DbUtil.Db.Organizations
                   where o.DivOrgs.Any(di => di.DivId == divid)
                   select o;
        }
        public bool UserSelectsOrganization()
        {
            if (masterorgid.HasValue && masterorg.RegistrationTypeId == RegistrationTypeCode.UserSelectsOrganization2)
                return true;
            if (divid == null)
                return false;
            var q = from o in GetOrgsInDiv()
                    where o.RegistrationTypeId == RegistrationTypeCode.UserSelectsOrganization
                    select o;
            return q.Count() > 0;
        }
        public bool OnlyOneAllowed()
        {
            if (org != null)
            {
                var setting = settings[org.OrganizationId];
                return org.RegistrationTypeId == RegistrationTypeCode.ChooseVolunteerTimes
                    || org.RegistrationTypeId == RegistrationTypeCode.CreateAccount
                    || setting.AllowOnlyOne || setting.AskVisible("AskTickets")
                    || setting.GiveOrgMembAccess;
            }
            if (settings != null)
            {
                var q = from o in settings.Values
                        where o.AllowOnlyOne || o.AskVisible("AskTickets")
                        select o;
                return q.Any();
            }
            return false;
        }
        public bool ChoosingSlots()
        {
            if (org != null)
                return org.RegistrationTypeId == RegistrationTypeCode.ChooseVolunteerTimes;
            return false;
        }
        public bool ManagingSubscriptions()
        {
            if (masterorgid.HasValue && masterorg.RegistrationTypeId == RegistrationTypeCode.ManageSubscriptions2)
                return true;

            if (org != null)
                return org.RegistrationTypeId == RegistrationTypeCode.ManageSubscriptions;

            var q = from o in GetOrgsInDiv()
                    where o.RegistrationTypeId == RegistrationTypeCode.ManageSubscriptions
                    select o;
            return q.Count() > 0;
        }
        public bool OnlinePledge()
        {
            if (org != null)
                return org.RegistrationTypeId == RegistrationTypeCode.OnlinePledge;
            return false;
        }
        public bool ManageGiving()
        {
            if (org != null)
                return org.RegistrationTypeId == RegistrationTypeCode.ManageGiving;
            return false;
        }
        public bool OnlineGiving()
        {
            if (org != null)
                return org.RegistrationTypeId == RegistrationTypeCode.OnlineGiving;
            return false;
        }
        public bool AskDonation()
        {
            if (org != null)
                return settings[org.OrganizationId].AskDonation;
            if (settings == null)
                return false;
            return settings.Values.Any(o => o.AskDonation);
        }
        public string DonationLabel()
        {
            if (org != null)
                return settings[org.OrganizationId].DonationLabel;
            return settings.Values.First(o => o.AskDonation).DonationLabel;
        }
        public string Header
        {
            get
            {
                if (masterorgid.HasValue)
                    return masterorg.OrganizationName;
                if (div != null)
                    return div.Name;
                else if (settings != null && org != null && settings[org.OrganizationId] != null)
                    return Util.PickFirst(settings[org.OrganizationId].Title, org.OrganizationName);
                else if (org != null)
                    return org.OrganizationName;
                return "no organization";
            }
        }
        public string Instructions
        {
            get
            {
                if (masterorg != null)
                {
                    var setting1 = new Settings();
                    if(settings.ContainsKey(masterorg.OrganizationId))
                        setting1 = settings[masterorg.OrganizationId];
                    var setting2 = setting1;
                    if (last != null && last.org != null && settings.ContainsKey(last.org.OrganizationId))
                        setting1 = settings[last.org.OrganizationId];
                    return @"
<div class=""instructions login"">{0}</div>
<div class=""instructions select"">{1}</div>
<div class=""instructions find"">{2}</div>
<div class=""instructions options"">{3}</div>
<div class=""instructions submit"">{4}</div>
<div class=""instructions sorry"">{5}</div>
"                   .Fmt(Util.PickFirst(setting1.InstructionLogin, setting2.InstructionLogin),
                         Util.PickFirst(setting1.InstructionSelect, setting2.InstructionSelect),
                         Util.PickFirst(setting1.InstructionFind, setting2.InstructionFind),
                         Util.PickFirst(setting1.InstructionOptions, setting2.InstructionOptions),
                         Util.PickFirst(setting1.InstructionSubmit, setting2.InstructionSubmit),
                         Util.PickFirst(setting1.InstructionSorry, setting2.InstructionSorry)
                         );
                }
                if (org != null)
                {
                    var setting = new Settings();
                    if (settings.ContainsKey(org.OrganizationId))
                        setting = settings[org.OrganizationId];
                    if (setting.InstructionAll != null)
                        if (setting.InstructionAll.ToString().HasValue())
                            return setting.InstructionAll.ToString();
                    var v = "{0}{1}{2}{3}{4}{5}".Fmt(
                                            setting.InstructionLogin,
                                            setting.InstructionSelect,
                                            setting.InstructionFind,
                                            setting.InstructionOptions,
                                            setting.InstructionSubmit,
                                            setting.InstructionSorry);
                    string ins = null;
                    if (v.HasValue())
                        ins = @"<div class=""instructions login"">{0}</div>
<div class=""instructions select"">{1}</div>
<div class=""instructions find"">{2}</div>
<div class=""instructions options"">{3}</div>
<div class=""instructions submit"">{4}</div>
<div class=""instructions sorry"">{5}</div>".Fmt(
                                            setting.InstructionLogin,
                                            setting.InstructionSelect,
                                            setting.InstructionFind,
                                            setting.InstructionOptions,
                                            setting.InstructionSubmit,
                                            setting.InstructionSorry
                                            );
					if (ins.Contains("{ev:", ignoreCase: true))
						ins = DoReplaceForExtraValueCode(ins, last.person);
                    return Util.PickFirst(ins, div != null ? div.Instructions : "") + "\n";
                }
                if (div != null)
                    return div.Instructions;
                return "";
            }
        }
		public static string DoReplaceForExtraValueCode(string text, Person p)
		{
			const string RE = @"{ev:(?<name>.+?)}";
			
			var re = new Regex(RE, RegexOptions.Singleline | RegexOptions.Multiline);
			var match = re.Match(text);
			while (match.Success)
			{
				var tag = match.Value;
				var name = match.Groups["name"].Value;

				if (p == null)
					text = text.Replace(tag, "");
				else
					text = text.Replace(tag, p.GetExtra(name));
				match = match.NextMatch();
			}
			return text;
		}

        public string Terms
        {
            get
            {
                if (masterorgid.HasValue)
                    return Util.PickFirst("{0}".Fmt(settings[masterorgid.Value].Terms), "");
                if (org != null)
                    return Util.PickFirst("{0}".Fmt(settings[org.OrganizationId].Terms),
                        div != null ? div.Terms : "");
                if (div != null)
                    return div.Terms;
                return "";
            }
        }
        public OnlineRegPersonModel LoadExistingPerson(int id, int index)
        {
            var person = DbUtil.Db.LoadPersonById(id);
            var p = new OnlineRegPersonModel
            {
                index = index,
                dob = person.DOB,
                email = person.EmailAddress.HasValue() ? person.EmailAddress : user.EmailAddress,
                first = person.PreferredName,
                last = person.LastName,
                PeopleId = id,
                phone = Util.PickFirst(person.CellPhone, person.HomePhone),
                orgid = orgid,
                masterorgid = masterorgid,
                divid = divid,
                classid = classid,
                IsFamily = true,
                LoggedIn = true,
                Found = true,
                IsValidForExisting = true,
            };
            return p;
        }
//		public void SavePaymentInfo()
//		{
//            try
//            {
//                var gateway = OnlineRegModel.GetTransactionGateway();
//                if (gateway == "authorizenet")
//                {
//                    var au = new AuthorizeNet(DbUtil.Db, m.testing);
//                    au.AddUpdateCustomerProfile(m.pid,
//                        m.SemiEvery,
//                        m.Day1,
//                        m.Day2,
//                        m.EveryN,
//                        m.Period,
//                        m.StartWhen,
//                        m.StopWhen,
//                        m.Type,
//                        m.Cardnumber,
//                        m.Expires,
//                        m.Cardcode,
//                        m.Routing,
//                        m.Account,
//                        m.testing);
//                }
//                else if (gateway == "sage")
//                {
//                    var sg = new CmsData.SagePayments(DbUtil.Db, m.testing);
//                    sg.storeVault(m.pid, 
//                        m.SemiEvery,
//                        m.Day1,
//                        m.Day2,
//                        m.EveryN,
//                        m.Period,
//                        m.StartWhen,
//                        m.StopWhen,
//                        m.Type,
//                        m.Cardnumber,
//                        m.Expires,
//                        m.Cardcode,
//                        m.Routing,
//                        m.Account,
//                        m.testing);
//                }
//                else
//                    throw new Exception("ServiceU not supported");
//		}
    }
}
