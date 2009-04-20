/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Linq;
using CmsData;
using UtilityExtensions;
using System.IO;
using System.Drawing.Imaging;
using Drawing = System.Drawing;

namespace CMSWeb
{
    public partial class FormImage : System.Web.UI.Page
    {
        VolunteerForm form;

        protected void Page_Load(object sender, EventArgs e)
        {
            var id = this.QueryString<int>("id");
            form = DbUtil.Db.VolunteerForms.Single(f => f.Id == id);
            if (!IsPostBack)
                HiddenField1.Value = "large";
            HyperLink2.NavigateUrl = "~/AppReview/VolunteerApp.aspx?id=" + form.PeopleId;
            HyperLink2.Text = "Return to Volunteer Application Review for: " + form.Person.Name;
        }

        protected override void OnPreRender(EventArgs e)
        {
            const string getimage = "~/Image.ashx?id=";
            base.OnPreRender(e);
            switch (HiddenField1.Value)
            {
                case "medium":
                    ImageButton1.ImageUrl = getimage + form.MediumId;
                    break;
                case "large":
                    ImageButton1.ImageUrl = getimage + form.LargeId;
                    break;
            }
        }

        protected void ImageButton1_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            var s = HiddenField1.Value;
            HiddenField1.Value = s == "medium" ? "large" : "medium";
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            var pid = form.PeopleId;
            DbUtil.Db.VolunteerForms.DeleteOnSubmit(form);
            ImageData.Image.DeleteOnSubmit(form.SmallId);
            ImageData.Image.DeleteOnSubmit(form.MediumId);
            ImageData.Image.DeleteOnSubmit(form.LargeId);
            DbUtil.Db.SubmitChanges();
            ImageData.DbUtil.Db.SubmitChanges();
            Response.Redirect("~/AppReview/VolunteerApp.aspx?id=" + pid);
        }
    }
}
