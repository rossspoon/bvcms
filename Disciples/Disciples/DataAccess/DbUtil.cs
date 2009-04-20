using System;
using System.Configuration;
using System.Web;

namespace BTeaData
{
    public static class DbUtil
    {
        private const string DATACONTEXT_KEY = "LinqUtilDataContextKey";

        private static BTeaDataContext InternalDataContext
        {
            get
            {
                return (BTeaDataContext)HttpContext.Current.Items[DATACONTEXT_KEY];
            }
            set
            {
                HttpContext.Current.Items[DATACONTEXT_KEY] = value;
            }
        }

        public static BTeaDataContext Db
        {
            get
            {
                if (InternalDataContext == null)
                    InternalDataContext = new BTeaDataContext(ConfigurationManager.ConnectionStrings["BTea"].ConnectionString);
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