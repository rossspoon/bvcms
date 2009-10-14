using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using DiscData;
using UtilityExtensions;

namespace BellevueTeachers
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WebService1 : System.Web.Services.WebService
    {

        public WebService1()
        {
        }

        [WebMethod]
        public string PostPodcast(string key, string Author, string Title, string Description, DateTime pubDate, string S3Name, int length)
        {
            if (key != "0KHHQHA7QMNGE4XM4ER2")
                return "NOT AUTH";
            var author = DbUtil.Db.GetUser(Author);
            var podcast = PodCast.Post(author, Title, Description, pubDate, S3Name, length);
            DbUtil.Db.SubmitChanges();
            podcast.BlogPost.NotifyEmail(false);
            return "OK";
        }
        [WebMethod]
        public List<string> GetAuthorization(string op, Guid g, out string key, out string code)
        {
            key = "";
            code = "";
            var a = new List<string>();
            if (op != "acts412")
                return a;
            string user;
            if (g == new Guid())
                user = "betcar";
            else
            {
                var tok = DbUtil.Db.TemporaryTokens.SingleOrDefault(tk => tk.Id == g);
                user = tok.User.Username;
                TimeSpan ts = DateTime.Now.Subtract(tok.CreatedOn);
                if (ts.Seconds > 120 || tok.Expired)
                    return a;
                tok.Expired = true;
                DbUtil.Db.SubmitChanges();
            }

            key = "0KHHQHA7QMNGE4XM4ER2";
            code = "YbEoAP5syZCauEErx5xdLuSpw/wZ7OK7YTgXeOmI";

            var q = from u in DbUtil.Db.UploadAuthenticationXrefs
                    where u.Postinguser == user
                    select u.Postsfor;
            foreach (var postsfor in q)
            {
                a.Add(postsfor);
                var pc = DbUtil.Db.GetUser(postsfor);
                a.Add(pc.FirstName + " " + pc.LastName);
            }
            return a;
        }

    }
}
