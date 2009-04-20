using System;
using System.Text;
using System.Web;
using System.Linq;

namespace BTeaData
{
    public class ContentService
    {
        #region Page Methods

        public static Page GetPage(string pageUrl)
        {
            var p = Page.LoadByUrl(pageUrl);
            if (p == null)
                throw new Exception("There is no page corresponding to " + pageUrl);
            return p;
        }

        public static Page GetPage(int pageID)
        {
            var p = Page.LoadById(pageID);
            if (p == null)
                throw new Exception("There is no page corresponding to " + pageID.ToString());
            return p;
        }

        public static Content GetContent(string contentName)
        {
            return DbUtil.Db.Contents.SingleOrDefault(c => c.ContentName == contentName);
        }
        public static Content GetContent(int id)
        {
            return DbUtil.Db.Contents.SingleOrDefault(c => c.ContentID == id);
        }
        public static bool ContentExists(string contentName)
        {
            return 0 < DbUtil.Db.Contents.Count(c => c.ContentName == contentName);
        }

        static string StripNonAlphaNumeric(string sIn)
        {
            StringBuilder sb = new StringBuilder(sIn);
            char c = " ".ToCharArray()[0];
            string stripList = ".'?\\/><$!@%^*&+,;:\"{}[]|#";
            for (int i = 0; i < stripList.Length; i++)
                sb.Replace(stripList[i], c);
            sb.Replace(" ", String.Empty);
            return sb.ToString();
        }

        static string TransformTitleToUrl(string title)
        {
            string result = title;
            result = result.Replace(" ", "-");
            result = StripNonAlphaNumeric(result);
            result = result.ToLower();
            if (!result.EndsWith(".aspx"))
                result += ".aspx";
            return result;
        }

        public static void SavePage(Page p)
        {
            if (p.PageID == 0)
            {
                DbUtil.Db.Pages.InsertOnSubmit(p);
                p.PageUrl = TransformTitleToUrl(p.Title);
                int existingUrls = DbUtil.Db.Pages.Count(pg => pg.PageUrl == p.PageUrl);
                if (existingUrls > 0)
                {
                    existingUrls++;
                    p.PageUrl = p.PageUrl.Replace(".aspx", "_{0}.aspx".Fmt(existingUrls));
                }
                p.CreatedById = Util.CurrentUser.UserId;
                p.CreatedOn = DateTime.Now;
            }
            else
            {
                p.ModifiedById = Util.CurrentUser.UserId;
                p.ModifiedOn = DateTime.Now;
            }
            DbUtil.Db.SubmitChanges();
        }

        public static void DeletePage(int pageID)
        {
            var p = Page.LoadById(pageID);
            DbUtil.Db.Pages.DeleteOnSubmit(p);
            DbUtil.Db.SubmitChanges();
        }
        #endregion
    }
}
