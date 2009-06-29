/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using UtilityExtensions;
using System.Xml.Linq;

namespace CmsData
{
    public static class DbUtil
    {
        private const string CMSDbKEY = "CMSDbKey";
        private static CMSDataContext InternalDb
        {
            get
            {
                return (CMSDataContext)HttpContext.Current.Items[CMSDbKEY];
            }
            set
            {
                HttpContext.Current.Items[CMSDbKEY] = value;
            }
        }
        public static CMSDataContext Db
        {
            get
            {
                if (HttpContext.Current == null)
                    return new CMSDataContext(Util.ConnectionString);
                if (InternalDb == null)
                {
                    InternalDb = new CMSDataContext(Util.ConnectionString);
                    //InternalDb.CommandTimeout = 1200;
                }
                return InternalDb;
            }
            set
            {
                InternalDb = value;
            }
        }
        public static void LogActivity(string activity)
        {
            LogActivity(activity, true);
        }
        public static void LogActivity(string activity, bool recordPageUrl)
        {
            var db = new CMSDataContext(Util.ConnectionString);
            var a = new ActivityLog
            {
                ActivityDate = Util.Now,
                UserId = Util.UserId1,
                Activity = activity,
                PageUrl = recordPageUrl ? HttpContext.Current.Request.RawUrl : null,
            };
            db.ActivityLogs.InsertOnSubmit(a);
            db.SubmitChanges();
            db.Dispose();
        }
        public static void DbDispose()
        {
            if (InternalDb != null)
            {
                InternalDb.Dispose();
                InternalDb = null;
            }
        }
        public static string Header()
        {
            var list = HttpContext.Current.Cache["Settings"] as Dictionary<string, string>;
            if (list == null)
            {

            }
            var hc = HttpContext.Current.Cache["header"] as string;
            if (hc == null)
            {
                var h = Db.Contents.SingleOrDefault(c => c.Name == "Header");
                if (h != null)
                    hc = h.Body;
                else
                    hc = @"
<div id='CommonHeaderImage'>
    <a href='/'><img src='/images/headerimage.jpg' /></a>
</div>
<div id='CommonHeaderTitleBlock'>
    <h1 id='CommonHeaderTitle'>Bellevue Baptist Church</h1>
    <h2 id='CommonHeaderSubTitle'>Feed My Sheep</h2>
</div>
";
                HttpContext.Current.Cache["header"] = hc;
            }
            return hc;
        }
        public static string Settings(string name)
        {
            if (HttpContext.Current == null)
                return "";
            var list = HttpContext.Current.Cache["Settings"] as Dictionary<string, string>;
            if (list == null)
            {
                list = Db.Settings.ToDictionary(c => c.Id, c => c.SettingX);
                HttpContext.Current.Cache["Settings"] = list;
            }
            if (list.ContainsKey(name))
                return list[name];
            return string.Empty;
        }
        public static int BFClassOrgTagId { get { return Settings("BFClassOrgTagId").ToInt(); } }
        public static int NewPeopleManagerId { get { return Settings("NewPeopleManagerId").ToInt(); } }
        public static string SystemEmailAddress { get { return Settings("SystemEmailAddress"); } }
        public static string TaskHost { get { return Settings("DefaultHost"); } }
        public static bool CheckRemoteAccessRole { get { return Settings("CheckRemoteAccessRole") == "true"; } }

        public const string MiscTagsString = "Misc Tags";
        public const int TagTypeId_Personal = 1;
        public const int TagTypeId_OrgMembersOnly = 3;
        public const int TagTypeId_CouplesHelper = 4;
        public const int TagTypeId_AddSelected = 5;
    }
}
