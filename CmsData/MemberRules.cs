using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilityExtensions;
using System.Text.RegularExpressions;
using System.Data.Linq;
using System.Xml.Linq;
using System.Data.Linq.SqlClient;

namespace CmsData
{
    public partial class Person
    {
        private int[] DiscClassStatuses = new int[] 
        { 
            (int)Person.DiscoveryClassStatusCode.AdminApproval, 
            (int)Person.DiscoveryClassStatusCode.Attended, 
            (int)Person.DiscoveryClassStatusCode.ExemptedChild 
        };
        public void MemberProfileAutomation(CMSDataContext Db)
        {
            if (DecisionTypeIdChanged)
                switch (DecisionTypeId ?? 0)
                {
                    case (int)Person.DecisionCode.ProfessionForMembership:
                        MemberStatusId = (int)Person.MemberStatusCode.Pending;
                        if (DiscoveryClassStatusId != (int)Person.DiscoveryClassStatusCode.Attended)
                            DiscoveryClassStatusId = (int)Person.DiscoveryClassStatusCode.Pending;
                        if (Age <= 12 && Family.People.Any(p =>
                                p.PositionInFamilyId == (int)Person.PositionInFamilyCode.Primary
                                && p.MemberStatusId == (int)Person.MemberStatusCode.Member
                                && SqlMethods.DateDiffMonth(p.JoinDate, Util.Now) >= 12))
                            BaptismTypeId = (int)Person.BaptismTypeCode.Biological;
                        else
                            BaptismTypeId = (int)Person.BaptismTypeCode.Original;
                        BaptismStatusId = (int)Person.BaptismStatusCode.NotScheduled;
                        break;
                    case (int)Person.DecisionCode.ProfessionNotForMembership:
                        MemberStatusId = (int)Person.MemberStatusCode.NotMember;
                        if (DiscoveryClassStatusId != (int)Person.DiscoveryClassStatusCode.Attended)
                            DiscoveryClassStatusId = (int)Person.DiscoveryClassStatusCode.NotSpecified;
                        if (BaptismStatusId != (int)Person.BaptismStatusCode.Completed)
                        {
                            BaptismTypeId = (int)Person.BaptismTypeCode.NonMember;
                            BaptismStatusId = (int)Person.BaptismStatusCode.NotScheduled;
                        }
                        break;
                    case (int)Person.DecisionCode.Letter:
                        MemberStatusId = (int)Person.MemberStatusCode.Pending;
                        if (DiscoveryClassStatusId != (int)Person.DiscoveryClassStatusCode.Attended)
                            DiscoveryClassStatusId = (int)Person.DiscoveryClassStatusCode.Pending;
                        if (BaptismStatusId != (int)Person.BaptismStatusCode.Completed)
                        {
                            BaptismTypeId = (int)Person.BaptismTypeCode.NotSpecified;
                            BaptismStatusId = (int)Person.BaptismStatusCode.NotSpecified;
                        }
                        break;
                    case (int)Person.DecisionCode.Statement:
                        MemberStatusId = (int)Person.MemberStatusCode.Pending;
                        if (DiscoveryClassStatusId != (int)Person.DiscoveryClassStatusCode.Attended)
                            DiscoveryClassStatusId = (int)Person.DiscoveryClassStatusCode.Pending;
                        if (BaptismStatusId != (int)Person.BaptismStatusCode.Completed)
                        {
                            BaptismTypeId = (int)Person.BaptismTypeCode.NotSpecified;
                            BaptismStatusId = (int)Person.BaptismStatusCode.NotSpecified;
                        }
                        break;
                    case (int)Person.DecisionCode.StatementReqBaptism:
                        MemberStatusId = (int)Person.MemberStatusCode.Pending;
                        if (DiscoveryClassStatusId != (int)Person.DiscoveryClassStatusCode.Attended)
                            DiscoveryClassStatusId = (int)Person.DiscoveryClassStatusCode.Pending;
                        if (BaptismStatusId != (int)Person.BaptismStatusCode.Completed)
                        {
                            BaptismTypeId = (int)Person.BaptismTypeCode.Required;
                            BaptismStatusId = (int)Person.BaptismStatusCode.NotScheduled;
                        }
                        break;
                    case (int)Person.DecisionCode.Cancelled:
                        MemberStatusId = (int)Person.MemberStatusCode.NotMember;
                        if (DiscoveryClassStatusId != (int)Person.DiscoveryClassStatusCode.Attended)
                            DiscoveryClassStatusId = (int)Person.DiscoveryClassStatusCode.NotSpecified;
                        if (BaptismStatusId != (int)Person.BaptismStatusCode.Completed)
                            if (BaptismStatusId != (int)Person.BaptismStatusCode.Completed)
                            {
                                BaptismTypeId = (int)Person.BaptismTypeCode.NotSpecified;
                                BaptismStatusId = (int)Person.BaptismStatusCode.Canceled;
                            }
                        EnvelopeOptionsId = (int)Person.EnvelopeOptionCode.None;
                        break;
                }
            // This section sets join codes
            if (DiscoveryClassStatusIdChanged || BaptismStatusIdChanged)
                switch (DecisionTypeId ?? 0)
                {
                    case (int)Person.DecisionCode.ProfessionForMembership:
                        if (DiscClassStatuses.Contains(DiscoveryClassStatusId ?? 0)
                            && BaptismStatusId == (int)Person.BaptismStatusCode.Completed)
                        {
                            MemberStatusId = (int)Person.MemberStatusCode.Member;
                            if (BaptismTypeId == (int)Person.BaptismTypeCode.Biological)
                                JoinCodeId = (int)Person.JoinTypeCode.BaptismBIO;
                            else
                                JoinCodeId = (int)Person.JoinTypeCode.BaptismPOF;
                            if (DiscoveryClassDate.HasValue && BaptismDate.HasValue)
                                JoinDate = DiscoveryClassDate.Value > BaptismDate.Value ?
                                    DiscoveryClassDate.Value : BaptismDate.Value;
                        }
                        break;
                    case (int)Person.DecisionCode.Letter:
                        if (DiscoveryClassStatusIdChanged)
                            if (DiscClassStatuses.Contains(DiscoveryClassStatusId ?? 0)
                                || DiscoveryClassStatusId == (int)Person.DiscoveryClassStatusCode.AdminApproval)
                            {
                                MemberStatusId = (int)Person.MemberStatusCode.Member;
                                JoinCodeId = (int)Person.JoinTypeCode.Letter;
                                JoinDate = DiscoveryClassDate.HasValue ? DiscoveryClassDate : DecisionDate;
                            }
                        break;
                    case (int)Person.DecisionCode.Statement:
                        if (DiscoveryClassStatusIdChanged)
                            if (DiscClassStatuses.Contains(DiscoveryClassStatusId ?? 0))
                            {
                                MemberStatusId = (int)Person.MemberStatusCode.Member;
                                JoinCodeId = (int)Person.JoinTypeCode.Statement;
                                JoinDate = DiscoveryClassDate.HasValue ? DiscoveryClassDate : DecisionDate;
                            }
                        break;
                    case (int)Person.DecisionCode.StatementReqBaptism:
                        if (DiscClassStatuses.Contains(DiscoveryClassStatusId ?? 0)
                            && BaptismStatusId == (int)Person.BaptismStatusCode.Completed)
                        {
                            MemberStatusId = (int)Person.MemberStatusCode.Member;
                            JoinCodeId = (int)Person.JoinTypeCode.BaptismSRB;
                            if (DiscoveryClassDate.HasValue)
                                JoinDate = DiscoveryClassDate.Value > BaptismDate.Value ?
                                    DiscoveryClassDate.Value : BaptismDate.Value;
                            else
                                JoinDate = BaptismDate;
                        }
                        break;
                }
            if (DeceasedDateChanged)
            {
                if (DeceasedDate.HasValue)
                    DeceasePerson(Db);
            }
            else if (DropCodeIdChanged)
            {
                switch (DropCodeId)
                {
                    case (int)Person.DropTypeCode.Administrative:
                        DropMembership(Db);
                        break;
                    case (int)Person.DropTypeCode.AnotherDenomination:
                        DropMembership(Db);
                        break;
                    case (int)Person.DropTypeCode.Duplicate:
                        DropMembership(Db);
                        MemberStatusId = (int)Person.MemberStatusCode.NotMember;
                        break;
                    case (int)Person.DropTypeCode.LetteredOut:
                        DropMembership(Db);
                        break;
                    case (int)Person.DropTypeCode.Other:
                        DropMembership(Db);
                        break;
                    case (int)Person.DropTypeCode.Requested:
                        DropMembership(Db);
                        break;
                }
            }
            if (DiscoveryClassStatusIdChanged
                && DiscoveryClassStatusId == (int)Person.DiscoveryClassStatusCode.Attended)
            {
                var q = from om in Db.OrganizationMembers
                        where om.PeopleId == PeopleId
                        where om.Organization.OrganizationName == "Step 1"
                        select om;
                foreach (var om in q)
                    om.Drop(Db);
            }
        }
        private void DropMembership(CMSDataContext Db)
        {
            dropMembership(false, Db);
        }
        private void DeceasePerson(CMSDataContext Db)
        {
            dropMembership(true, Db);
        }
        private void dropMembership(bool Deceased, CMSDataContext Db)
        {
            if (MemberStatusId == (int)Person.MemberStatusCode.Member)
            {
                if (Deceased)
                    DropCodeId = (int)Person.DropTypeCode.Deceased;
                MemberStatusId = (int)Person.MemberStatusCode.Previous;
                DropDate = Util.Now.Date;
            }
            if (Deceased)
            {
                EmailAddress = null;
                DoNotCallFlag = true;
                DoNotMailFlag = true;
                DoNotVisitFlag = true;
            }
            if (SpouseId.HasValue)
            {
                var spouse = Db.LoadPersonById(SpouseId.Value);
                if (Deceased)
                {
                    spouse.MaritalStatusId = (int)Person.MaritalStatusCode.Widowed;
                    if (spouse.EnvelopeOptionsId.HasValue)
                        if (spouse.EnvelopeOptionsId != (int)Person.EnvelopeOptionCode.None)
                            spouse.EnvelopeOptionsId = (int)Person.EnvelopeOptionCode.Individual;
                    spouse.ContributionOptionsId = (int)Person.EnvelopeOptionCode.Individual;
                }

                if (spouse.MemberStatusId == (int)Person.MemberStatusCode.Member)
                    if (spouse.EnvelopeOptionsId == (int)Person.EnvelopeOptionCode.Joint)
                        spouse.EnvelopeOptionsId = (int)Person.EnvelopeOptionCode.Individual;
            }
            EnvelopeOptionsId = (int)Person.EnvelopeOptionCode.None;

            var list = OrganizationMembers.ToList();
            foreach(var om in list)
                om.Drop(Db);
        }
    }
}
