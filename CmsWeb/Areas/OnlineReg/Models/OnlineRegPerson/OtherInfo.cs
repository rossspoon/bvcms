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
        [OptionalField]
        private string[] _Checkbox2;
        public string[] Checkbox2
        {
            get { return _Checkbox2; }
            set { _Checkbox2 = value; }
        }

        public bool CheckboxChecked(string key)
        {
            if (Checkbox == null)
                return false;
            return Checkbox.Contains(key);
        }
        public bool Checkbox2Checked(string key)
        {
            if (Checkbox2 == null)
                return false;
            return Checkbox2.Contains(key);
        }

        public IEnumerable<RegSettings.MenuItem> CheckboxItemsChosen()
        {
            if (Checkbox == null)
                return new List<RegSettings.MenuItem>();
            var items = setting.Checkboxes;
            var q = from i in Checkbox
                    join c in items on i equals c.SmallGroup
                    select c;
            return q;
        }
        public IEnumerable<RegSettings.MenuItem> Checkbox2ItemsChosen()
        {
            if (Checkbox2 == null)
                return new List<RegSettings.MenuItem>();
            var items = setting.Checkboxes2;
            var q = from i in Checkbox2
                    join c in items on i equals c.SmallGroup
                    select c;
            return q;
        }
        public RegSettings.MenuItem Dropdown1ItemChosen()
        {
            return setting.Dropdown1.SingleOrDefault(i => i.SmallGroup == option);
        }
        public RegSettings.MenuItem Dropdown2ItemChosen()
        {
            return setting.Dropdown2.SingleOrDefault(i => i.SmallGroup == option2);
        }
        public RegSettings.MenuItem Dropdown3ItemChosen()
        {
            return setting.Dropdown3.SingleOrDefault(i => i.SmallGroup == option3);
        }
        private List<string> _GroupTags;
        public List<string> GroupTags
        {
            get
            {
                if (_GroupTags == null)
                    _GroupTags = (from mt in DbUtil.Db.OrgMemMemTags
                                  where mt.OrgId == org.OrganizationId
                                  select mt.MemberTag.Name).ToList();
                return _GroupTags;
            }
        }
        
        public bool IsGroupFilled(RegSettings.MenuItem i)
        {
            if (i.Limit > 0)
            {
                var cnt = GroupTags.Count(mm => mm == i.SmallGroup);
                if (cnt >= i.Limit)
                    return true;
            }
            return false;
        }
        public class SelectListItemFilled : SelectListItem
        {
            public bool Filled { get; set; }
        }
        public IEnumerable<SelectListItemFilled> DropdownList1()
        {
            var q = from s in setting.Dropdown1
                    let amt = s.Fee.HasValue ? " ({0:C})".Fmt(s.Fee) : ""
                    select new SelectListItemFilled 
                    { 
                        Text = s.Description + amt, 
                        Value = s.SmallGroup,
                        Filled = IsGroupFilled(s)
                    };
            var list = q.ToList();
            list.Insert(0, new SelectListItemFilled { Text = "(please select)", Value = "00" });
            return list;
        }
        public IEnumerable<SelectListItemFilled> DropdownList2()
        {
            var q = from s in setting.Dropdown2
                    let amt = s.Fee.HasValue ? " ({0:C})".Fmt(s.Fee) : ""
                    select new SelectListItemFilled 
                    { 
                        Text = s.Description + amt, 
                        Value = s.SmallGroup,
                        Filled = IsGroupFilled(s)
                    };
            var list = q.ToList();
            list.Insert(0, new SelectListItemFilled { Text = "(please select)", Value = "00" });
            return list;
        }
        public IEnumerable<SelectListItemFilled> DropdownList3()
        {
            var q = from s in setting.Dropdown3
                    let amt = s.Fee.HasValue ? " ({0:C})".Fmt(s.Fee) : ""
                    select new SelectListItemFilled 
                    { 
                        Text = s.Description + amt, 
                        Value = s.SmallGroup,
                        Filled = IsGroupFilled(s)
                    };
            var list = q.ToList();
            list.Insert(0, new SelectListItemFilled { Text = "(please select)", Value = "00" });
            return list;
        }
        public class MenuItemChosen
        {
            public string sg { get; set; }
            public string desc { get; set; }
            public int number { get; set; }
            public decimal amt { get; set; }
        }
        public IEnumerable<MenuItemChosen> MenuItemsChosen()
        {
            if (MenuItem == null)
                return new List<MenuItemChosen>();
            var items = setting.MenuItems;
            var q = from i in MenuItem
                    join m in items on i.Key equals m.SmallGroup
                    where i.Value.HasValue
                    select new MenuItemChosen { sg = m.SmallGroup, number = i.Value ?? 0, desc = m.Description, amt = m.Fee ?? 0 };
            return q;
        }
        public class FundItemChosen
        {
            public string desc { get; set; }
            public int fundid { get; set; }
            public decimal amt { get; set; }
        }
        public IEnumerable<FundItemChosen> FundItemsChosen()
        {
            if (FundItem == null)
                return new List<FundItemChosen>();
            var items = Funds();
			var q = from i in FundItem
					join m in items on i.Key equals m.Value.ToInt()
					where i.Value.HasValue
					select new FundItemChosen { fundid = m.Value.ToInt(), desc = m.Text, amt = i.Value.Value };
            return q;
        }
        public IEnumerable<SelectListItem> GradeOptions()
        {
            var q = from s in setting.GradeOptions
                    select new SelectListItem { Text = s.Description, Value = s.Code.ToString() };
            var list = q.ToList();
            list.Insert(0, new SelectListItem { Text = "(please select)", Value = "00" });
            return list;
        }
        public static List<SelectListItem> ShirtSizes(CMSDataContext Db, Organization org)
        {
            var setting = new RegSettings(org.RegSetting, Db, org.OrganizationId);
            return ShirtSizes(setting);
        }
        private static List<SelectListItem> ShirtSizes(RegSettings setting)
        {
            var q = from ss in setting.ShirtSizes
                    select new SelectListItem
                    {
                        Value = ss.SmallGroup,
                        Text = ss.Description
                    };
            var list = q.ToList();
            list.Insert(0, new SelectListItem { Value = "0", Text = "(please select)" });
            if (setting.AllowLastYearShirt == true)
                list.Add(new SelectListItem { Value = "lastyear", Text = "Use shirt from last year" });
            return list;
        }
        public List<SelectListItem> ShirtSizes()
        {
            return ShirtSizes(setting);
        }
        public void FillPriorInfo()
        {
            if (!IsNew && LoggedIn == true)
            {
                var rr = DbUtil.Db.RecRegs.SingleOrDefault(r => r.PeopleId == PeopleId);
                if (rr != null)
                {
                    if (setting.AskRequest == true)
                    {
                        var om = GetOrgMember();
                        if (om != null)
                            request = om.Request;
                    }
                    if (setting.AskShirtSize == true)
                        shirtsize = rr.ShirtSize;
                    if (setting.AskEmContact == true)
                    {
                        emcontact = rr.Emcontact;
                        emphone = rr.Emphone;
                    }
                    if (setting.AskInsurance == true)
                    {
                        insurance = rr.Insurance;
                        policy = rr.Policy;
                    }
                    if (setting.AskDoctor == true)
                    {
                        docphone = rr.Docphone;
                        doctor = rr.Doctor;
                    }
                    if (setting.AskParents == true)
                    {
                        mname = rr.Mname;
                        fname = rr.Fname;
                    }
                    if (setting.AskAllergies == true)
                        medical = rr.MedicalDescription;
                    if (setting.AskCoaching == true)
                        coaching = rr.Coaching;
                    if (setting.AskChurch == true)
                    {
                        otherchurch = rr.ActiveInAnotherChurch ?? false;
                        memberus = rr.Member ?? false;
                    }
                    if (setting.AskTylenolEtc == true)
                    {
                        tylenol = rr.Tylenol;
                        advil = rr.Advil;
                        robitussin = rr.Robitussin;
                        maalox = rr.Maalox;
                    }
                }
            }
#if DEBUG2
            request = "Toby";
            ntickets = 1;
            gradeoption = "12";
            YesNoQuestion["Facebook"] = true;
            YesNoQuestion["Twitter"] = true;
            ExtraQuestion["Your Occupation"] = "programmer";
            ExtraQuestion["Your Favorite Snack"] = "peanuts";
            MenuItem["Fish"] = 1;
            MenuItem["Turkey"] = 0;
            option = "opt2";
            option2 = "none";
            paydeposit = false;
            Checkbox = new string[] { "PuttPutt", "Horseshoes" };
            shirtsize = "XL";
            emcontact = "dc";
            emphone = "br545";
            insurance = "bcbs";
            policy = "2424";
            doctor = "costalot";
            docphone = "35353365";
            tylenol = true;
            advil = true;
            maalox = false;
            robitussin = false;
            fname = "david carroll";
            coaching = false;
            paydeposit = false;
            grade = "4";
#endif
        }
        public bool NeedsCopyFromPrevious()
        {
            if (org != null)
                return (setting.AskEmContact == true
                    || setting.AskInsurance == true
                    || setting.AskDoctor == true
                    || setting.AskParents == true);
            return false;
        }
    }
}