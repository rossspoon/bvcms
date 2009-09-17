/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Linq;
using System.Data.Linq;
using UtilityExtensions;
using CmsData;
using System.Web.Mvc;
using System.Collections.Generic;
using CMSPresenter;

namespace CMSWeb.Models
{
    public class RecDetailModel
    {
        internal RecReg recreg;
        public RecDetailModel(int id)
        {
            recreg = DbUtil.Db.RecRegs.SingleOrDefault(v => v.Id == id);
        }
        public int Id
        {
            get { return recreg.Id; }
        }
        public string Name
        {
            get
            {
                if (recreg.Person != null)
                    return recreg.Person.Name;
                return "not assigned";
            }
        }
        public int? PeopleId
        {
            get { return recreg.PeopleId; }
        }
        public string ShirtSize
        {
            get { return recreg.ShirtSize; }
            set { recreg.ShirtSize = value; }
        }
        public int? League
        {
            get
            {
                return recreg.DivId ?? 0;
            }
            set
            {
                recreg.DivId = value;
            }
        }
        public bool FeePaid
        {
            get { return recreg.FeePaid ?? false; }
            set { recreg.FeePaid = value; }
        }
        public string Request
        {
            get { return recreg.Request; }
            set { recreg.Request = value; }
        }
        public bool ActiveInAnotherChurch
        {
            get { return recreg.ActiveInAnotherChurch ?? false; }
            set { recreg.ActiveInAnotherChurch = value; }
        }
        public bool MedAllergy
        {
            get { return recreg.MedAllergy ?? false; }
            set { recreg.MedAllergy = value; }
        }
        public bool IsDocument
        {
            get { return recreg.IsDocument ?? false; }
        }
        public int ImgId
        {
            get { return recreg.ImgId ?? 0; }
        }
        public string Email
        {
            get { return recreg.Email; }
        }
        public string TransactionID
        {
            get { return recreg.TransactionId; }
        }
        internal RecAgeDivision GetRecAgeDivision(int divid)
        {
            if (!recreg.PeopleId.HasValue)
                return null;
            var q = from r in DbUtil.Db.RecAgeDivisions
                    where r.DivId == divid
                    where r.GenderId == recreg.Person.GenderId || r.GenderId == 0
                    select r;
            var list = q.ToList();
            var bd0 = recreg.Person.GetBirthdate();
            var bd = bd0.HasValue ? bd0.Value : DateTime.MinValue;
            var q2 = from r in list
                     let age = bd.AgeAsOf(r.agedate)
                     where age >= r.StartAge && age <= r.EndAge
                     select r;
            return q2.SingleOrDefault();
        }
        public string AgeDiv
        {
            get
            {
                if (RecAgeDiv == null)
                    return "";
                return RecAgeDiv.Organization.OrganizationName;
            }
        }
        public int? AgeDivId { get; set; }
        internal void AssignPerson(int pid)
        {
            if (PeopleId != pid)
            {
                if (PeopleId.HasValue && AgeDivId.HasValue)
                {
                    var q = from om in DbUtil.Db.OrganizationMembers
                            where om.PeopleId == PeopleId && om.OrganizationId == AgeDivId
                            select om;
                    var member = q.SingleOrDefault();
                    if (member != null)
                        member.Drop();
                }
                recreg.PeopleId = pid;
                DbUtil.Db.SubmitChanges();
                OrganizationMember.UpdateMeetingsToUpdate();
            }
        }
        public IEnumerable<SelectListItem> Leagues()
        {
            var q = from d in DbUtil.Db.Divisions
                    where d.RecAgeDivisions.Count() > 0
                    orderby d.Name
                    select new SelectListItem
                    {
                        Text = d.Name,
                        Value = d.Id.ToString(),
                    };
            var list = q.ToList();
            list.Insert(0, new SelectListItem { Text = "(Select League)", Value = "0", Selected = true });
            return list;
        }
        private RecAgeDivision _RecAgeDiv;
        public RecAgeDivision RecAgeDiv
        {
            get
            {
                if (_RecAgeDiv == null)
                    _RecAgeDiv = GetRecAgeDivision(League.Value);
                return _RecAgeDiv;
            }
        }
        internal bool EnrollInOrg()
        {
            if (RecAgeDiv == null || !recreg.PeopleId.HasValue)
                return false;
            var oid = RecAgeDiv.OrgId;
            OrgId = oid;
            recreg.OrgId = oid;
            DbUtil.Db.SubmitChanges();
            var member = DbUtil.Db.OrganizationMembers.SingleOrDefault(om =>
                om.OrganizationId == OrgId && om.PeopleId == recreg.PeopleId);
            if (member == null)
                OrganizationController.InsertOrgMembers(
                    OrgId.Value,
                    recreg.PeopleId.Value,
                    (int)OrganizationMember.MemberTypeCode.Member,
                    DateTime.Today, null, false);
            return true;
        }
        public CmsData.Organization organization { get; set; }
        public int? OrgId
        {
            get
            {
                return organization.OrganizationId;
            }
            set
            {
                organization = DbUtil.Db.Organizations.Single(o => o.OrganizationId == value);
            }
        }

    }
}
