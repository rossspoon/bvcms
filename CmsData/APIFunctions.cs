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

namespace CmsData
{
    public class APIFunctions
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
        public static string Login(CMSDataContext Db, User u)
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
                return m.Run(api, u.Person);
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
    }
}
