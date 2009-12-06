using System;
using System.Text;
using System.Web;
using System.Linq;
using UtilityExtensions;

namespace CmsData
{
    public class ContentService
    {
        #region Page Methods

        public static PageContent GetPage(string pageUrl)
        {
            var p = PageContent.LoadByUrl(pageUrl);
            if (p == null)
                throw new Exception("There is no page corresponding to " + pageUrl);
            return p;
        }

        public static PageContent GetPage(int pageID)
        {
            var p = PageContent.LoadById(pageID);
            if (p == null)
                throw new Exception("There is no page corresponding to " + pageID.ToString());
            return p;
        }

        public static ParaContent GetContent(string contentName)
        {
            return DbUtil.Db.ParaContents.SingleOrDefault(c => c.ContentName == contentName);
        }
        public static ParaContent GetContent(int id)
        {
            return DbUtil.Db.ParaContents.SingleOrDefault(c => c.ContentID == id);
        }
        public static bool ContentExists(string contentName)
        {
            return 0 < DbUtil.Db.ParaContents.Count(c => c.ContentName == contentName);
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

        public static void SavePage(PageContent p)
        {
            if (p.PageID == 0)
            {
                DbUtil.Db.PageContents.InsertOnSubmit(p);
                p.PageUrl = TransformTitleToUrl(p.Title);
                int existingUrls = DbUtil.Db.PageContents.Count(pg => pg.PageUrl == p.PageUrl);
                if (existingUrls > 0)
                {
                    existingUrls++;
                    p.PageUrl = p.PageUrl.Replace(".aspx", "_{0}.aspx".Fmt(existingUrls));
                }
                p.CreatedById = DbUtil.Db.CurrentUser.UserId;
                p.CreatedOn = DateTime.Now;
            }
            else
            {
                p.ModifiedById = DbUtil.Db.CurrentUser.UserId;
                p.ModifiedOn = Util.Now;
            }
            DbUtil.Db.SubmitChanges();
        }

        public static void DeletePage(int pageID)
        {
            var p = PageContent.LoadById(pageID);
            DbUtil.Db.PageContents.DeleteOnSubmit(p);
            DbUtil.Db.SubmitChanges();
        }
        #endregion
    }
}
