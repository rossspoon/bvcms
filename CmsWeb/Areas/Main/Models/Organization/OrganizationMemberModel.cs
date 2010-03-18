using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using UtilityExtensions;

namespace CMSWeb.Models.OrganizationPage
{
    public class OrganizationMemberModel
    {
        public enum GroupSelect
        {
            Active,
            Inactive,
            Pending,
            Previous
        }
        public int OrganizationId { get; set; }
        private int GroupId;
        private GroupSelect Select;

        public PagerModel2 Pager { get; set; }
        public OrganizationMemberModel(int id, int groupid, GroupSelect select)
        {
            OrganizationId = id;
            GroupId = groupid;
            Select = select;
            Pager = new PagerModel2(Count);
        }
        private IQueryable<OrganizationMember> _members;
        private IQueryable<OrganizationMember> FetchMembers()
        {
            int inactive = (int)OrganizationMember.MemberTypeCode.InActive;
            bool Active = true;
            bool Pending = false;
            switch (Select)
            {
                case GroupSelect.Inactive:
                    Active = false;
                    Pending = false;
                    break;
                case GroupSelect.Pending:
                    Active = true;
                    Pending = true;
                    break;
                //case GroupSelect.Previous:
                //    return PrevOrgMembers(OrganizationId, sortExpression, maximumRows, startRowIndex);
            }

            if (_members == null)
                _members = from om in DbUtil.Db.OrganizationMembers
                           where om.OrganizationId == OrganizationId
                           where om.OrgMemMemTags.Any(mt => mt.MemberTagId == GroupId) || GroupId <= 0
                           where om.OrgMemMemTags.Count() == 0 || GroupId != -1
                           where (Active && om.MemberTypeId != inactive)
                               || (!Active && om.MemberTypeId == inactive)
                           where (Pending && om.Pending == true)
                               || (!Pending && (om.Pending ?? false) == false)
                           select om;
            return _members;
        }
        int? _count;
        public int Count()
        {
            if (!_count.HasValue)
                _count = FetchMembers().Count();
            return _count.Value;
        }
        public IEnumerable<PersonMemberInfo> Members()
        {
            var q0 = ApplySort();
            q0 = q0.Skip(Pager.StartRow).Take(Pager.PageSize);
            var tagownerid = Util.CurrentTagOwnerId;
            var q = from om in q0
                    let p = om.Person
                    select new PersonMemberInfo
                    {
                        PeopleId = p.PeopleId,
                        Name = p.Name,
                        Name2 = p.Name2,
                        BirthDate = Util.FormatBirthday(
                            p.BirthYear,
                            p.BirthMonth,
                            p.BirthDay),
                        Address = p.PrimaryAddress,
                        Address2 = p.PrimaryAddress2,
                        CityStateZip = Util.FormatCSZ(p.PrimaryCity, p.PrimaryState, p.PrimaryZip),
                        EmailAddress = p.EmailAddress,
                        PhonePref = p.PhonePrefId,
                        HomePhone = p.HomePhone,
                        CellPhone = p.CellPhone,
                        WorkPhone = p.WorkPhone,
                        MemberStatus = p.MemberStatus.Description,
                        Email = p.EmailAddress,
                        BFTeacher = p.BFClass.LeaderName,
                        BFTeacherId = p.BFClass.LeaderId,
                        Age = p.Age.ToString(),
                        MemberTypeCode = om.MemberType.Code,
                        MemberType = om.MemberType.Description,
                        MemberTypeId = om.MemberTypeId,
                        InactiveDate = om.InactiveDate,
                        AttendPct = om.AttendPct,
                        LastAttended = om.LastAttended,
                        HasTag = p.Tags.Any(t => t.Tag.Name == Util.CurrentTagName && t.Tag.PeopleId == tagownerid),
                        //FromTab = fromtab,
                        Joined = om.EnrollmentDate,
                    };
            return q;
        }
        public IQueryable<OrganizationMember> ApplySort()
        {
            var q = FetchMembers();
            if (Pager.Direction == "asc")
                switch (Pager.Sort)
                {
                    case "Name":
                        q = from om in q
                            let p = om.Person
                            orderby p.LastName,
                            p.FirstName,
                            p.PeopleId
                            select om;
                        break;
                    case "MemberStatus":
                        q = from om in q
                            let p = om.Person
                            orderby p.MemberStatus.Code,
                            p.LastName,
                            p.FirstName,
                            p.PeopleId
                            select om;
                        break;
                    case "MemberType":
                        q = from om in q
                            let p = om.Person
                            orderby om.MemberType.Code,
                            p.LastName,
                            p.FirstName,
                            p.PeopleId
                            select om;
                        break;
                    case "Address":
                        q = from om in q
                            let p = om.Person
                            orderby p.Family.StateCode,
                            p.Family.CityName,
                            p.Family.AddressLineOne,
                            p.PeopleId
                            select om;
                        break;
                    case "BFTeacher":
                        q = from om in q
                            let p = om.Person
                            orderby p.BFClass.LeaderName,
                            p.LastName,
                            p.FirstName,
                            p.PeopleId
                            select om;
                        break;
                    case "AttendPct":
                        q = from om in q
                            orderby om.AttendPct
                            select om;
                        break;
                    case "Age":
                        q = from om in q
                            let p = om.Person
                            orderby p.BirthYear, p.BirthMonth, p.BirthDay
                            select om;
                        break;
                    case "DOB":
                        q = from om in q
                            let p = om.Person
                            orderby p.BirthMonth, p.BirthDay,
                            p.LastName, p.FirstName
                            select om;
                        break;
                    case "LastAttended":
                        q = from om in q
                            let p = om.Person
                            orderby om.LastAttended, p.LastName, p.FirstName
                            select om;
                        break;
                }
            else
                switch (Pager.Sort)
                {
                    case "MemberStatus":
                        q = from om in q
                            let p = om.Person
                            orderby p.MemberStatus.Code descending,
                            p.LastName descending,
                            p.FirstName descending,
                            p.PeopleId descending
                            select om;
                        break;
                    case "MemberType":
                        q = from om in q
                            let p = om.Person
                            orderby om.MemberType.Code descending,
                            p.LastName descending,
                            p.FirstName descending,
                            p.PeopleId descending
                            select om;
                        break;
                    case "Address":
                        q = from om in q
                            let p = om.Person
                            orderby p.Family.StateCode descending,
                                   p.Family.CityName descending,
                                   p.Family.AddressLineOne descending,
                                   p.PeopleId descending
                            select om;
                        break;
                    case "BFTeacher":
                        q = from om in q
                            let p = om.Person
                            orderby p.BFClass.LeaderName descending,
                            p.LastName descending,
                            p.FirstName descending,
                            p.PeopleId descending
                            select om;
                        break;
                    case "AttendPct":
                        q = from om in q
                            orderby om.AttendPct descending
                            select om;
                        break;
                    case "Name":
                        q = from om in q
                            let p = om.Person
                            orderby p.LastName descending,
                            p.LastName descending,
                            p.PeopleId descending
                            select om;
                        break;
                    case "DOB":
                        q = from om in q
                            let p = om.Person
                            orderby p.BirthMonth descending, p.BirthDay descending,
                            p.LastName descending, p.FirstName descending
                            select om;
                        break;
                    case "LastAttended":
                        q = from om in q
                            let p = om.Person
                            orderby om.LastAttended descending, p.LastName descending, p.FirstName descending
                            select om;
                        break;
                    case "Age":
                        q = from om in q
                            let p = om.Person
                            orderby p.BirthYear descending, p.BirthMonth descending, p.BirthDay descending
                            select om;
                        break;
                }
            return q;
        }

