/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using CmsData;
using System.Data.Linq;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using UtilityExtensions;
using System.Web;
using System.Configuration;
using System.Data.Linq.SqlClient;
using System.Web.Security;

namespace CMSPresenter
{
    [DataObject]
    public class CodeValueController
    {
        public CodeValueController()
        {
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<CodeValueItem> GetStateList()
        {
            const string NAME = "GetStateList";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                var q = from s in DbUtil.Db.StateLookups
                        orderby s.StateCode
                        select new CodeValueItem
                        {
                            Code = s.StateCode,
                            Value = s.StateCode + " - " + s.StateName
                        };
                list = q.ToList();
                list.Insert(0, new CodeValueItem { Code = "", Value = "(not specified)" });
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> LetterStatusCodes()
        {
            const string NAME = "LetterStatusCodes";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                var q = from ms in DbUtil.Db.MemberLetterStatuses
                        orderby ms.Description
                        select new CodeValueItem
                        {
                            Id = ms.Id,
                            Code = ms.Code,
                            Value = ms.Description
                        };
                list = q.ToList();
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> EnvelopeOptions()
        {
            const string NAME = "EnvelopeOptions";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                var q = from ms in DbUtil.Db.EnvelopeOptions
                        orderby ms.Description
                        select new CodeValueItem
                        {
                            Id = ms.Id,
                            Code = ms.Code,
                            Value = ms.Description
                        };
                list = q.ToList();
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> JoinTypes()
        {
            const string NAME = "JoinTypes";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                var q = from ms in DbUtil.Db.JoinTypes
                        orderby ms.Description
                        select new CodeValueItem
                        {
                            Id = ms.Id,
                            Code = ms.Code,
                            Value = ms.Description
                        };
                list = q.ToList();
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> TitleCodes()
        {
            const string NAME = "TitleCodes";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                var q = from ms in DbUtil.Db.NameTitles
                        orderby ms.Description
                        select new CodeValueItem
                        {
                            Id = ms.Id,
                            Code = ms.Code,
                            Value = ms.Description
                        };
                list = q.ToList();
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> VolApplicationStatusCodes()
        {
            const string NAME = "VolApplicationStatusCodes";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                var q = from sc in DbUtil.Db.VolApplicationStatuses
                        orderby sc.Description
                        select new CodeValueItem
                        {
                            Id = sc.Id,
                            Code = sc.Code,
                            Value = sc.Description
                        };
                list = q.ToList();
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> DropTypes()
        {
            const string NAME = "DropTypes";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                var q = from ms in DbUtil.Db.DropTypes
                        orderby ms.Description
                        select new CodeValueItem
                        {
                            Id = ms.Id,
                            Code = ms.Code,
                            Value = ms.Description
                        };
                list = q.ToList();
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> GenderCodes()
        {
            const string NAME = "GenderCodes";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                var q = from ms in DbUtil.Db.Genders
                        select new CodeValueItem
                        {
                            Id = ms.Id,
                            Code = ms.Code,
                            Value = ms.Description
                        };
                list = q.ToList();
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> BundleStatusTypes()
        {
            const string NAME = "BundleStatusTypes";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                var q = from ms in DbUtil.Db.BundleStatusTypes
                        select new CodeValueItem
                        {
                            Id = ms.Id,
                            Code = ms.Code,
                            Value = ms.Description
                        };
                list = q.ToList();
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> BundleHeaderTypes()
        {
            const string NAME = "BundleHeaderTypes";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                var q = from ms in DbUtil.Db.BundleHeaderTypes
                        select new CodeValueItem
                        {
                            Id = ms.Id,
                            Code = ms.Code,
                            Value = ms.Description
                        };
                list = q.ToList();
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> ContributionStatuses()
        {
            const string NAME = "ContributionStatuses";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                var q = from ms in DbUtil.Db.ContributionStatuses
                        select new CodeValueItem
                        {
                            Id = ms.Id,
                            Code = ms.Code,
                            Value = ms.Description
                        };
                list = q.ToList();
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> ContributionTypes()
        {
            const string NAME = "ContributionTypes";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                var q = from ms in DbUtil.Db.ContributionTypes
                        select new CodeValueItem
                        {
                            Id = ms.Id,
                            Code = ms.Code,
                            Value = ms.Description
                        };
                list = q.ToList();
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> Funds()
        {
            const string NAME = "Funds";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                var q = from f in DbUtil.Db.ContributionFunds
                        where f.FundStatusId == 1
                        orderby f.FundId
                        select new CodeValueItem
                        {
                            Id = f.FundId,
                            Value = f.FundName
                        };
                list = q.ToList();
                HttpRuntime.Cache[NAME] = list;
            }
            list.Insert(0, new CodeValueItem { Id = 0, Value = "(not specified)" });
            return list;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> GenderCodesWithUnspecified()
        {
            var u = new CodeValueItem { Id = 99, Code = "99", Value = "(not specified)" };
            var list = GenderCodes().ToList();
            list.Insert(0, u);
            return list;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> DiscoveryClassStatusCodes()
        {
            const string NAME = "DiscoveryClassStatusCodes";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                var q = from c in DbUtil.Db.DiscoveryClassStatuses
                        select new CodeValueItem
                        {
                            Id = c.Id,
                            Code = c.Code,
                            Value = c.Description
                        };
                list = q.ToList();
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> EntryPoints()
        {
            const string NAME = "EntryPoints";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                var q = from ms in DbUtil.Db.EntryPoints
                        select new CodeValueItem
                        {
                            Id = ms.Id,
                            Code = ms.Code,
                            Value = ms.Description
                        };
                list = q.ToList();
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> Origins()
        {
            const string NAME = "Origins";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                var q = from ms in DbUtil.Db.Origins
                        select new CodeValueItem
                        {
                            Id = ms.Id,
                            Code = ms.Code,
                            Value = ms.Description
                        };
                list = q.ToList();
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> InterestPoints()
        {
            const string NAME = "InterestPoints";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                var q = from ms in DbUtil.Db.InterestPoints
                        select new CodeValueItem
                        {
                            Id = ms.Id,
                            Code = ms.Code,
                            Value = ms.Description
                        };
                list = q.ToList();
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> BaptismTypes()
        {
            const string NAME = "BaptismTypes";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                var q = from ms in DbUtil.Db.BaptismTypes
                        select new CodeValueItem
                        {
                            Id = ms.Id,
                            Code = ms.Code,
                            Value = ms.Description
                        };
                list = q.ToList();
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> BaptismStatuses()
        {
            const string NAME = "BaptismStatuses";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                var q = from ms in DbUtil.Db.BaptismStatuses
                        select new CodeValueItem
                        {
                            Id = ms.Id,
                            Code = ms.Code,
                            Value = ms.Description
                        };
                list = q.ToList();
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> DecisionCodes()
        {
            const string NAME = "DecisionCodes";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                var q = from ms in DbUtil.Db.DecisionTypes
                        select new CodeValueItem
                        {
                            Id = ms.Id,
                            Code = ms.Code,
                            Value = ms.Description
                        };
                list = q.ToList();
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> FamilyPositionCodes()
        {
            const string NAME = "FamilyPositionCodes";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                var q = from ms in DbUtil.Db.FamilyPositions
                        select new CodeValueItem
                        {
                            Id = ms.Id,
                            Code = ms.Code,
                            Value = ms.Description
                        };
                list = q.ToList();
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> Ministries()
        {
            const string NAME = "Ministries";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                var q = from m in DbUtil.Db.Ministries
                        orderby m.MinistryName
                        select new CodeValueItem
                        {
                            Id = m.MinistryId,
                            Code = m.MinistryName,
                            Value = m.MinistryName
                        };
                list = q.ToList();
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> Ministries0()
        {
            return Ministries().AddNotSpecified();
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> ContactReasonCodes()
        {
            const string NAME = "ContactReasonCodes";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                var q = from c in DbUtil.Db.NewContactReasons
                        orderby c.Description
                        select new CodeValueItem
                        {
                            Id = c.Id,
                            Code = c.Code,
                            Value = c.Description
                        };
                list = q.ToList();
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> ContactReasonCodes0()
        {
            return ContactReasonCodes().AddNotSpecified();
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> ContactTypeCodes()
        {
            const string NAME = "ContactTypeCodes";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                var q = from c in DbUtil.Db.NewContactTypes
                        orderby c.Description
                        select new CodeValueItem
                        {
                            Id = c.Id,
                            Code = c.Code,
                            Value = c.Description
                        };
                list = q.ToList();
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> ContactTypeCodes0()
        {
            return ContactTypeCodes().AddNotSpecified();
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> UserTags(int? UserPeopleId)
        {
            var ownerstring = "";
            if (UserPeopleId == Util.UserPeopleId)
                DbUtil.Db.TagCurrent(); // make sure the current tag exists
            else
                ownerstring = UserPeopleId + ":";

            var q1 = from t in DbUtil.Db.Tags
                     where t.PeopleId == UserPeopleId
                     where t.TypeId == DbUtil.TagTypeId_Personal
                     orderby t.Name
                     select new CodeValueItem
                     {
                         Id = t.Id,
                         Code = t.Id + "," + ownerstring + t.Name,
                         Value = t.Name
                     };
            var q2 = from t in DbUtil.Db.Tags
                     where t.PeopleId != UserPeopleId
                     where t.TagShares.Any(ts => ts.PeopleId == UserPeopleId)
                     where t.TypeId == DbUtil.TagTypeId_Personal
                     orderby t.PersonOwner.Name2, t.Name
                     let op = DbUtil.Db.People.SingleOrDefault(p => p.PeopleId == t.PeopleId)
                     select new CodeValueItem
                     {
                         Id = t.Id,
                         Code = t.Id + "," + t.PeopleId + ":" + t.Name,
                         Value = op.Name + ":" + t.Name
                     };
            var list = q1.ToList();
            list.AddRange(q2);
            return list;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> UserTagsWithUnspecified()
        {
            var list = UserTags(Util.UserPeopleId).ToList();
            list.Insert(0, top[0]);
            return list;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> UsersToEmailFrom()
        {
            var user = DbUtil.Db.CurrentUser;
            var q = from u in user.UsersICanEmailFor
                    select new CodeValueItem
                    {
                        Id = u.Boss.UserId,
                        Code = u.Boss.Person.EmailAddress,
                        Value = u.Boss.Username
                    };
            var list = q.ToList();
            list.Insert(0, new CodeValueItem { Id = user.UserId, Code = user.Person.EmailAddress, Value = user.Name2 });
            return list;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> UserQueries()
        {
            string uname = Util.UserName;
            var q1 = from qb in DbUtil.Db.QueryBuilderClauses
                     where qb.SavedBy == uname
                     orderby qb.Description
                     select new CodeValueItem
                     {
                         Id = qb.QueryId,
                         Code = qb.QueryId.ToString(),
                         Value = qb.SavedBy + ":" + qb.Description
                     };
            var q2 = from qb in DbUtil.Db.QueryBuilderClauses
                     where qb.SavedBy != uname && qb.IsPublic
                     orderby qb.SavedBy, qb.Description
                     select new CodeValueItem
                     {
                         Id = qb.QueryId,
                         Code = qb.QueryId.ToString(),
                         Value = qb.SavedBy + ":" + qb.Description
                     };

            var list = q1.Union(q2).ToList();
            return list;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> MaritalStatusCodes()
        {
            const string NAME = "MaritalStatusCodes";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                var q = from ms in DbUtil.Db.MaritalStatuses
                        select new CodeValueItem
                        {
                            Id = ms.Id,
                            Code = ms.Code,
                            Value = ms.Description
                        };
                list = q.ToList();
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> MaritalStatusCodes99()
        {
            return MaritalStatusCodes().AddNotSpecified(99);
        }

        private static CodeValueItem[] top = 
		        { 
		            new CodeValueItem 
		            { 
		                Id = 0,
		                Value = "(not specified)",
		                Code = "0"
		            } 
		        };
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<CodeValueItem> QueryBuilderFields(string category)
        {
            int n = 1;
            return from f in FieldClass.Fields.Values
                   where f.CategoryTitle == category
                   select new CodeValueItem
                   {
                       Id = n++,
                       Value = f.Title,
                       Code = f.Name
                   };
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<string> QueryBuilderCategories()
        {
            return (from f in CategoryClass.Categories
                    select f.Title).ToList();
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> BitCodes()
        {
            const string NAME = "BitCodes";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                list = new List<CodeValueItem> 
                {
                    new CodeValueItem { Id = 1, Value = "True", Code = "T" },
                    new CodeValueItem { Id = 0, Value = "False", Code = "F" },
		        };
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }
        //[DataObjectMethod(DataObjectMethodType.Select, false)]
        //public List<CodeValueItem> MeetingStatusCodes()
        //{
        //    const string NAME = "MeetingStatusCodes";
        //    var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
        //    if (list == null)
        //    {
        //        var q = from ms in DbUtil.Db.MeetingStatuses
        //                select new CodeValueItem
        //                {
        //                    Id = ms.Id,
        //                    Code = ms.Code,
        //                    Value = ms.Description
        //                };
        //        list = q.ToList();
        //        HttpRuntime.Cache[NAME] = list;
        //    }
        //    return list;
        //}
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> DateFields()
        {
            const string NAME = "DateFields";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                list = new List<CodeValueItem> 
	            {
	                new CodeValueItem { Id =  1, Value = "Joined", Code = "JoinDate" },
	                new CodeValueItem { Id =  2, Value = "Dropped", Code = "DropDate" },
	                new CodeValueItem { Id =  3, Value = "Decision", Code = "DecisionDate" },
	                new CodeValueItem { Id =  4, Value = "Baptism", Code = "BaptismDate" },
	                new CodeValueItem { Id =  5, Value = "Wedding", Code = "WeddingDate" },
	                new CodeValueItem { Id =  6, Value = "Discovery Class", Code = "DiscoveryClassDate" },
	                new CodeValueItem { Id =  7, Value = "Letter Req'd", Code = "LetterDateRequested" },
	                new CodeValueItem { Id =  8, Value = "Letter Rec'd", Code = "LetterDateReceived" },
	                new CodeValueItem { Id =  9, Value = "Addr From", Code = "AddressFromDate" },
	                new CodeValueItem { Id = 10, Value = "Add To", Code = "AddressToDate" },
	                new CodeValueItem { Id = 11, Value = "Alt Addr From", Code = "AltAddressFromDate" },
	                new CodeValueItem { Id = 12, Value = "Alt Addr To", Code = "AltAddressToDate" },
	                new CodeValueItem { Id = 13, Value = "Deceased", Code = "DeceasedDate" },
	            };
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }


        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> AllCampuses()
        {
            const string NAME = "AllCampuses";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                var q = from c in DbUtil.Db.MainCampus
                        select new CodeValueItem
                        {
                            Id = c.Id,
                            Code = c.Id.ToString(),
                            Value = c.Campus,
                        };
                list = q.ToList();
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> AllCampuses0()
        {
            return AllCampuses().AddNotSpecified();
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> OrganizationStatusCodes()
        {
            const string NAME = "OrganizationStatusCodes";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                var q = from c in DbUtil.Db.OrganizationStatuses
                        select new CodeValueItem
                        {
                            Id = c.Id,
                            Code = c.Code,
                            Value = c.Description,
                        };
                list = q.ToList();
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> OrganizationStatusCodes0()
        {
            return OrganizationStatusCodes().AddNotSpecified();
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<CodeValueItem> ResidentCodesWithZero()
        {
            const string NAME = "ResidentCodesWithZero";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                list = ResidentCodes();
                list.Insert(0, top[0]);
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<CodeValueItem> ResidentCodes()
        {
            const string NAME = "ResidentCodes";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                var q = from c in DbUtil.Db.ResidentCodes
                        select new CodeValueItem
                        {
                            Id = c.Id,
                            Code = c.Code,
                            Value = c.Description,
                        };
                list = q.ToList();
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> OrganizationTypes()
        {
            const string NAME = "OrganizationTypes";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
                list = MeetingTypes();
            return list;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> MeetingTypes()
        {
            const string NAME = "MeetingTypes";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                var q = from c in DbUtil.Db.MeetingTypes
                        select new CodeValueItem
                        {
                            Id = c.Id,
                            Code = c.Code,
                            Value = c.Description,
                        };
                list = q.ToList();
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> GenderClasses()
        {
            const string NAME = "GenderClasses";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                var q = from c in DbUtil.Db.GenderClasses
                        select new CodeValueItem
                        {
                            Id = c.Id,
                            Code = c.Code,
                            Value = c.Description,
                        };
                list = q.ToList();
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> AttendanceTrackLevelCodes()
        {
            const string NAME = "AttendanceTrackLevelCodes";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                var q = from c in DbUtil.Db.AttendTrackLevels
                        select new CodeValueItem
                        {
                            Id = c.Id,
                            Code = c.Code,
                            Value = c.Description,
                        };
                list = q.ToList();
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> SecurityTypeCodes()
        {
            const string NAME = "SecurityTypeCodes";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                list = new List<CodeValueItem> 
		            {
		                new CodeValueItem { Id = 0, Value = "None", Code = "N" },
		                new CodeValueItem { Id = 1, Value = "Children", Code = "C" },
		                new CodeValueItem { Id = 2, Value = "Beeper", Code = "B" },
		                new CodeValueItem { Id = 3, Value = "UnShared", Code = "U" },
		            };
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> BadETCodes()
        {
            var list = new List<CodeValueItem> 
                {
                    new CodeValueItem { Id = 11, Value = "Enroll-Enroll", Code = "N" },
                    new CodeValueItem { Id = 55, Value = "Drop-Drop", Code = "C" },
                    new CodeValueItem { Id = 15, Value = "Same Time", Code = "C" },
                    new CodeValueItem { Id = 10, Value = "Missing Drop", Code = "B" },
                };
            return list;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> AttendanceClassifications()
        {
            const string NAME = "AttendanceClassifications";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                var q = from i in DbUtil.Db.AttendanceClassifications
                        orderby i.Id
                        select new CodeValueItem
                        {
                            Id = i.Id,
                            Code = i.Code,
                            Value = i.Description
                        };
                list = q.ToList();
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> MemberStatusCodes()
        {
            const string NAME = "MemberStatusCodes";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                var q = from ms in DbUtil.Db.MemberStatuses
                        select new CodeValueItem
                        {
                            Id = ms.Id,
                            Code = ms.Code,
                            Value = ms.Description
                        };
                list = q.ToList();
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> MemberStatusCodes0()
        {
            return MemberStatusCodes().AddNotSpecified();
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<CodeValueItem> Schedules()
        {
            const string NAME = "WeeklySchedules";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                var q = from c in DbUtil.Db.WeeklySchedules
                        select new CodeValueItem
                        {
                            Id = c.Id,
                            Code = c.Code,
                            Value = c.Description
                        };
                list = q.ToList();
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<CodeValueItem> Schedules0()
        {
            return Schedules().AddNotSpecified();
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<CodeValueItem> UserRoles()
        {
            var q = from s in DbUtil.Db.Roles
                   orderby s.RoleId
                   select new CodeValueItem
                   {
                       Id = s.RoleId,
                       Code = s.RoleName,
                       Value = s.RoleName,
                   };
            var list = q.ToList();
            list.Insert(0, new CodeValueItem { Code="(not specified)", Id=0});
            return list;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<CodeValueItem> MemberTypeCodesByFreq()
        {
            var q = from mt in DbUtil.Db.OrganizationMembers
                    group mt by mt.MemberTypeId into g
                    orderby g.Count()
                    select new { g.Key, count = g.Count() };

            var q2 = from mt in DbUtil.Db.MemberTypes
                     join g in q on mt.Id equals g.Key
                     orderby g.count descending
                     select new CodeValueItem
                     {
                         Id = mt.Id,
                         Code = mt.Code,
                         Value = mt.Description
                     };
            return q2;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<MemberTypeItem> MemberTypeCodes2()
        {
            const string NAME = "MemberTypeCodes";
            var list = HttpRuntime.Cache[NAME] as List<MemberTypeItem>;
            if (list == null)
            {
                var q = from mt in DbUtil.Db.MemberTypes
                        orderby mt.Description
                        select new MemberTypeItem
                        {
                            Id = mt.Id,
                            Code = mt.Code,
                            Value = mt.Description,
                            AttendanceTypeId = mt.AttendanceTypeId
                        };
                list = q.ToList();
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<MemberTypeItem> MemberTypeCodes0()
        {
            var list = MemberTypeCodes2().ToList();
            list.Insert(0, new MemberTypeItem { Id = 0, Value = "(not specified)" });
            return list;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static IEnumerable<CodeValueItem> MemberTypeCodes()
        {
            var list = MemberTypeCodes2();
            return list.Select(c => new CodeValueItem { Code = c.Code, Id = c.Id, Value = c.Value });
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> AttendanceTypeCodes()
        {
            const string NAME = "AttendanceTypeCodes";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                var q = from c in DbUtil.Db.AttendTypes
                        select new CodeValueItem
                        {
                            Id = c.Id,
                            Code = c.Code,
                            Value = c.Description,
                        };
                list = q.ToList();
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> AddressTypeCodes()
        {
            const string NAME = "AddressTypeCodes";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                var q = from at in DbUtil.Db.AddressTypes
                        select new CodeValueItem
                        {
                            Id = at.Id,
                            Code = at.Code,
                            Value = at.Description
                        };
                list = q.ToList();
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<CodeValueItem> Schools()
        {
            var q = from p in DbUtil.Db.People
                    group p by p.SchoolOther into g
                    orderby g.Key
                    select new CodeValueItem
                    {
                        Value = g.Key,
                        Code = g.Key,
                    };
            return q;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<CodeValueItem> Employers()
        {
            var q = from p in DbUtil.Db.People
                    group p by p.EmployerOther into g
                    orderby g.Key
                    select new CodeValueItem
                    {
                        Value = g.Key,
                    };
            return q;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<CodeValueItem> Occupations()
        {
            var q = from p in DbUtil.Db.People
                    group p by p.OccupationOther into g
                    orderby g.Key
                    select new CodeValueItem
                    {
                        Value = g.Key,
                    };
            return q;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> VolunteerCodes()
        {
            const string NAME = "VolunteerCodes";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                var q = from vc in DbUtil.Db.VolunteerCodes
                        select new CodeValueItem
                        {
                            Id = vc.Id,
                            Code = vc.Code,
                            Value = vc.Description
                        };
                list = q.ToList();
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> TaskStatusCodes()
        {
            const string NAME = "TaskStatusCodes";
            var list = HttpRuntime.Cache[NAME] as List<CodeValueItem>;
            if (list == null)
            {
                var q = from vc in DbUtil.Db.TaskStatuses
                        orderby vc.Description
                        select new CodeValueItem
                        {
                            Id = vc.Id,
                            Code = vc.Code,
                            Value = vc.Description
                        };
                list = q.ToList();
                HttpRuntime.Cache[NAME] = list;
            }
            return list;
        }

        //--------------------------------------------------
        //--------------Organizations---------------------

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<CodeValueItem> GetOrganizationList(int DivId)
        {
            var q = from ot in DbUtil.Db.DivOrgs
                    where ot.DivId == DivId
                    && (SqlMethods.DateDiffMonth(ot.Organization.OrganizationClosedDate, Util.Now) < 14
                        || ot.Organization.OrganizationStatusId == 30)
                    orderby ot.Organization.OrganizationStatusId, ot.Organization.OrganizationName
                    select new CodeValueItem
                    {
                        Id = ot.OrgId,
                        Value = Organization.FormatOrgName(ot.Organization.OrganizationName,
                           ot.Organization.LeaderName, ot.Organization.Location)
                    };
            return q;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<CodeValueItem> OrgDivTags()
        {
            return from t in DbUtil.Db.Programs
                   orderby t.Name
                   select new CodeValueItem
                   {
                       Id = t.Id,
                       Value = t.Name
                   };
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<CodeValueItem> OrgSubDivTags(int ProgId)
        {
            var q = from div in DbUtil.Db.Divisions
                    where div.ProgId == ProgId
                    orderby div.Name
                    select new CodeValueItem
                    {
                        Id = div.Id,
                        Value = div.Name
                    };
            return top.Union(q);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<string> OrgSubDivTags2(int ProgId)
        {
            var q = from program in DbUtil.Db.Programs
                    from div in program.Divisions
                    where (program.Id == ProgId || ProgId == 0)
                    orderby program.Name, div.Name
                    select (ProgId > 0 ? program.Name + "." : "") + div.Name;
            return q;
        }

        public class DropDownItem
        {
            public string Value { get; set; }
            public string Text { get; set; }
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<CodeValueItem> AllOrgDivTags()
        {
            var q = from program in DbUtil.Db.Programs
                    from div in program.Divisions
                    orderby program.Name, div.Name
                    select new CodeValueItem
                    {
                        Id = div.Id,
                        Value = "{0}: {1}".Fmt(program.Name, div.Name)
                    };
            return top.Union(q);
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<DropDownItem> AllOrgDivTags2()
        {
            var q = from program in DbUtil.Db.Programs
                    from div in program.Divisions
                    orderby program.Name, div.Name
                    select new DropDownItem
                    {
                        Value = "{0}:{1}".Fmt(program.Id, div.Id),
                        Text = "{0}: {1}".Fmt(program.Name, div.Name)
                    };
            return (new[] 
		        { 
		            new DropDownItem 
		            { 
		                Text = "(not specified)",
		                Value = "0"
		            } 
		        }).Union(q);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<CodeValueItem> Organizations(int SubDivId)
        {
            return top.Union(GetOrganizationList(SubDivId));
        }
    }
    public static class CodeValue
    {
        public static List<CodeValueItem> AddNotSpecified(this IEnumerable<CodeValueItem> q)
        {
            return q.AddNotSpecified(0);
        }
        public static List<CodeValueItem> AddNotSpecified(this IEnumerable<CodeValueItem> q, int value)
        {
            var list = q.ToList();
            list.Insert(0, new CodeValueItem { Id = value, Value = "(not specified)" });
            return list;
        }
    }
}
