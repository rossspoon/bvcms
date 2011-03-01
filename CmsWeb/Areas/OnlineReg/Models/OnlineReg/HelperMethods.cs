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
using System.Data.Linq.SqlClient;
using CMSPresenter;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Collections;
using System.Runtime.Serialization;

namespace CmsWeb.Models
{
    public partial class OnlineRegModel
    {
        public static Organization CreateAccountOrg
        {
            get
            {
                return new Organization
                {
                    OrganizationName = "My Data",
                    RegistrationTypeId = (int)Organization.RegistrationEnum.CreateAccount,
                    AllowOnlyOne = true,
                };
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
            get { return meeting().MeetingDate.ToString2("ddd, MMM d h:mm tt"); }
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
        public bool? testing { get; set; }
        public string qtesting
        {
            get { return testing == true ? "?testing=true" : ""; }
        }
        public bool IsCreateAccount()
        {
            if (org != null)
                return org.RegistrationTypeId == (int)Organization.RegistrationEnum.CreateAccount;
            return false;
        }
        public bool IsEnded()
        {
            if (div != null && UserSelectsOrganization())
                return UserSelectClasses(div.Id).Count() == 0;
            else if(org != null)
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
            if (divid != null)
                return DbUtil.Db.Organizations.Any(o => o.DivOrgs.Any(di => di.DivId == divid) &&
                    o.RegistrationClosed == true);
            return org.RegistrationClosed == true;
        }
        public bool UserSelectsOrganization()
        {
            return divid != null && DbUtil.Db.Organizations.Any(o => o.DivOrgs.Any(di => di.DivId == divid) &&
                    o.RegistrationTypeId == (int)CmsData.Organization.RegistrationEnum.UserSelectsOrganization);
        }
        public bool OnlyOneAllowed()
        {
            if (org != null)
                return org.AllowOnlyOne == true
                    || org.AskTickets == true
                    || org.RegistrationTypeId == (int)Organization.RegistrationEnum.ChooseSlot
                    || org.RegistrationTypeId == (int)Organization.RegistrationEnum.CreateAccount
                    || org.GiveOrgMembAccess == true;

            var q = from o in DbUtil.Db.Organizations
                    where o.DivOrgs.Any(di => di.DivId == divid)
                    where o.AllowOnlyOne == true || o.AskTickets == true
                    select o;
            return q.Count() > 0;
        }
        public bool ChoosingSlots()
        {
            if (org != null)
                return org.RegistrationTypeId == (int)Organization.RegistrationEnum.ChooseSlot;
            return false;
        }
        public bool ManagingSubscriptions()
        {
            if (org != null)
                return org.RegistrationTypeId == (int)Organization.RegistrationEnum.ManageSubscriptions;
            return DbUtil.Db.Organizations.Any(o => o.DivOrgs.Any(di => di.DivId == divid) &&
                    o.RegistrationTypeId == (int)CmsData.Organization.RegistrationEnum.ManageSubscriptions);
        }
        public bool AskDonation()
        {
            if (org != null)
                return org.AskDonation ?? false;
            return DbUtil.Db.Organizations.Any(o => o.DivOrgs.Any(di => di.DivId == divid) &&
                    o.AskDonation == true);
        }
        public string Header
        {
            get
            {
                if (div != null)
                    return div.Name;
                else
                    return org != null ? org.OrganizationName : "no organization";
            }
        }
        public string Instructions
        {
            get
            {
                if (org != null)
                    return Util.PickFirst(org.Instructions, div != null ? div.Instructions : "");
                if (div != null)
                    return div.Instructions;
                return "";
            }
        }
        public string Terms
        {
            get
            {
                if (org != null)
                    return Util.PickFirst(org.Terms, div != null ? div.Terms : "");
                if (div != null)
                    return div.Terms;
                return "";
            }
        }
        public OnlineRegPersonModel LoadExistingPerson(int id)
        {
		    var person = DbUtil.Db.LoadPersonById(id);
		    var p = new OnlineRegPersonModel
            {
                dob = person.DOB,
                email = person.EmailAddress.HasValue() ? person.EmailAddress: user.EmailAddress,
                first = person.FirstName,
                last = person.LastName,
                PeopleId = id,
                phone = Util.PickFirst(person.CellPhone, person.HomePhone),
                orgid = orgid,
                divid = divid,
                classid = classid,
                IsFamily = true,
                LoggedIn = true,
            };
            return p;
        }
    }
}
