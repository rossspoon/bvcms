/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using UtilityExtensions;
using System.Text;
using CmsData;

namespace CMSWeb.Models
{
    public class CouponModel
    {
        public int? orgid { get; set; }
        public decimal amount {get; set;}

        public IEnumerable<Coupon> Coupons()
        {
            var q = from c in DbUtil.Db.Coupons
                    where c.OrgId == orgid
                    select c;
            return q;
        }
        public IEnumerable<SelectListItem> OnlineRegs()
        {
            var q = from o in DbUtil.Db.Organizations
                    where o.RegistrationTypeId > (int)CmsData.Organization.RegistrationEnum.None
                    where o.ClassFilled != true
                    orderby o.Division.Name, o.OrganizationName
                    select new SelectListItem
                    {
                        Text = o.Division.Name + ": " + o.OrganizationName,
                        Value = o.OrganizationId.ToString()
                    };
            return q;
        }
        public Coupon CreateCoupon()
        {
            var c = new Coupon
            {
                Id = Util.RandomPassword(12),
                OrgId = orgid,
                Created = DateTime.Now,
                Amount = amount,
            };
            DbUtil.Db.Coupons.InsertOnSubmit(c);
            DbUtil.Db.SubmitChanges();
            return c;
        }
    }
}
