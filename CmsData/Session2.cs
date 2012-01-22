using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using UtilityExtensions;
using System.Collections;
using System.Web.SessionState;

namespace CmsData
{
    public partial class CMSDataContext
    {
        public int? CurrentOrgId { get; set; }
        public int[] CurrentGroups { get; set; }
        public int CurrentPeopleId { get; set; }
        public int? CurrentTagOwnerId { get; set; }
        public string CurrentTagName { get; set; }
        public int VisitLookbackDays { get; set; }
        public bool OrgMembersOnly { get; set; }
        public bool OrgLeadersOnly { get; set; }
        public string Host { get; set; }

        public void CopySession()
        {
            if (Util.SessionId != null)
            {
                CurrentOrgId = Util2.CurrentOrgId;
                CurrentGroups = Util2.CurrentGroups;
                CurrentPeopleId = Util2.CurrentPeopleId;
                CurrentTagOwnerId = Util2.CurrentTagOwnerId;
                CurrentTagName = Util2.CurrentTagName;
                OrgMembersOnly = Util2.OrgMembersOnly;
                OrgLeadersOnly = Util2.OrgLeadersOnly;
                VisitLookbackDays = Util2.VisitLookbackDays;
                Host = Util.Host;
            }
        }
        public Session2 ExportSession()
        {
            var s = new Session2();
            s.CurrentOrgId = CurrentOrgId;
            s.CurrentGroups = CurrentGroups;
            s.CurrentPeopleId = CurrentPeopleId;
            s.CurrentTagOwnerId = CurrentTagOwnerId;
            s.CurrentTagName = CurrentTagName;
            s.OrgMembersOnly = OrgMembersOnly;
            s.OrgLeadersOnly = OrgLeadersOnly;
            s.VisitLookbackDays = VisitLookbackDays;
            s.Host = Host;
            return s;
        }
        public void ImportSession(Session2 s)
        {
            CurrentOrgId = s.CurrentOrgId;
            CurrentGroups = s.CurrentGroups;
            CurrentPeopleId = s.CurrentPeopleId;
            CurrentTagOwnerId = s.CurrentTagOwnerId;
            CurrentTagName = s.CurrentTagName;
            OrgMembersOnly = s.OrgMembersOnly;
            OrgLeadersOnly = s.OrgLeadersOnly;
            VisitLookbackDays = s.VisitLookbackDays;
            Host = s.Host;
        }
        public string Setting(string name, string defaultvalue)
        {
            var list = HttpRuntime.Cache[Host + "Setting"] as Dictionary<string, string>;
            if (list == null)
            {
                try
                {
                    list = Settings.ToDictionary(c => c.Id, c => c.SettingX,
                        StringComparer.OrdinalIgnoreCase);
                    HttpRuntime.Cache[Host + "Setting"] = list;
                }
                catch (Exception ex)
                {
                    return string.Empty;
                }
            }
            if (list.ContainsKey(name))
                return list[name];
            if (defaultvalue.HasValue())
                return defaultvalue;
            return string.Empty;
        }
        public void SetSetting(string name, string value)
        {
            var list = HttpRuntime.Cache[Host + "Setting"] as Dictionary<string, string>;
            if (list == null)
            {
                list = Settings.ToDictionary(c => c.Id, c => c.SettingX);
                HttpRuntime.Cache[Host + "Setting"] = list;
            }
            list[name] = value;

            var setting = Settings.SingleOrDefault(c => c.Id == name);
            if (setting == null)
            {
                setting = new Setting { Id = name, SettingX = value };
                Settings.InsertOnSubmit(setting);
            }
            else
                setting.SettingX = value;
        }
        public void DeleteSetting(string name)
        {
            var list = HttpRuntime.Cache[Host + "Setting"] as Dictionary<string, string>;
            if (list == null)
            {
                list = Settings.ToDictionary(c => c.Id, c => c.SettingX);
                HttpRuntime.Cache[Host + "Setting"] = list;
            }
            list.Remove(name);

            var setting = Settings.SingleOrDefault(c => c.Id == name);
            if (setting != null)
                Settings.DeleteOnSubmit(setting);
        }
    }
    public class Session2
    {
        public int? CurrentOrgId { get; set; }
        public int[] CurrentGroups { get; set; }
        public int CurrentPeopleId { get; set; }
        public int? CurrentTagOwnerId { get; set; }
        public string CurrentTagName { get; set; }
        public int VisitLookbackDays { get; set; }
        public bool OrgMembersOnly { get; set; }
        public bool OrgLeadersOnly { get; set; }
        public string Host { get; set; }
    }
}
