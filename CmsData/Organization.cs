using System;
using System.Collections.Generic;
using System.Linq;
using UtilityExtensions;
using System.Text;

namespace CmsData
{
    public partial class Organization
    {
        public enum AttendanceClassificationCode
        {
            Normal = 0,
            InService = 1,
            Offsite = 2,
            Baptism = 3,
            Step1Class = 4
        }
        public enum OrgStatusCode
        {
            Create = 10,
            Review = 20,
            Active = 30,
            Inactive = 40,
        }
        public OrgStatusCode OrgStatusEnum
        {
            get { return (OrgStatusCode)OrganizationStatusId; }
            set { OrganizationStatusId = (int)value; }
        }

        public static string FormatOrgName(string name, string leader, string loc)
        {
            if (loc.HasValue())
                loc = ", " + loc;
            //return "{0}:{1}{2} ({3})".Fmt(name, leader, loc, count);
            return "{0}:{1}{2}".Fmt(name, leader, loc);
        }

        public string FullName
        {
            get { return FormatOrgName(OrganizationName, LeaderName, Location); }
        }
        private CMSDataContext _Db;
        public CMSDataContext Db
        {
            get
            {
                if (_Db == null)
                    _Db = this.GetDataContext() as CMSDataContext;
                return _Db;
            }
        }

        #region Tags

        private string _TagString;
        public string TagString
        {
            get
            {
                if (_TagString == null)
                {
                    var sb = new StringBuilder();
                    var q = from d in DivOrgs
                            orderby d.Division.Name
                            select d.Division.Name;
                    foreach (var name in q)
                        sb.Append(name + ";");
                    if (sb.Length > 0)
                        sb.Remove(sb.Length - 1, 1);
                    _TagString = sb.ToString();
                }
                return _TagString;
            }
            set
            {
                var a = value.Split(';');
                var qdelete = from d in DivOrgs
                              where !a.Contains(d.Division.Name)
                              select d;
                Db.DivOrgs.DeleteAllOnSubmit(qdelete);

                var q = from s in a
                        join d2 in DivOrgs on s equals d2.Division.Name into g
                        from d in g.DefaultIfEmpty()
                        where d == null
                        select s;

                foreach (var s in q)
                {
                    var div = Db.Divisions.SingleOrDefault(d => d.Name == s);
                    if (div == null)
                    {
                        div = new Division { Name = s };
                        string misctags = DbUtil.Settings("MiscTagsString");
                        var prog = Db.Programs.SingleOrDefault(p => p.Name == misctags);
                        if (prog == null)
                        {
                            prog = new Program { Name = misctags };
                            Db.Programs.InsertOnSubmit(prog);
                        }
                        div.Program = prog;
                    }
                    DivOrgs.Add(new DivOrg { Division = div });
                }
                _TagString = value;
            }
        }
        public bool ToggleTag(int divid)
        {
            var divorg = DivOrgs.SingleOrDefault(d => d.DivId == divid);
            if (divorg == null)
            {
                DivOrgs.Add(new DivOrg { DivId = divid });
                return true;
            }
            DivOrgs.Remove(divorg);
            Db.DivOrgs.DeleteOnSubmit(divorg);
            return false;
        }
        
        public List<string> TagPickList()
        {
            var q1 = from d in Db.DivOrgs
                      where d.OrgId == OrganizationId
                      orderby d.Division.Name
                      select d.Division.Name;
            var q2 = from p in Db.Programs
                      from d in p.Divisions
                      where !q1.Contains(d.Name)
                      orderby d.Name
                      select d.Name;
                      
            var list = q1.ToList();
            list.AddRange(q2);
            return list;
        }
        public bool PurgeOrg()
        {
            try
            {
                Db.PurgeOrganization(OrganizationId);
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}
