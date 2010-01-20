/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using CmsData;
using CmsData.View;
using System.Collections;
using UtilityExtensions;
using com.qas.proweb;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;

namespace CMSPresenter
{
    [DataObject]
    public class PersonController
    {
        private CMSDataContext Db;

        public PersonController()
        {
            Db = DbUtil.Db;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<FamilyMember> FamilyMembers(int PersonId)
        {
            var p = DbUtil.Db.LoadPersonById(PersonId);
            var q = from m in p.Family.People
                    orderby 
                        m.PeopleId == m.Family.HeadOfHouseholdId ? 1 : 
                        m.PeopleId == m.Family.HeadOfHouseholdSpouseId ? 2 : 
                        3, m.Age descending, m.Name2
                    select new FamilyMember
                   {
                       Id = m.PeopleId,
                       Name = m.Name,
                       Age = m.Age,
                       Deceased = m.DeceasedDate != null,
                       PositionInFamily = m.FamilyPosition.Code
                   };
            return q;
        }

        public class OrganizationView
        {
            public string Name { get; set; }
            public int Id { get; set; }
            public string Location { get; set; }
            public string LeaderName { get; set; }
            public DateTime? MeetingTime { get; set; }
            public string Schedule { get { return "{0:dddd h:mm tt}".Fmt(MeetingTime); } }
            public int? LeaderId { get; set; }
            public string MemberType { get; set; }
            public DateTime? EnrollDate { get; set; }
            public DateTime? DropDate { get; set; }
            public string DivisionName { get; set; }
            public decimal? AttendPct { get; set; }
        }

        private int _EnrollDataCount;
        public int EnrollDataCount(int pid, string sortExpression, int maximumRows, int startRowIndex)
        {
            return _EnrollDataCount;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<OrganizationView> EnrollData(int pid, string sortExpression, int maximumRows, int startRowIndex)
        {
            var q = from om in Db.OrganizationMembers
                    where om.PeopleId == pid
                    where (om.Pending ?? false) == false
                    where !(om.Organization.SecurityTypeId == 3 && Util.OrgMembersOnly)
                    select om;
            _EnrollDataCount = q.Count();
            q = ApplyEnrollSort(q, sortExpression);

            var q2 = from om in q
                     let div = om.Organization.DivOrgs.FirstOrDefault(d => d.Division.Program.Name != DbUtil.MiscTagsString).Division
                     select new OrganizationView
                     {
                         Id = om.OrganizationId,
                         Name = om.Organization.OrganizationName,
                         Location = om.Organization.Location,
                         LeaderName = om.Organization.LeaderName,
                         MeetingTime = om.Organization.MeetingTime,
                         MemberType = om.MemberType.Description,
                         LeaderId = om.Organization.LeaderId,
                         EnrollDate = om.EnrollmentDate,
                         AttendPct = (om.AttendPct == null ? 0 : om.AttendPct.Value),
                         DivisionName = div.Program.Name + "/" + div.Name,
                     };
            return q2.Skip(startRowIndex).Take(maximumRows);
        }

        int _PreviousEnrollDataCount;
        public int PreviousEnrollDataCount(int pid, string sortExpression, int maximumRows, int startRowIndex)
        {
            return _PreviousEnrollDataCount;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<OrganizationView> PreviousEnrollData(int pid, string sortExpression, int maximumRows, int startRowIndex)
        {
            var q = from etd in Db.EnrollmentTransactions
                    where etd.TransactionStatus == false
                    where etd.PeopleId == pid
                    where etd.TransactionTypeId >= 4
                    where !(etd.Organization.SecurityTypeId == 3 && Util.OrgMembersOnly)
                    select etd;
            _PreviousEnrollDataCount = q.Count();
            q = ApplyEnrollSort(q, sortExpression);

            var q2 = from etd in q
                     let ete = Db.EnrollmentTransactions.SingleOrDefault(ete =>
                        ete.TransactionId == etd.EnrollmentTransactionId)
                     let div = etd.Organization.DivOrgs.First(t => t.Division.Program.Name != DbUtil.MiscTagsString).Division
                     select new OrganizationView
                    {
                        Id = etd.OrganizationId,
                        Name = etd.OrganizationName,
                        Location = etd.Organization.Location,
                        LeaderName = etd.Organization.LeaderName,
                        MeetingTime = etd.Organization.MeetingTime,
                        LeaderId = etd.Organization.LeaderId,
                        EnrollDate = ete.TransactionDate,
                        DropDate = etd.TransactionDate,
                        MemberType = etd.MemberType.Description,
                        DivisionName = div.Program.Name + "/" + div.Name,
                        AttendPct = etd.AttendancePercentage
                    };
            return q2.Skip(startRowIndex).Take(maximumRows);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<OrganizationView> PendingEnrollData(int pid)
        {
            var dt = Util.Now;
            var q = from o in Db.Organizations
                    from om in o.OrganizationMembers
                    where om.PeopleId == pid && om.Pending.Value == true
                    let l = Db.People.SingleOrDefault(p => p.PeopleId == o.LeaderId)
                    let div = om.Organization.DivOrgs.FirstOrDefault(d => d.Division.Program.Name != DbUtil.MiscTagsString).Division
                    orderby o.OrganizationName
                    select new OrganizationView
                    {
                        Id = o.OrganizationId,
                        Name = o.OrganizationName,
                        Location = o.Location,
                        LeaderName = l.Name,
                        MeetingTime = o.MeetingTime,
                        LeaderId = o.LeaderId,
                        EnrollDate = om.EnrollmentDate,
                        MemberType = om.MemberType.Description,
                        DivisionName = div.Program.Name + "/" + div.Name,
                    };
            return q;
        }
        private IQueryable<OrganizationMember> ApplyEnrollSort(IQueryable<OrganizationMember> q, string sortExpression)
        {
            switch (sortExpression)
            {
                case "Name":
                    q = q.OrderBy(om => om.Organization.OrganizationName);
                    break;
                case "EnrollDate":
                    q = q.OrderBy(om => om.EnrollmentDate);
                    break;
                case "Name DESC":
                    q = q.OrderByDescending(om => om.Organization.OrganizationName);
                    break;
                case "EnrollDate DESC":
                    q = q.OrderByDescending(om => om.EnrollmentDate);
                    break;
            }
            return q;
        }
        private IQueryable<EnrollmentTransaction> ApplyEnrollSort(IQueryable<EnrollmentTransaction> q, string sortExpression)
        {
            switch (sortExpression)
            {
                case "Name":
                    q = q.OrderBy(t => t.Organization.OrganizationName);
                    break;
                case "EnrollDate":
                    q = from t in q
                        let te = Db.EnrollmentTransactions.SingleOrDefault(te =>
                            te.TransactionId == t.EnrollmentTransactionId)
                        orderby te.TransactionDate
                        select t;
                    break;
                case "Name DESC":
                    q = q.OrderByDescending(t => t.Organization.OrganizationName);
                    break;
                default:
                case "EnrollDate DESC":
                    q = from t in q
                        let te = Db.EnrollmentTransactions.SingleOrDefault(te =>
                            te.TransactionId == t.EnrollmentTransactionId)
                        orderby te.TransactionDate descending
                        select t;
                    break;
            }
            return q;
        }
        public class AddressResult
        {
            public bool found { get; set; }
            public string selector { get; set; }
            public string address { get; set; }
            public string Line1 { get; set; }
            public string Line2 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Zip { get; set; }
        }
        public static string GetZip4(Person p)
        {
            var a = LookupAddress(p.PrimaryAddress, p.PrimaryAddress2, p.PrimaryCity, p.PrimaryState, p.PrimaryZip);
            if (a.found)
                return a.Zip;
            else
                return p.PrimaryZip;
        }
        public static string GetZip4(AddressResult a)
        {
            var a2 = LookupAddress(a.Line1, a.Line2, a.City, a.State, a.Zip);
            if (a2.found)
                return a2.Zip;
            else
                return a.Zip;
        }
        public static AddressResult LookupAddress(string line1, string line2, string city, string st, string zip)
        {
            string url = DbUtil.Settings("QAServer", "");
            if (!url.HasValue())
                return new AddressResult { found = false };

            var ws = new QuickAddress(url);
            ws.Engine = QuickAddress.EngineTypes.Verification;
            var address = new string[] { line1, line2, city, st, zip };

            var result = ws.Search("USA", address, PromptSet.Types.Default);
            if (result.VerifyLevel == SearchResult.VerificationLevels.PremisesPartial)
            {
                var a = result.Picklist.Items[0].PartialAddress;
            }
            else if (result.VerifyLevel == SearchResult.VerificationLevels.Verified
                || result.VerifyLevel == SearchResult.VerificationLevels.InteractionRequired)
            {
                var q = from ad in result.Address.AddressLines
                        where !string.IsNullOrEmpty(ad.Line)
                        select ad.Line;
                var sb = new StringBuilder();
                foreach (var s in q)
                {
                    if (sb.Length > 0)
                        sb.Append(";");
                    sb.Append(s);
                }
                var m = Regex.Match(sb.ToString(), @"(?<line1>[^;]*)(;(?<line2>[^;]*))*;(?<city>.*)\s(?<st>[^ ]+)\s+(?<zip>\d{5}(-\d{4})?)");
                var a = new AddressResult
                {
                    found = true,
                    Line1 = m.Groups["line1"].Value,
                    Line2 = m.Groups["line2"].Value,
                    City = m.Groups["city"].Value,
                    State = m.Groups["st"].Value,
                    Zip = m.Groups["zip"].Value
                };
                string lab = a.Line1;
                if (a.Line2.HasValue())
                    lab += "\n" + a.Line2;
                lab += "\n" + Util.FormatCSZ4(a.City, a.State, a.Zip);
                a.address = lab;

                return a;
            }
            return new AddressResult { found = false };
        }
    }
}
