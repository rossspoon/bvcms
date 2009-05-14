using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UtilityExtensions;
using System.Diagnostics;
using CmsData;

namespace CMSWeb.Admin
{
    public partial class VBSMed : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write("<html><body><pre>\n");
            var q = from v in DbUtil.Db.VBSApps
                    where v.IsDocument == true
                    select v;
            foreach (var v in q)
            {
                var i = ImageData.DbUtil.Db.Images.SingleOrDefault(im => im.Id == v.ImgId);
                if (i == null)
                    continue;
                Response.Write("{0,8}: {1,5}, {2,5}, {3}\n".Fmt(v.PeopleId, v.MedAllergy ?? false, i.HasMedical(), i.Medical()));
                v.MedAllergy = i.HasMedical();
            }
            Response.Write("</pre></body></html>\n");
        }
    }
}
