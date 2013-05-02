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
using System.Web.UI;
using System.Web.UI.WebControls;
using CmsWeb.Models;
using UtilityExtensions;
using CmsData;

namespace CmsWeb.Contributions
{
    public partial class GLExtract : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var today = Util.Now.Date;
                FromDate.Text = today.ToString("d");
                ToDate.Text = today.ToString("d");
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;
            var dt1 = DateTime.Parse(FromDate.Text); // todo: tryparse here
            var dt2 = DateTime.Parse(ToDate.Text);
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "text/plain";
            Response.AddHeader("Content-Disposition", "attachment;filename=GLTRN2000.txt");
            var ctl = new BundleModel();
            var q = ctl.GetGLExtract(dt1, dt2);
            var GLBundlePrefix = DbUtil.Db.Setting("GLBundlePrefix", "CM");
            foreach (var i in q)
            {
                Response.Write(
                    "\"00000\",\"001{0}{1:00}{2}{3}\",\"000\",\"{4:MMddyy}\",\"{5}\",\"\",\"{6}0000{7}\",\"{8:00000000000}\",\"\"\r\n"
                    .Fmt(i.Fund, i.Month, GLBundlePrefix, i.HeaderId.PadLeft(5, '0'), i.ContributionDate, i.FundName, i.FundDept, i.FundAcct, i.Amount * 100));
            }
            Response.Flush();
            Response.Close();
        }
    }
}
