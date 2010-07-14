/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using UtilityExtensions;
using System.Net;
using System.Linq;
using CmsData;

namespace CmsWeb
{
    public partial class MemberMap : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "image/png";
            var id = this.QueryString<int?>("id");
            string EmptyMap = "~/images/EmptyMap.png";
            var s = EmptyMap;
            if (id.HasValue)
            {
                var o = DbUtil.Db.LoadOrganizationById(id.Value);
                s = o.OrganizationMembers.GetMapWithPushPins();
            }
            try
            {
                var wc = new WebClient();
                var b = wc.DownloadData(s);
                Response.BinaryWrite(b);
            }
            catch
            {
                Page.Response.WriteFile(Server.MapPath(EmptyMap));
            }
        }
    }
}
