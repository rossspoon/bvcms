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
using System.Text.RegularExpressions;

namespace CMSWeb.Models
{
    public class VolunteerModel
    {
        private string _view;
        public string View
        {
            get
            {
                return _view;
            }
            set
            {
                _view = value;
                var c = DbUtil.Content("Volunteer-" + View + ".view");
                if (c != null)
                {
                    formcontent = c.Body;
                    formtitle = c.Title;
                }
            }
        }
        public string first { get; set; }
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
        public string formcontent { get; set; }
        public string formtitle { get; set; }
        public string FormInitialize()
        {
            var q = from vi in person.FetchVolInterestInterestCodes(View)
                    select vi.VolInterestCode;
            var sb = new StringBuilder();
            sb.AppendLine("<script type='text/javascript'>");
            sb.AppendLine("$(document).ready(function() {");
            foreach (var vi in q)
                sb.AppendFormat("$(\"input[name={0}{1}]\").attr('checked', true);\n", vi.Org, vi.Code);
            sb.AppendLine("});");
            sb.AppendLine("</script>");
            return sb.ToString();
        }

        public string zip { get; set; }
        public string phone { get; set; }
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
            person = CMSWeb.Models.SearchPeopleModel
                .FindPerson(phone, first, last, birthday, out count);
            return count;
        }

        public void ValidateModel(ModelStateDictionary modelState)
        {
            CMSWeb.Models.SearchPeopleModel
                .ValidateFindPerson(modelState, first, last, birthday, phone);
            if (!phone.HasValue())
                modelState.AddModelError("phone", "phone required");
            if (!email.HasValue() || !Util.ValidEmail(email))
                modelState.AddModelError("email", "valid email required");
        }
        public void ValidateModel2(ModelStateDictionary ModelState)
        {
            if (interests == null)
                ModelState.AddModelError("interests", "Must check at least one interest");
        }
        public string PrepareSummaryText()
        {
            var q = from vi in person.VolList
                    orderby vi.Value.sortdesc
                    select vi.Key;
            var sb = new StringBuilder();
            foreach (var i in q)
                sb.AppendFormat("{0}<br/>\n", i);
            return sb.ToString();
        }
        public string PrepareSummaryText2()
        {
            var q = from vi in person.VolList
                    where vi.Value.oi.nodrop
                    orderby vi.Value.sortdesc
                    select vi.Key;
            var sb = new StringBuilder();
            foreach (var i in q)
                sb.AppendFormat("{0}<br/>\n", i);
            return sb.ToString();
        }
    }
}
