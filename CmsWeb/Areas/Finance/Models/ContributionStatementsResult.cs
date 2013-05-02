/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Web.Mvc;

namespace CmsWeb.Areas.Finance.Models.Report
{
    public class ContributionStatementsResult : ActionResult
    {
        public string outputfile { get; set; }

        public ContributionStatementsResult(string file)
        {
            outputfile = file;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var Response = context.HttpContext.Response;
            if (outputfile.EndsWith(".pdf"))
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=statements.pdf");
            }
            else
            {
                Response.ContentType = "text/plain";
                Response.AddHeader("content-disposition", "attachment;filename=statements.txt");
            }
            Response.TransmitFile(outputfile);
        }
    }
}

