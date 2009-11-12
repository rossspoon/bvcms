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
    public class VolunteerModel
    {
        private int _OpportunityId;
        public int OpportunityId
        {
            get
            {
                return _OpportunityId;
            }
            set
            {
                _OpportunityId = value;
                Opportunity = DbUtil.Db.VolOpportunities.Single(o => o.Id == value);
            }
        }
        internal VolInterest VolInterest;
        public int VolInterestId
        {
            get
            {
                return VolInterest.Id;
            }
            set
            {
                VolInterest = DbUtil.Db.VolInterests.SingleOrDefault(vi => vi.Id == value);
                interests = VolInterest.VolInterestInterestCodes.Select(vi =>
                               vi.VolInterestCode.Id.ToString()).ToArray();
                OpportunityId = VolInterest.VolOpportunity.Id;
                person = VolInterest.Person;
                question = VolInterest.Question;
            }
        }
        public VolOpportunity Opportunity;
        public string first {get; set;}
        public string last {get; set;}
        public string dob {get; set;}
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

        public string zip { get; set; }
        public string phone {get; set;}
        public string email { get; set; }
        public Person person { get; set; }
        public string[] interests { get; set; }
        public string question { get; set; }

        public string Checked(int id)
        {
            if (interests != null && interests.Contains(id.ToString()))
                return "checked='checked'";
            return "";
        }


        public int FindMember()
        {
            int count;
            person = SearchPeopleModel.FindPerson(phone, first, last, birthday, out count);
            return count;
        }

        public void ValidateModel(ModelStateDictionary modelState)
        {
            SearchPeopleModel.ValidateFindPerson(modelState, first, last, birthday, phone);

            if (!phone.HasValue())
                modelState.AddModelError("phone", "phone required");
            if (!email.HasValue() || !Util.ValidEmail(email))
                modelState.AddModelError("email", "Please specify a valid email address.");
        }
        public void ValidateModel2(ModelStateDictionary ModelState)
        {
            if (interests == null)
                ModelState.AddModelError("interests", "Must check at least one interest");
            else if (Opportunity.MaxChecks.HasValue && interests.Length > Opportunity.MaxChecks)
                ModelState.AddModelError("interests", "Please check a maximum of {0} interests".Fmt(Opportunity.MaxChecks));
        }
        public string PrepareSummaryText()
        {
            var q = from vi in VolInterest.VolInterestInterestCodes
                    select vi.VolInterestCode.Description;
            var sb = new StringBuilder("<blockquote>\n");
            foreach (var i in q)
                sb.AppendFormat("{0}</br>\n", i);
            sb.Append("</blockquote>\n");
            return sb.ToString();

        }
    }
}
