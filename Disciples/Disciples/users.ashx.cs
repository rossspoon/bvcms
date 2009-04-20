using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;
using System.Configuration;

namespace BellevueTeachers
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class users : IHttpHandler
    {

        private class UserInfo
        {
            public string lowUser { get; set; }
            public string User { get; set; }
            public bool FromCms { get; set; }
            public bool FromDisciples { get; set; }
            public override string ToString()
            {
                return (FromCms ? "C" : "") + (FromDisciples ? "D" : "") + "\t" + User;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var list = new Dictionary<string, UserInfo>();
            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["CMS"].ConnectionString))
            {
                cn.Open();
                var cmd = new SqlCommand("select username from dbo.Users", cn);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var u = new UserInfo
                    {
                        FromCms = true,
                        User = rdr.GetString(0),
                        lowUser = rdr.GetString(0).ToLower(),
                    };
                    list.Add(u.lowUser, u);
                }
            }
            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["BTea"].ConnectionString))
            {
                cn.Open();
                var cmd = new SqlCommand("select username from aspnet_Users where IsAnonymous = 0", cn);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var k = rdr.GetString(0).ToLower();
                    if (list.ContainsKey(k))
                    {
                        var u = list[k];
                        u.FromDisciples = true;
                    }
                    else
                    {
                        var u = new UserInfo
                        {
                            FromDisciples = true,
                            User = rdr.GetString(0),
                            lowUser = rdr.GetString(0).ToLower(),
                        };
                        list.Add(u.lowUser, u);
                    }
                }
            }
            var q = from di in list
                    orderby di.Key
                    select di.Value;
            foreach (var u in q)
                context.Response.Write(u + "\n");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
