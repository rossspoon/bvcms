using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using UtilityExtensions;

namespace CmsData
{
    public static class Util2
    {
        private const string STR_CurrentTag = "CurrentTag";
        private const string STR_DefaultTag = "UnNamed";
        public static object GetSessionObj(string key, object def)
        {
            if (HttpContext.Current != null)
                if (HttpContext.Current.Session != null)
                    if (HttpContext.Current.Session[key] != null)
                        return HttpContext.Current.Session[key];
            return def;
        }
        public static string CurrentTag
        {
            get
            {
                return GetSessionObj(STR_CurrentTag, STR_DefaultTag).ToString();
            }
            set
            {
                if (HttpContext.Current != null)
                    HttpContext.Current.Session[STR_CurrentTag] = value;
            }
        }
        const string STR_ActiveOrganizationId = "ActiveOrganizationId";
        public static int CurrentOrgId
        {
            get
            {
                return GetSessionObj(STR_ActiveOrganizationId, 0).ToInt();
            }
            set
            {
                if (HttpContext.Current != null)
                    HttpContext.Current.Session[STR_ActiveOrganizationId] = value;
            }
        }
        const string STR_ActiveGroupId = "ActiveGroup";
        public static int[] CurrentGroups
        {
            get
            {
                return (int[])GetSessionObj(STR_ActiveGroupId, new int[] { 0 });
            }
            set
            {
                if (value == null)
                    value = new int[] { 0 };
                if (HttpContext.Current != null)
                    HttpContext.Current.Session[STR_ActiveGroupId] = value;
            }
        }
        const string STR_ActivePersonId = "ActivePersonId";
        public static int CurrentPeopleId
        {
            get
            {
                return GetSessionObj(STR_ActivePersonId, 0).ToInt();
            }
            set
            {
                if (HttpContext.Current != null)
                    HttpContext.Current.Session[STR_ActivePersonId] = value;
            }
        }
        public static int? CurrentTagOwnerId
        {
            get
            {
                var pid = Util.UserPeopleId;
                var a = CurrentTag.Split(':');
                if (a.Length > 1)
                    pid = a[0].ToInt2();
                return pid;
            }
        }
        public static string CurrentTagName
        {
            get
            {
                string tag = CurrentTag;
                var a = tag.Split(':');
                if (a.Length == 2)
                    return a[1];
                return tag;
            }
        }
        public const string STR_OrgMembersOnly = "OrgMembersOnly";
        public static bool OrgMembersOnly
        {
            get
            {
                return (bool)GetSessionObj(STR_OrgMembersOnly, false);
            }
            set
            {
                if (HttpContext.Current != null)
                    HttpContext.Current.Session[STR_OrgMembersOnly] = value;
            }
        }
        private const string STR_VisitLookbackDays = "VisitLookbackDays";
        public static int VisitLookbackDays
        {
            get
            {
                return GetSessionObj(STR_VisitLookbackDays, 180).ToInt();
            }
            set
            {
                if (HttpContext.Current != null)
                    HttpContext.Current.Session[STR_VisitLookbackDays] = value;
            }
        }
    }
}
