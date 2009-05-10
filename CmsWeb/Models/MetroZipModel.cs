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
using CmsData;
using System.Web.Mvc;

namespace CMSWeb.Models
{
    public class MetroZipModel
    {
        public string ZipCode
        {
            get
            {
                return zip.ZipCode;
            }
            set
            {
                zip = DbUtil.Db.Zips.SingleOrDefault(f => f.ZipCode == value);
            }
        }
        public Zip zip { get; set; }

        public IEnumerable<SelectListItem> MetroMarginalCodes()
        {
            var q = from c in DbUtil.Db.ResidentCodes
                    select new SelectListItem
                    {
                        Text = c.Description,
                        Value = c.Id.ToString(),
                    };
            return q;
        }
        public string InsertZip()
        {
            var f = new Zip { ZipCode = "new zip" };
            DbUtil.Db.Zips.InsertOnSubmit(f);
            DbUtil.Db.SubmitChanges();
            return f.ZipCode;
        }
        public void DeleteZip(string id)
        {
            var f = DbUtil.Db.Zips.SingleOrDefault(fu => fu.ZipCode == id);
            if (f != null)
                DbUtil.Db.Zips.DeleteOnSubmit(f);
            DbUtil.Db.SubmitChanges();
        }
    }
}
