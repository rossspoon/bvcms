using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using UtilityExtensions;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;

namespace CmsWeb.Models
{
    public partial class OnlineRegPersonModel
    {
        private Dictionary<string, string> _ExtraQuestion = new Dictionary<string, string>();
        public Dictionary<string, string> ExtraQuestion
        {
            get { return _ExtraQuestion; }
            set { _ExtraQuestion = value; }
        }
        public string ExtraQuestionValue(string s)
        {
            if (ExtraQuestion.ContainsKey(s))
                return ExtraQuestion[s];
            return null;
        }
        private Dictionary<string, bool?> _YesNoQuestion = new Dictionary<string, bool?>();
        public Dictionary<string, bool?> YesNoQuestion
        {
            get { return _YesNoQuestion; }
            set { _YesNoQuestion = value; }
        }
        public bool YesNoChecked(string key, bool value)
        {
            if (YesNoQuestion != null && YesNoQuestion.ContainsKey(key))
                return YesNoQuestion[key] == value;
            return false;
        }

        [OptionalField]
        private string[] _Checkbox;
        public string[] Checkbox
        {
            get { return _Checkbox; }
            set { _Checkbox = value; }
        }

        public bool CheckboxChecked(string key)
        {
            if (Checkbox == null)
                return false;
            return Checkbox.Contains(key);
        }
        public List<SelectListItem> ShirtSizes()
        {
            return ShirtSizes(org);
        }
        public class YesNoQuestionItem
        {
            public string name { get; set; }
            public string desc { get; set; }
            public int n { get; set; }
        }
        public IEnumerable<YesNoQuestionItem> YesNoQuestions()
        {
            var i = 0;
            var q = from s in (org.YesNoQuestions ?? string.Empty).Split(',')
                    let a = s.Split('=')
                    where s.HasValue()
                    select new YesNoQuestionItem { name = a[0].Trim(), desc = a[1], n = i++ };
            return q;
        }
        public IEnumerable<YesNoQuestionItem> Checkboxes()
        {
            var i = 0;
            var q = from s in (org.Checkboxes ?? string.Empty).Split(',')
                    let a = s.Split('=')
                    where s.HasValue()
                    select new YesNoQuestionItem { name = a[0].Trim(), desc = a[1], n = i++ };
            return q;
        }
        public class ExtraQuestionItem
        {
            public string question { get; set; }
            public int n { get; set; }
        }
        public IEnumerable<ExtraQuestionItem> ExtraQuestions()
        {
            var i = 0;
            var q = from s in (org.ExtraQuestions ?? string.Empty).Split(',')
                    where s.HasValue()
                    select new ExtraQuestionItem { question = s, n = i++ };
            return q;
        }
        public IEnumerable<SelectListItem> Options()
        {
            var q = from s in (org.AskOptions ?? string.Empty).Split(',')
                    let a = s.Split('=')
                    where s.HasValue()
                    let amt = a.Length > 1 ? " ({0:C})".Fmt(decimal.Parse(a[1])) : ""
                    select new SelectListItem { Text = a[0].Trim() + amt, Value = a[0].Trim() };
            var list = q.ToList();
            list.Insert(0, new SelectListItem { Text = "(not specified)", Value = "0" });
            return list;
        }
        public IEnumerable<SelectListItem> ExtraOptions()
        {
            var q = from s in (org.ExtraOptions ?? string.Empty).Split(',')
                    where s.HasValue()
                    let a = s.Split('=')
                    where a.Length > 1
                    select new SelectListItem { Text = a[1].Trim(), Value = a[0].Trim() };
            var list = q.ToList();
            list.Insert(0, new SelectListItem { Text = "(not specified)", Value = "00" });
            return list;
        }
        public class MenuItemType
        {
            public int n { get; set; }
            public string desc { get; set; }
            public string sg { get; set; }
            public decimal amt { get; set; }
        }
        [NonSerialized]
        private List<MenuItemType> menuitems;
        public List<MenuItemType> MenuItems()
        {
            if (menuitems == null)
            {
                menuitems = new List<MenuItemType>();
                if (org.MenuItems.HasValue())
                {
                    var i = 0;
                    var re = new Regex(@"(?<desc>.*?)(?:\[(?<sg>.*?)\])?=(?<amt>[\d.]+),?");
                    var m = re.Match(org.MenuItems);
                    while (m.Success)
                    {
                        var mi = new MenuItemType();
                        mi.n = i++;
                        mi.desc = m.Groups["desc"].Value;
                        mi.sg = m.Groups["sg"].Value;
                        if (!mi.sg.HasValue())
                            mi.sg = mi.desc;
                        mi.amt = decimal.Parse(m.Groups["amt"].Value);
                        menuitems.Add(mi);
                        m = m.NextMatch();
                    }
                }
            }
            return menuitems;
        }
        public class MenuItemChosen
        {
            public string sg { get; set; }
            public string desc { get; set; }
            public int n { get; set; }
            public int number { get; set; }
            public decimal amt { get; set; }
        }
        public IEnumerable<MenuItemChosen> MenuItemsChosen()
        {
            int nn = 0;
            var items = MenuItems();
            var q = from i in MenuItem
                    join m in items on i.Key equals m.sg
                    where i.Value.HasValue
                    select new MenuItemChosen { sg = m.sg, n = nn++, number = i.Value ?? 0, desc = m.desc, amt = m.amt };
            return q;
        }
        public IEnumerable<SelectListItem> GradeOptions()
        {
            var q = from s in (org.GradeOptions ?? string.Empty).Split(',')
                    where s.HasValue()
                    let a = s.Split('=')
                    where a.Length > 1
                    select new SelectListItem { Text = a[1].Trim(), Value = a[0].ToInt().ToString() };
            var list = q.ToList();
            list.Insert(0, new SelectListItem { Text = "(not specified)", Value = "00" });
            return list;
        }
        public class AgeGroupItem
        {
            public int StartAge { get; set; }
            public int EndAge { get; set; }
            public string Name { get; set; }
        }
        public IEnumerable<AgeGroupItem> AgeGroups()
        {
            var q = from o in (org.AgeGroups ?? string.Empty).Split(',')
                    where o.HasValue()
                    let b = o.Split('=')
                    let a = b[0].Split('-')
                    select new AgeGroupItem
                    {
                        StartAge = a[0].ToInt(),
                        EndAge = a[1].ToInt(),
                        Name = b[1]
                    };
            return q;
        }
        public static List<SelectListItem> ShirtSizes(CmsData.Organization org)
        {
            const string sizes = "YT-S=Youth: Small (6-8),YT-M=Youth: Medium (10-12),YT-L=Youth: Large (14-16),AD-S=Adult: Small,AD-M=Adult: Medium,AD-L=Adult: Large,AD-XL=Adult: X-Large,AD-XXL=Adult: XX-Large,AD-XXXL=Adult: XXX-Large";
            var shirtsizes = Util.PickFirst(org.ShirtSizes, sizes);
            var q = from ss in shirtsizes.Split(',')
                    let a = ss.Split('=')
                    select new SelectListItem
                    {
                        Value = a[0],
                        Text = a[1]
                    };
            var list = q.ToList();
            list.Insert(0, new SelectListItem { Value = "0", Text = "(not specified)" });
            if (org != null && org.AllowLastYearShirt == true)
                list.Add(new SelectListItem { Value = "lastyear", Text = "Use shirt from last year" });
            return list;
        }
        public void FillPriorInfo()
        {
#if DEBUG
            shirtsize = "YT-L";
            request = "tommy";
            emcontact = "test";
            emphone = "test";
            docphone = "test";
            doctor = "test";
            insurance = "test";
            policy = "test";
            mname = "";
            fname = "test t";
            tylenol = true;
            advil = true;
            robitussin = false;
            maalox = false;
            paydeposit = true;
#endif
            if (!IsNew)
            {
                var rr = DbUtil.Db.RecRegs.SingleOrDefault(r => r.PeopleId == PeopleId);
                if (rr != null)
                {
                    if (org.AskRequest == true)
                    {
                        var om = GetOrgMember();
                        if (om != null)
                            request = om.Request;
                    }
                    if (org.AskShirtSize == true)
                        shirtsize = rr.ShirtSize;
                    if (org.AskEmContact == true)
                    {
                        emcontact = rr.Emcontact;
                        emphone = rr.Emphone;
                    }
                    if (org.AskInsurance == true)
                    {
                        insurance = rr.Insurance;
                        policy = rr.Policy;
                    }
                    if (org.AskDoctor == true)
                    {
                        docphone = rr.Docphone;
                        doctor = rr.Doctor;
                    }
                    if (org.AskParents == true)
                    {
                        mname = rr.Mname;
                        fname = rr.Fname;
                    }
                    if (org.AskAllergies == true)
                        medical = rr.MedicalDescription;
                    if (org.AskCoaching == true)
                        coaching = rr.Coaching;
                    if (org.AskChurch == true)
                    {
                        otherchurch = rr.ActiveInAnotherChurch ?? false;
                        memberus = rr.Member ?? false;
                    }
                    if (org.AskTylenolEtc == true)
                    {
                        tylenol = rr.Tylenol;
                        advil = rr.Advil;
                        robitussin = rr.Robitussin;
                        maalox = rr.Maalox;
                    }
                }
            }
        }
        public bool NeedsCopyFromPrevious()
        {
            if (org != null)
                return (org.AskEmContact == true
                    || org.AskInsurance == true
                    || org.AskDoctor == true
                    || org.AskParents == true);
            return false;
        }
    }
}