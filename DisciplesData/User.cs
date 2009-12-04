using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.ComponentModel;
using System.Collections.Generic;

namespace DiscData.View
{
    public partial class UserList
    {
        public bool IsOnLine
        {
            get
            {
                var onlineSpan = new TimeSpan(0, Membership.UserIsOnlineTimeWindow, 0);
                var compareTime = Util.Now.Subtract(onlineSpan);
                return LastVisit > compareTime;// && LastVisit != CreationDate;
            }
        }
        public string Name
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
    }
}
namespace DiscData
{
    public partial class User
    {
        public string Name
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
    }
}
