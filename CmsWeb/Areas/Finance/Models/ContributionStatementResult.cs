/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Collections;
using CmsData;
using UtilityExtensions;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Diagnostics;
using iTextSharp.text.html.simpleparser;
using CmsWeb.Areas.Main.Models.Report;

namespace CmsWeb.Areas.Finance.Models.Report
{
    public class ContributionStatementResult : ActionResult
    {
        public int FamilyId { get; set; }
        public int PeopleId { get; set; }
        public int? SpouseId { get; set; }
        public int typ { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            var Response = context.HttpContext.Response;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "filename=foo.pdf");
            var c = new ContributionStatements
            {
                FamilyId = FamilyId,
                FromDate = FromDate,
                PeopleId = PeopleId,
                SpouseId = SpouseId,
                ToDate = ToDate,
                typ = typ
            };
            IEnumerable<ContributorInfo> q = null;
            var noaddressok = DbUtil.Db.Setting("RequireAddressOnStatement", "true") == "false";
            switch (typ)
            {
                case 1:
                    SpouseId = DbUtil.Db.People.Where(p => p.PeopleId == PeopleId).Single().SpouseId.ToInt();
                    q = ContributionModel.contributors(DbUtil.Db, FromDate, ToDate, PeopleId, SpouseId, 0, noaddressok, useMinAmt: true);
                    break;
                case 2:
                    FamilyId = DbUtil.Db.People.Where(p => p.PeopleId == PeopleId).Single().FamilyId;
                    q = ContributionModel.contributors(DbUtil.Db, FromDate, ToDate, 0, 0, FamilyId, noaddressok, useMinAmt: true);
                    break;
                case 3:
                    q = ContributionModel.contributors(DbUtil.Db, FromDate, ToDate, 0, 0, 0, noaddressok, useMinAmt: true);
                    break;
            }
            c.Run(Response.OutputStream, DbUtil.Db, q);
        }
    }
}

