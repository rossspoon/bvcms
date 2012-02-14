using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using System.Text;
using UtilityExtensions;
using System.Web.Mvc;
using System.Xml.Linq;
using CmsData.Codes;
using System.Configuration;

namespace CmsWeb.Models
{
    public class ManageGivingModel
    {
        public int pid { get; set; }
        public int orgid { get; set; }
        public DateTime? StartWhen { get; set; }
        public DateTime? StopWhen { get; set; }
        public string SemiEvery { get; set; }
        public int? Day1 { get; set; }
        public int? Day2 { get; set; }
        public int? EveryN { get; set; }
        public string Period { get; set; }
        public string Type { get; set; }
        public string Cardnumber { get; set; }
        public string Expires { get; set; }
        public string Cardcode { get; set; }
        public string Routing { get; set; }
        public string Account { get; set; }
        public bool testing { get; set; }
        public decimal total { get; set; }

        private Dictionary<int, decimal?> _FundItem = new Dictionary<int, decimal?>();
        public Dictionary<int, decimal?> FundItem
        {
            get { return _FundItem; }
            set { _FundItem = value; }
        }
        public decimal? FundItemValue(int n)
        {
            if (FundItem.ContainsKey(n))
                return FundItem[n];
            return null;
        }

        private Person _Person;
        public Person person
        {
            get
            {
                if (_Person == null)
                    _Person = DbUtil.Db.LoadPersonById(pid);
                return _Person;
            }
        }
        private Organization _organization;
        public Organization Organization
        {
            get
            {
                if (_organization == null)
                    _organization = DbUtil.Db.Organizations.Single(d => d.OrganizationId == orgid);
                return _organization;
            }
        }
        public RegSettings setting
        {
            get
            {
                return new RegSettings(Organization.RegSetting, DbUtil.Db, orgid);
            }
        }
        public ManageGivingModel()
        {
            testing = ConfigurationManager.AppSettings["testing"].ToBool();
#if DEBUG
            testing = true;
#endif
        }
        public ManageGivingModel(int pid, int orgid)
            : this()
        {
            this.pid = pid;
            this.orgid = orgid;
            var rg = person.RecurringGivings.FirstOrDefault();
            if (rg != null)
            {
                SemiEvery = rg.SemiEvery;
                Type = rg.Type;
                StartWhen = rg.StartWhen;
                StopWhen = rg.StopWhen;
                Day1 = rg.Day1;
                Day2 = rg.Day2;
                Period = rg.Period;
                foreach (var ra in person.RecurringAmounts.AsEnumerable())
                    FundItem.Add(ra.FundId, ra.Amt);
                Cardnumber = rg.MaskedCard;
                Account = rg.MaskedAccount;
                Expires = rg.Expires;
                Cardcode = rg.Ccv;
            }
            total = FundItem.Sum(ff => ff.Value) ?? 0;
        }
        public void ValidateModel(ModelStateDictionary ModelState)
        {
            if (Type == "C")
            {
                if (!ValidateCard(Cardnumber))
                    ModelState.AddModelError("Cardnumber", "invalid card number");
                Expires = Expires.Trim();
                DateTime dt;
                if (Expires.Length != 4)
                    ModelState.AddModelError("Expires", "invalid expiration date (MMYY)");
                else
                {
                    var s = Expires.Insert(2, "/15/");
                    if (!DateTime.TryParse(s, out dt))
                        ModelState.AddModelError("Expires", "invalid expiration date (MMYY)");
                }
                var ccvlen = Cardcode.GetDigits().Length;
                if (ccvlen < 3 || ccvlen > 4)
                    ModelState.AddModelError("Cardcode", "invalid Cardcode");
            }
            if (Type == "B")
            {
                if (!checkABA(Routing))
                    ModelState.AddModelError("Routing", "invalid routing number");
            }
            if (SemiEvery == "S")
            {
                if (!Day1.HasValue || !Day2.HasValue)
                    ModelState.AddModelError("Semi", "Both Days must have values");
                else if (Day2 > 31)
                    ModelState.AddModelError("Semi", "Day2 must be 31 or less");
                else if (Day1 >= Day2)
                    ModelState.AddModelError("Semi", "Day1 must be less than Day2");
            }
            else if (SemiEvery == "E")
            {
                if (!EveryN.HasValue || EveryN < 1)
                    ModelState.AddModelError("Every", "Days must be > 0");
            }
            else
                ModelState.AddModelError("Every", "Must Choose Payment Frequency");
            if (!StartWhen.HasValue)
                ModelState.AddModelError("StartWhen", "StartDate must have a value");
            else if (StartWhen < DateTime.Today)
                ModelState.AddModelError("StartWhen", "StartDate must occur after today");
            else if (!StopWhen.HasValue)
                ModelState.AddModelError("StopWhen", "StopDate must have a value");
            else if (StopWhen <= StartWhen)
                ModelState.AddModelError("StopWhen", "StopDate must occur after StartDate");
        }
        private bool checkABA(string s)
        {
            var t = s.GetDigits();
            if (t.Length != 9)
                return false;

            var n = 0;
            for (var i = 0; i < t.Length; i += 3)
                n += t[i] * 3 + t[i + 1] * 7 + t[i + 2];
            if (n != 0 && n % 10 == 0)
                return true;
            else
                return false;
        }
        public static bool ValidateCard(string s)
        {
            if (s.StartsWith("X"))
                return true;
            var number = new int[16];
            int len = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (char.IsDigit(s, i))
                {
                    if (len == 16)
                        return false;
                    number[len++] = s[i] - '0';
                }
            }

            switch (s[0])
            {
                case '5':
                    if (len != 16)
                        return false;
                    if (number[1] == 0 || number[1] > 5)
                        return false;
                    break;

                case '4':
                    if (len != 16 && len != 13)
                        return false;
                    break;

                case '3':
                    if (len != 15)
                        return false;
                    if ((number[1] != 4 && number[1] != 7))
                        return false;
                    break;

                case '6':
                    if (len != 16)
                        return false;
                    if (number[1] != 0 || number[2] != 1 || number[3] != 1)
                        return false;
                    break;
            }
            int sum = 0;
            for (int i = len - 1; i >= 0; i--)
                if (i % 2 == len % 2)
                {
                    int n = number[i] * 2;
                    sum += (n / 10) + (n % 10);
                }
                else
                    sum += number[i];
            return sum % 10 == 0;
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
            var items = OnlineRegPersonModel.Funds();
            var q = from i in FundItem
                    join m in items on i.Key equals m.Value.ToInt()
                    where i.Value.HasValue
                    select new FundItemChosen { fundid = m.Value.ToInt(), desc = m.Text, amt = i.Value.Value };
            return q;
        }
        public Decimal Total()
        {
            return FundItemsChosen().Sum(f => f.amt);
        }
        public object Autocomplete
        {
            get
            {
#if DEBUG
                return new { AUTOCOMPLETE = "on" };
#else
                return new { AUTOCOMPLETE = "off" };
#endif
            }
        }
    }
}
