using System;
using System.Configuration;
using System.Web;

namespace DiscData
{
    public static class DbUtil
    {
        private const string DATACONTEXT_KEY = "LinqUtilDataContextKey";

        private static DiscDataContext InternalDataContext
        {
            get
            {
                return (DiscDataContext)HttpContext.Current.Items[DATACONTEXT_KEY];
            }
            set
            {
                HttpContext.Current.Items[DATACONTEXT_KEY] = value;
            }
        }

        public static DiscDataContext Db
        {
            get
            {
                if (InternalDataContext == null)
                    InternalDataContext = new DiscDataContext(ConfigurationManager.ConnectionStrings["Disc"].ConnectionString);
                return InternalDataContext;
            }
            set
            {
                InternalDataContext = value;
            }
        }
        public static void CleanUp()
        {
            if (InternalDataContext != null)
            {
                InternalDataContext.Dispose();
                InternalDataContext = null;
            }
        }
    }
}