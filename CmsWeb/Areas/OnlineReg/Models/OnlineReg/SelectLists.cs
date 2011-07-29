using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using CmsData;
using System.Web.Mvc;
using System.Text;
using System.Configuration;
using UtilityExtensions;
using System.Data.Linq.SqlClient;
using CMSPresenter;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Collections;
using System.Runtime.Serialization;
using CmsData.Codes;

namespace CmsWeb.Models
{
    public partial class OnlineRegModel
    {
        public static IQueryable<Organization> UserSelectClasses(int? divid)
        {
            var a = new int[] 
            { 
                RegistrationEnum.UserSelectsOrganization,
                RegistrationEnum.ComputeOrganizationByAge,
                RegistrationEnum.ManageSubscriptions
            };
            var q = from o in DbUtil.Db.Organizations
                    where o.DivOrgs.Any(od => od.DivId == divid)
                    where o.OrganizationStatusId == OrgStatusCode.Active
                    where a.Contains(o.RegistrationTypeId.Value)
                    where o.OnLineCatalogSort != null || o.RegistrationTypeId == RegistrationEnum.ComputeOrganizationByAge
                    select o;
            return q;
        }

        public IEnumerable<SelectListItem> Classes()
        {
            return Classes(divid, classid ?? 0);
        }
        public static IEnumerable<SelectListItem> Classes(int? divid, int id)
        {
            var q = from o in UserSelectClasses(divid)
                    let hasroom = (o.ClassFilled ?? false) == false && ((o.Limit ?? 0) == 0 || o.Limit > o.MemberCount)
                    where hasroom
                    orderby o.OnLineCatalogSort, o.OrganizationName
                    select new SelectListItem
                    {
                        Value = o.OrganizationId.ToString(),
                        Text = ClassName(o),
                        Selected = o.OrganizationId == id,
                    };
            var list = q.ToList();
            if (list.Count == 1)
                return list;
            list.Insert(0, new SelectListItem { Text = "Registration Options", Value = "0" });
            return list;
        }
        public IEnumerable<String> FilledClasses()
        {
            var q = from o in UserSelectClasses(divid)
                    let hasroom = (o.ClassFilled ?? false) == false && ((o.Limit ?? 0) == 0 || o.Limit > o.MemberCount)
                    where !hasroom
                    orderby o.OnLineCatalogSort, o.OrganizationName
                    select ClassName(o);
            return q;
        }
        private static string ClassName(CmsData.Organization o)
        {
            var lead = o.LeaderName;
            if (lead.HasValue())
                lead = ": " + lead;
            var loc = o.Location;
            if (loc.HasValue())
                loc = " ({0})".Fmt(loc);
            var dt1 = o.FirstMeetingDate;
            var dt2 = o.LastMeetingDate;
            var dt = "";
            if (dt1.HasValue && dt2.HasValue)
                dt = ", {0:MMM d}-{1:MMM d}".Fmt(dt1, dt2);
            else if (dt1.HasValue)
                dt = ", {0:MMM d}".Fmt(dt1);

            return o.OrganizationName + lead + dt + loc;
        }


        public List<SelectListItem> ShirtSizes()
        {
            var q = from ss in DbUtil.Db.ShirtSizes
                    orderby ss.Id
                    select new SelectListItem
                    {
                        Value = ss.Code,
                        Text = ss.Description
                    };
            var list = q.ToList();
            list.Insert(0, new SelectListItem { Value = "0", Text = "(not specified)" });
            if (org != null && org.AllowLastYearShirt == true)
                list.Add(new SelectListItem { Value = "lastyear", Text = "Use shirt from last year" });
            return list;
        }
    }
}