        public class PersonMemberInfo
        {
            public int PeopleId { get; set; }
            public string Name { get; set; }
            public string Name2 { get; set; }
            public string BirthDate { get; set; }
            public string Address { get; set; }
            public string Address2 { get; set; }
            public string CityStateZip { get; set; }
            public string EmailAddress { get; set; }
            private int _PhonePref;
            public int PhonePref { set { _PhonePref = value; } }
            private string _HomePhone;
            public string HomePhone
            {
                set
                {
                    if (value.HasValue())
                    {
                        _HomePhone = PhoneFmt(string.Empty, PhoneType.Home, value);
                        _Phones.Add(PhoneFmt("H", PhoneType.Home, value));
                    }
                }
                get { return _HomePhone; }
            }
            private string _CellPhone;
            public string CellPhone
            {
                set
                {
                    if (value.HasValue())
                    {
                        _CellPhone = PhoneFmt(string.Empty, PhoneType.Cell, value);
                        _Phones.Add(PhoneFmt("C", PhoneType.Cell, value));
                    }
                }
                get { return _CellPhone; }
            }

            private string _WorkPhone;
            public string WorkPhone
            {
                set
                {
                    if (value.HasValue())
                    {
                        _WorkPhone = PhoneFmt(string.Empty, PhoneType.Work, value);
                        _Phones.Add(PhoneFmt("W", PhoneType.Work, value));
                    }
                }
                get { return _WorkPhone; }
            }
            public string MemberStatus { get; set; }
            public string Email { get; set; }
            public string BFTeacher { get; set; }
            public int? BFTeacherId { get; set; }
            public string Age { get; set; }
            public string MemberTypeCode { get; set; }
            public string MemberType { get; set; }
            public int MemberTypeId { get; set; }
            public DateTime? InactiveDate { get; set; }
            public decimal? AttendPct { get; set; }
            public DateTime? LastAttended { get; set; }
            public bool HasTag { get; set; }
            public GroupSelect FromTab { get; set; }
            public DateTime? Joined { get; set; }
            private enum PhoneType
            {
                Home, Cell, Work
            }
            private string PhoneFmt(string prefix, PhoneType type, string number)
            {
                var s = number.FmtFone(type + " ");
                if ((type == PhoneType.Home && _PhonePref == 10)
                    || (type == PhoneType.Cell && _PhonePref == 20)
                    || (type == PhoneType.Work && _PhonePref == 30))
                    return number.FmtFone("*" + prefix + " ");
                return number.FmtFone(prefix + " ");
            }
            private List<string> _Phones = new List<string>();
            public List<string> Phones
            {
                get { return _Phones; }
            }
        }
    }
}
