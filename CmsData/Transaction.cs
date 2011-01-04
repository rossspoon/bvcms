using System;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Net.Mail;
using UtilityExtensions;

namespace CmsData
{
    public partial class Transaction
    {
        public string ServiceUOrgID
        {
            get
            {
                if (Testing ?? false)
                    return DbUtil.Db.Setting("ServiceUOrgIDTest", "0"); 
                return DbUtil.Db.Setting("ServiceUOrgID", "0");
            }
        }
        public string ServiceUOrgAccountID
        {
            get
            {
                if (Testing ?? false)
                    return DbUtil.Db.Setting("ServiceUOrgAccountIDTest", "0");
                return DbUtil.Db.Setting("ServiceUOrgAccountID", "0");
            }
        }
    }
}