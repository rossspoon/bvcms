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
using Microsoft.Scripting.Hosting;

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
        public static string TestAPI(string init, string script, Dictionary<string, string> namevalues)
        {
            var shell = @"
from System.Net import WebClient, NetworkCredential
from System.Collections.Specialized import NameValueCollection
from System.Text import Encoding
from System import Convert

{0}
class TestAPI(object):
	def Run(self):
{1}";
            try
            {
                var sb = new StringBuilder();
                var ss = script.Replace("\r\n", "\n");
                foreach (var s in script.Split('\n'))
                    sb.AppendFormat("\t\t{0}\n", s);
                var engine = Python.CreateEngine();
                script = shell.Fmt(init, sb.ToString());
                var sc = engine.CreateScriptSourceFromString(script);
                var code = sc.Compile();
                var scope = engine.CreateScope();
                foreach (var i in namevalues)
                    scope.SetVariable(i.Key, i.Value);
                code.Execute(scope);
                dynamic TestAPI = scope.GetVariable("TestAPI");
                dynamic m = TestAPI();
                var ret = m.Run();
                return HttpUtility.HtmlEncode(ret);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string Login(Person p)
        {
            var script = Db.Content("API-LoginInfo");
            if (script == null)
            {
                script = new Content();
                script.Body = @"
from System import *
from System.Text import *

class LoginInfo(object):

	def Run(self, m, w, p):
		w.Start('Login')
		w.Attr('PeopleId', p.PeopleId)
		w.Attr('PreferredName', p.PreferredName)
		w.Attr('Last', p.LastName)
		w.Attr('EmailAddress', p.EmailAddress)
		w.Attr('IsMember', p.MemberStatusId == 10)
		for b in m.QueryBits(p.PeopleId):
			w.Add('QueryBit', b);
		w.End()
		return w.ToString()
";
            }
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
                return m.Run(api, w, p);
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
        public void DeleteExtraValue(int peopleid, string field)
        {
            var q = from v in Db.PeopleExtras
                    where v.Field == field
                    where v.PeopleId == peopleid
                    select v;
            Db.PeopleExtras.DeleteAllOnSubmit(q);
            Db.SubmitChanges();
        }
        public string ExtraValues(int peopleid, string fields)
        {
            try
            {
                var a = (fields ?? "").Split(',');
                var nofields = !fields.HasValue();
                var q = from v in Db.PeopleExtras
                        where nofields || a.Contains(v.Field)
                        where v.PeopleId == peopleid
                        select v;
                var w = new APIWriter();
                w.Start("ExtraValues");
                w.Attr("Id", peopleid);
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
        public string AddEditExtraValue(int peopleid, string field, string value)
        {
            try
            {
                var q = from v in Db.PeopleExtras
                        where v.Field == field
                        where v.PeopleId == peopleid
                        select v;
                var ev = q.SingleOrDefault();
                if (ev == null)
                {
                    ev = new PeopleExtra
                    {
                        PeopleId = peopleid,
                        Field = field,
                        TransactionTime = DateTime.Now
                    };
                    Db.PeopleExtras.InsertOnSubmit(ev);
                }
                ev.StrValue = value;
                Db.SubmitChanges();
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
