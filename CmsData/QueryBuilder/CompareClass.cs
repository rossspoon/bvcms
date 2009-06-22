/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using UtilityExtensions;
using System.Text;
using System.Xml.Linq;
using System.Linq.Expressions;
using System.Web;

namespace CmsData
{
    public class CompareClass
    {
        public FieldType FieldType { get; set; }
        public CompareType CompType { get; set; }
        public bool IsMultiple
        {
            get { return CompType == CompareType.OneOf || CompType == CompareType.NotOneOf; }
        }
        public string Display { get; set; }
        internal string ToString(QueryBuilderClause c)
        {
            string fld = c.FieldInfo.Display(c);
            switch (FieldType)
            {
                case FieldType.NullBit:
                case FieldType.Bit:
                case FieldType.Code:
                case FieldType.CodeStr:
                    return Display.Fmt(fld, c.CodeValues);
                case FieldType.String:
                case FieldType.StringEqual:
                case FieldType.Number:
                case FieldType.NullNumber:
                case FieldType.Integer:
                case FieldType.NullInteger:
                    return Display.Fmt(fld, c.TextValue);
                case FieldType.Date:
                    return Display.Fmt(fld, c.DateValue);
                case FieldType.DateField:
                    return Display.Fmt(fld, c.CodeIdValue);
                default:
                    throw new ArgumentException();
            }
        }
        internal Expression Expression(QueryBuilderClause c, ParameterExpression parm)
        {
            switch (c.FieldInfo.QueryType)
            {
                case QueryType.AttendPct:
                    return Expressions.AttendPct(parm,
                               c.DivOrg,
                               c.SubDivOrg,
                               c.Organization,
                               CompType,
                               decimal.Parse(c.TextValue));
                case QueryType.AttendPctHistory:
                    return Expressions.AttendPctHistory(parm, c.GetDataContext() as CMSDataContext,
                               c.DivOrg,
                               c.SubDivOrg,
                               c.Organization,
                               c.StartDate,
                               c.EndDate,
                               CompType,
                               decimal.Parse(c.TextValue));
                case QueryType.AttendCntHistory:
                    return Expressions.AttendCntHistory(parm, c.GetDataContext() as CMSDataContext,
                               c.DivOrg,
                               c.SubDivOrg,
                               c.Organization,
                               c.StartDate,
                               c.EndDate,
                               CompType,
                               c.TextValue.ToInt());
                case QueryType.AttendTypeAsOf:
                    return Expressions.AttendanceTypeAsOf(parm,
                               c.StartDate,
                               c.EndDate,
                               c.DivOrg,
                               c.SubDivOrg,
                               c.Organization,
                               CompType,
                               c.CodeIntIds);
                case QueryType.AttendMemberTypeAsOf:
                    return Expressions.AttendMemberTypeAsOf(parm, c.GetDataContext() as CMSDataContext,
                               c.StartDate,
                               c.EndDate,
                               c.DivOrg,
                               c.SubDivOrg,
                               c.Organization,
                               CompType,
                               string.Join(",",c.CodeStrIds));
                // B --------------------
                case QueryType.Birthday:
                    return Expressions.Birthday(parm, CompType, c.TextValue);
                case QueryType.BadET:
                    return Expressions.BadET(parm, CompType, c.CodeIntIds);
                // C ------------------------
                case QueryType.CreatedBy:
                    return Expressions.CreatedBy(parm, c.GetDataContext() as CMSDataContext, 
                        CompType, c.TextValue);
                // D --------------------
				case QueryType.DaysTillBirthday:
					return Expressions.DaysTillBirthday(parm, c.GetDataContext() as CMSDataContext,
							   CompType,
							   c.TextValue.ToInt());
				case QueryType.DaysSinceContact:
					return Expressions.DaysSinceContact(parm, 
							   CompType,
							   c.TextValue.ToInt());
				// F ----------------------------
                case QueryType.FamHasPrimAdultChurchMemb:
                    return Expressions.FamHasPrimAdultChurchMemb(parm, CompType, c.CodeIds == "1");
                case QueryType.FamilyHasChildren:
                    return Expressions.FamilyHasChildren(parm, CompType, c.CodeIds == "1");
                // H --------------------
                case QueryType.HasCurrentTag:
                    return Expressions.HasCurrentTag(parm,
                               CompType,
                               c.CodeIds == "1");
                case QueryType.HasMyTag:
                    return Expressions.HasMyTag(parm,
                               c.Tags,
                               CompType,
                               c.CodeIds == "1");
                case QueryType.HasLowerName:
                    return Expressions.HasLowerName(c.GetDataContext() as CMSDataContext,
                               parm,
                               CompType,
                               c.CodeIds == "1");
                case QueryType.HasPicture:
                    return Expressions.HasPicture(parm,
                               CompType,
                               c.CodeIds == "1");
                case QueryType.HasRelatedFamily:
                    return Expressions.HasRelatedFamily(parm,
                               CompType,
                               c.CodeIds == "1");
				case QueryType.HaveVolunteerApplications:
					return Expressions.HaveVolunteerApplications(parm,
							   CompType,
							   c.CodeIds == "1");
				case QueryType.HasContacts:
					return Expressions.HasContacts(parm,
							   CompType,
							   c.CodeIds == "1");
				// I -----------------
                case QueryType.IsCurrentPerson:
                    return Expressions.IsCurrentPerson(parm,
                               CompType,
                               c.CodeIds == "1");
                case QueryType.InCurrentOrg:
                    return Expressions.InCurrentOrg(parm,
                               CompType,
                               c.CodeIds == "1");
                case QueryType.InactiveCurrentOrg:
                    return Expressions.InactiveCurrentOrg(parm,
                               CompType,
                               c.CodeIds == "1");
                case QueryType.InOneOfMyOrgs:
                    return Expressions.InOneOfMyOrgs(parm, c.GetDataContext() as CMSDataContext,
                               CompType,
                               c.CodeIds == "1");
                case QueryType.IsUser:
                    return Expressions.IsUser(parm,
                               CompType,
                               c.CodeIds == "1");
                case QueryType.IsMemberOf:
                    return Expressions.IsMemberOf(parm,
                               c.DivOrg,
                               c.SubDivOrg,
                               c.Organization,
                               CompType,
                               c.CodeIds == "1");
                case QueryType.IsInactiveMemberOf:
                    return Expressions.IsInactiveMemberOf(parm,
                               c.DivOrg,
                               c.SubDivOrg,
                               c.Organization,
                               CompType,
                               c.CodeIds == "1");
                case QueryType.InBFClass:
                    return Expressions.InBFClass(parm,
                               CompType,
                               c.CodeIds == "1");
                case QueryType.IncludeDeceased:
                    c.SetIncludeDeceased();
                    return Expressions.IncludeDeceased(parm,
                               CompType,
                               c.CodeIds == "1");
                case QueryType.IsHeadOfHousehold:
                    return Expressions.IsHeadOfHousehold(parm,
                               CompType,
                               c.CodeIds == "1");
                // L -------------------
                // M -------------------
                case QueryType.MembOfOrgWithSched:
                    return Expressions.MembOfOrgWithSched(parm,
                               c.DivOrg,
                               c.SubDivOrg,
                               c.Organization,
                               CompType,
                               c.CodeIntIds);
                case QueryType.MemberTypeCodes:
                    return Expressions.MemberTypeIds(parm,
                               c.DivOrg,
                               c.SubDivOrg,
                               c.Organization,
                               c.Schedule,
                               CompType,
                               c.CodeIntIds);
                case QueryType.MemberTypeAsOf:
                    return Expressions.MemberTypeAsOf(parm,
                               c.StartDate,
                               c.EndDate,
                               c.DivOrg,
                               c.SubDivOrg,
                               c.Organization,
                               CompType,
                               c.CodeIntIds);
                // N -------------------
                case QueryType.NumberOfMemberships:
                    return Expressions.NumberOfMemberships(parm,
                               c.DivOrg,
                               c.SubDivOrg,
                               c.Organization,
                               c.Schedule,
                               CompType,
                               c.TextValue.ToInt());
                // O --------------------------
                case QueryType.OrgMemberCreatedDate:
                    return Expressions.OrgMemberCreatedDate(parm,
                               c.DivOrg,
                               c.SubDivOrg,
                               c.Organization,
                               CompType,
                               c.DateValue);
                case QueryType.OrgInactiveDate:
                    return Expressions.OrgInactiveDate(parm,
                               c.DivOrg,
                               c.SubDivOrg,
                               c.Organization,
                               CompType,
                               c.DateValue);
                case QueryType.OrgJoinDateCompare:
                    return Expressions.OrgJoinDateCompare(parm,
                               c.DivOrg,
                               c.SubDivOrg,
                               c.Organization,
                               CompType,
                               c.CodeIdValue);
                case QueryType.OrgJoinDateDaysAgo:
                    return Expressions.OrgJoinDateDaysAgo(parm,
                               c.DivOrg,
                               c.SubDivOrg,
                               c.Organization,
                               CompType,
                               c.TextValue.ToInt());
                case QueryType.OrgJoinDate:
                    return Expressions.OrgJoinDate(parm,
                               c.DivOrg,
                               c.SubDivOrg,
                               c.Organization,
                               CompType,
                               c.StartDate);
                // R ----------------
                case QueryType.RecentAttendType:
                    return Expressions.RecentAttendType(parm,
                               c.DivOrg,
                               c.SubDivOrg,
                               c.Organization,
                               c.Days,
                               CompType,
                               c.CodeIntIds);
                case QueryType.RecentAttendCount:
                    return Expressions.RecentAttendCount(parm,
                               c.DivOrg,
                               c.SubDivOrg,
                               c.Organization,
                               c.Days,
                               CompType,
                               c.TextValue.ToInt());
                case QueryType.RecentAttendMemberType:
                    return Expressions.RecentAttendMemberType(parm,
                               c.DivOrg,
                               c.SubDivOrg,
                               c.Organization,
                               c.Days,
                               CompType,
                               c.CodeIntIds);
                // S -------------------------
                case QueryType.SavedQuery:
                    return Expressions.SavedQuery(parm,
                               c.GetDataContext() as CMSDataContext,
                               c.SavedQueryIdDesc,
                               CompType,
                               c.CodeIds == "1");
                case QueryType.SmallGroup:
                    return Expressions.SmallGroup(parm, CompType, c.TextValue);

                // U ----------------------
                case QueryType.UserRole:
                    return Expressions.UserRole(parm,
                               CompType,
                               c.CodeIntIds);
                // V -------------------
				case QueryType.VBSActiveOtherChurch:
					return Expressions.VBSActiveOtherChurch(parm,
							   CompType,
							   c.CodeIds == "1");
                case QueryType.VBSPubPhoto:
                    return Expressions.VBSPubPhoto(parm,
                               CompType,
                               c.CodeIds == "1");
                case QueryType.VBSMedAllergy:
                    return Expressions.VBSMedAllergy(parm,
                               CompType,
                               c.CodeIds == "1");
                case QueryType.VisitedCurrentOrg:
                    return Expressions.VisitedCurrentOrg(parm,
                               CompType,
                               c.CodeIds == "1");
				case QueryType.VolunteerApprovalCode:
					return Expressions.VolunteerApprovalCode(parm,
							   CompType,
							   c.CodeIntIds);
				case QueryType.VolAppStatusCode:
					return Expressions.VolAppStatusCode(parm,
							   CompType,
							   c.CodeIntIds);
				case QueryType.VolunteerProcessedDateMonthsAgo:
                    return Expressions.VolunteerProcessedDateMonthsAgo(parm,
                        CompType,
                        int.Parse(c.TextValue));
                // W ----------------------
                case QueryType.WorksVolunteerWeek:
                    return Expressions.WorksVolunteerWeek(parm,
                               c.DivOrg,
                               c.SubDivOrg,
                               c.Organization,
                               CompType,
                               c.Quarters);
                case QueryType.WasMemberAsOf:
                    return Expressions.WasMemberAsOf(parm,
                               c.GetDataContext() as CMSDataContext,
                               c.StartDate,
                               c.EndDate,
                               c.DivOrg,
                               c.SubDivOrg,
                               c.Organization,
                               CompType,
                               c.CodeIds == "1");
                default:
                    if (CompType == CompareType.IsNull || CompType == CompareType.IsNotNull)
                        return Expressions.CompareConstant(parm,
                                   c.Field,
                                   CompType,
                                   null);
                    switch (FieldType)
                    {
                        case FieldType.NullBit:
                        case FieldType.Bit:
                            return Expressions.CompareConstant(parm,
                                       c.Field,
                                       CompType,
                                       c.CodeIds == "1");
                        case FieldType.Code:
                            return Expressions.CompareConstant(parm,
                                       c.Field,
                                       CompType,
                                       IsMultiple ? (object)c.CodeIntIds : (object)c.CodeIds.ToInt());
                        case FieldType.CodeStr:
                            return Expressions.CompareConstant(parm,
                                       c.Field,
                                       CompType,
                                       IsMultiple ? (object)c.CodeStrIds : (object)c.CodeIdValue);
                        case FieldType.String:
                            return Expressions.CompareConstant(parm,
                                       c.Field,
                                       CompType,
                                       c.TextValue);
                        case FieldType.Number:
                        case FieldType.NullNumber:
                            return Expressions.CompareConstant(parm,
                                       c.Field,
                                       CompType,
                                       decimal.Parse(c.TextValue));
                        case FieldType.Integer:
                        case FieldType.NullInteger:
                            return Expressions.CompareConstant(parm,
                                       c.Field,
                                       CompType,
                                       int.Parse(c.TextValue));
                        case FieldType.Date:
                            if (c.Field == "DeceasedDate")
                                c.SetIncludeDeceased();
                            return Expressions.CompareDateConstant(parm,
                                       c.Field,
                                       CompType,
                                       c.DateValue);
                        default:
                            throw new ArgumentException();
                    }
            }
        }
        public static CompareType Convert(string type)
        {
            return (CompareType)Enum.Parse(typeof(CompareType), type);
        }
        public static List<CompareClass> Comparisons
        {
            get
            {
                var _Comparisons = (List<CompareClass>)HttpRuntime.Cache["comparisons"];
                if (_Comparisons == null)
                {
                    var xdoc = XDocument.Parse(Properties.Resources.CompareMap);
                    var q = from f in xdoc.Descendants("FieldType")
                            from c in f.Elements("Comparison")
                            select new CompareClass
                            {
                                FieldType = FieldClass.Convert((string)f.Attribute("Name")),
                                CompType = Convert((string)c.Attribute("Type")),
                                Display = (string)c.Attribute("Display")
                            };
                    _Comparisons = q.ToList();
                    HttpRuntime.Cache["comparisons"] = _Comparisons;
                }
                return _Comparisons;
            }
        }
    }
}
