using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilityExtensions;
using System.Text.RegularExpressions;
using System.Data.Linq;
using System.Xml.Linq;
using System.Data.Linq.SqlClient;
using IronPython.Hosting;
using System.IO;
using CmsData.Codes;
using System.Web;
using System.Xml;
using System.Diagnostics;

namespace CmsData.API
{
    public partial class APIFunctions
    {
        private CMSDataContext Db;

        public APIFunctions()
        {
            Db = new CMSDataContext("Data Source=.;Initial Catalog=CMS_bellevue;Integrated Security=True");
        }
        public APIFunctions(CMSDataContext Db)
        {
            this.Db = Db;
        }
        public string Login(User u)
        {
            var script = DbUtil.Content("API-LoginInfo");
            if (script == null)
                return "<login error=\"no API-LoginInfo script\" />";
            var engine = Python.CreateEngine();
            var sc = engine.CreateScriptSourceFromString(script.Body);
            try
            {
                var code = sc.Compile();
                var scope = engine.CreateScope();
                code.Execute(scope);

                dynamic LoginInfo = scope.GetVariable("LoginInfo");
                dynamic m = LoginInfo();
                var api = new APIFunctions(Db);
                var w = new APIWriter();
                return m.Run(api, w, u.Person);
            }
            catch (Exception ex)
            {
                return "<login error=\"API-LoginInfo script error: {0}\" />".Fmt(ex.Message);
            }
        }
        public int AttendCount(int orgid, int PeopleId)
        {
            var q = from a in Db.Attends
                    where a.OrganizationId == orgid
                    where a.PeopleId == PeopleId
                    where a.AttendanceFlag == true
                    select a;
            return q.Count();
        }
        public List<string> QueryBits(int PeopleId)
        {
            var q1 = (from f in Db.QueryBitsFlags()
                      select f).ToList();
            var q2 = (from t in Db.TagPeople
                      where t.PeopleId == 828612
                      where t.Tag.TagType.Id == 100
                      select t.Tag.Name).ToList();
            var q = from t in q2
                    join f in q1 on t equals f[0]
                    select f[1];
            var list = q.ToList();
            return list;
        }
        public DateTime LastAttendDate(int orgid, int PeopleId)
        {
            var q = from a in Db.Attends
                    where a.OrganizationId == orgid
                    where a.PeopleId == PeopleId
                    where a.AttendanceFlag == true
                    orderby a.MeetingDate descending
                    select a.MeetingDate;
            return q.FirstOrDefault();
        }
        public int IsMemberOf(int orgid, int PeopleId)
        {
            var q = from a in Db.OrganizationMembers
                    where a.OrganizationId == orgid
                    where a.PeopleId == PeopleId
                    where a.MemberTypeId != Codes.MemberTypeCode.InActive
                    select a;
            return q.Count();
        }
        public string ExtraValues(int id, string fields)
        {
            var a = fields.Split(',');
            try
            {
                var q = from v in DbUtil.Db.PeopleExtras
                        where a.Contains(v.Field)
                        where v.PeopleId == id
                        select v;
                var w = new APIWriter();
                w.Start("ExtraValues");
                foreach (var v in q)
                    w.Add(v.Field, v.StrValue);
                w.End();
                return w.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static string AddEditExtraValue(int id, string field, string value)
        {
            try
            {
                var q = from v in DbUtil.Db.PeopleExtras
                        where v.Field == field
                        where v.PeopleId == id
                        select v;
                var ev = q.SingleOrDefault();
                if (ev == null)
                {
                    ev = new PeopleExtra
                    {
                        PeopleId = id,
                        Field = field,
                        TransactionTime = DateTime.Now
                    };
                    DbUtil.Db.PeopleExtras.InsertOnSubmit(ev);
                }
                ev.StrValue = value;
                DbUtil.Db.SubmitChanges();
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
