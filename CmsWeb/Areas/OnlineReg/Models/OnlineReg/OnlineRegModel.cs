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
using CmsData.Codes;

namespace CmsWeb.Models
{
    [Serializable]
    public partial class OnlineRegModel
    {
        private IList<OnlineRegPersonModel> list = new List<OnlineRegPersonModel>();
        public IList<OnlineRegPersonModel> List
        {
            get { return list; }
            set { list = value; }
        }
        [NonSerialized]
        public OnlineRegPersonModel current;
        [NonSerialized]
        public bool ShowFindInstructions;
        [NonSerialized]
        public bool ShowLoginInstructions;
        [NonSerialized]
        public bool ShowOtherInstructions;
        [NonSerialized]
        private CmsData.Division _div;
        public CmsData.Division div
        {
            get
            {
                if (_div == null && divid.HasValue)
                    _div = DbUtil.Db.Divisions.SingleOrDefault(d => d.Id == divid);
                return _div;
            }
        }
        public void ParseSettings()
        {
            if (HttpContext.Current.Items.Contains("RegSettings"))
                return;
            var list = new Dictionary<int, RegSettings>();
            if (_Divid.HasValue)
            {
                var q = from o in DbUtil.Db.Organizations
                        where o.DivOrgs.Any(od => od.DivId == divid)
                        where o.OrganizationStatusId == OrgStatusCode.Active
                        where (o.RegistrationClosed ?? false) == false
                        where o.RegistrationTypeId != RegistrationTypeCode.None
                        select new { o.OrganizationId, o.RegSetting };
                foreach (var i in q)
                    list[i.OrganizationId] = new RegSettings(i.RegSetting, DbUtil.Db, i.OrganizationId);
            }
            else if (masterorgid.HasValue)
            {
                var q = from o in UserSelectClasses(masterorg)
                        select new { o.OrganizationId, o.RegSetting };
                foreach (var i in q)
                    list[i.OrganizationId] = new RegSettings(i.RegSetting, DbUtil.Db, i.OrganizationId);
                list[masterorg.OrganizationId] = new RegSettings(masterorg.RegSetting, DbUtil.Db, masterorg.OrganizationId);
            }
            else if (org == null)
                return;
            else
                list[_Orgid.Value] = new RegSettings(org.RegSetting, DbUtil.Db, _Orgid.Value);
            if (HttpContext.Current.Items.Contains("RegSettings"))
                return;
            HttpContext.Current.Items.Add("RegSettings", list);
        }
        public static RegSettings ParseSetting(string RegSetting, int OrgId)
        {
            return new RegSettings(RegSetting, DbUtil.Db, OrgId);
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
                {
                    _masterorg = DbUtil.Db.LoadOrganizationById(masterorgid.Value);
                    ParseSettings();
                }
                else
                {
                    if (org != null && (org.RegistrationTypeId == RegistrationTypeCode.UserSelectsOrganization2
                        || org.RegistrationTypeId == RegistrationTypeCode.ComputeOrganizationByAge2
                        || org.RegistrationTypeId == RegistrationTypeCode.ManageSubscriptions2))
                    {
                        _masterorg = org;
                        masterorgid = orgid;
                        _Orgid = null;
                        _org = null;
                    }
                }
                return _masterorg;
            }
        }
        public string URL { get; set; }

        [NonSerialized]
        private CmsData.Organization _org;
        public CmsData.Organization org
        {
            get
            {
                if (_org == null && orgid.HasValue)
                    if (orgid == Util.CreateAccountCode)
                        _org = CreateAccountOrg();
                    else
                        _org = DbUtil.Db.LoadOrganizationById(orgid.Value);
                return _org;
            }
        }

        private int? _Divid;
        public int? divid
        {
            get
            {
                return _Divid;
            }
            set
            {
                _Divid = value;
                if (value > 0)
                    ParseSettings();
            }
        }
        [OptionalField]
        private int? _masterorgid;
        public int? masterorgid
        {
            get { return _masterorgid; }
            set
            {
                _masterorgid = value;
                if (value > 0)
                    ParseSettings();
            }
        }
        private int? _Orgid;
        public int? orgid
        {
            get
            {
                return _Orgid;
            }
            set
            {
                _Orgid = value;
                if (value > 0)
                    ParseSettings();
            }
        }
        public int? classid { get; set; }
        public int? TranId { get; set; }
        [NonSerialized]
        private Transaction _Transaction;
        public Transaction Transaction
        {
            get
            {
                if (_Transaction == null && TranId.HasValue)
                    _Transaction = DbUtil.Db.Transactions.SingleOrDefault(tt => tt.Id == TranId);
                return _Transaction;
            }
        }
        public string username { get; set; }
        public string password { get; set; }
        public bool nologin { get; set; }
        public decimal? donation { get; set; }
        public int? donor { get; set; }
        public int? UserPeopleId { get; set; }
        [OptionalField]
        private string _Registertag;
        public string registertag
        {
            get { return _Registertag; }
            set { _Registertag = value; }
        }
        [NonSerialized]
        private Person _User;
        public Person user
        {
            get
            {
                if (_User == null && UserPeopleId.HasValue)
                    _User = DbUtil.Db.LoadPersonById(UserPeopleId.Value);
                return _User;
            }
        }

        private CmsData.Meeting _meeting;
        public CmsData.Meeting meeting()
        {
            if (_meeting == null)
            {
                var q = from m in DbUtil.Db.Meetings
                        where m.Organization.OrganizationId == orgid
                        where m.MeetingDate > Util.Now.AddHours(-12)
                        orderby m.MeetingDate
                        select m;
                _meeting = q.FirstOrDefault();
            }
            return _meeting;
        }

        //public OnlineRegModel()
        //{
        //}
        //protected OnlineRegModel(SerializationInfo si, StreamingContext context)
        //{
        //    UserPeopleId = (int?)si.GetValue("UserPeopleId", typeof(int?));
        //    URL = si.GetString("URL");
        //    classid = (int?)si.GetValue("classid", typeof(int?));
        //    divid = (int?)si.GetValue("divid", typeof(int?));
        //    masterorgid = (int?)si.GetValue("masterorgid", typeof(int?));
        //    nologin = si.GetBoolean("nologin");
        //    orgid = (int?)si.GetValue("orgid", typeof(int?));
        //    testing = si.GetBoolean("testing");
        //    username = si.GetString("username");
        //}
        //public void GetObjectData(SerializationInfo info, StreamingContext context)
        //{
        //    throw new NotImplementedException();
        //}
        public void CreateList()
        {
#if DEBUG2
            List = new List<OnlineRegPersonModel>
            {
                new OnlineRegPersonModel
                {
                    divid = divid,
                    orgid = orgid,
                    masterorgid = masterorgid,
                    first = "David",
                    last = "Carroll",
                    dob = "5/30/52",
                    email = "david@bvcms.com",
                    phone = "",
                    LoggedIn = false,
                }
            };
#else
            List = new List<OnlineRegPersonModel>
            {
                new OnlineRegPersonModel
                {
                    divid = divid,
                    orgid = orgid,
                    masterorgid = masterorgid,
                    LoggedIn = false,
                }
            };
#endif
        }
    }
}
