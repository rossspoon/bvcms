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
using System.Collections.Generic;

namespace CMSWeb
{
    public partial class UploadPicture : System.Web.UI.Page
    {
        Person person;
        protected void Page_Load(object sender, EventArgs e)
        {
            var id = this.QueryString<int>("id");
            person = DbUtil.Db.People.Single(p => p.PeopleId == id);
            if (person.Picture == null)
                person.Picture = new Picture();
            if (!IsPostBack)
                HiddenField1.Value = "large";
            HyperLink2.NavigateUrl = "~/Person.aspx?id=" + id.ToString();
            HyperLink2.Text = "Return to: " + person.Name;
        }

        protected void Upload_Click(object sender, EventArgs e)
        {
            var Db = DbUtil.Db;
            DbUtil.LogActivity("Uploading Picture for {0}".Fmt(person.Name));
            var p = person.Picture;
            p.CreatedDate = Util.Now;
            p.CreatedBy = Util.UserName;
            var bits = new byte[ImageFile.PostedFile.ContentLength];
            ImageFile.PostedFile.InputStream.Read(bits, 0, bits.Length);
            p.SmallId = ImageData.Image.NewImageFromBits(bits, 120, 120).Id;
            p.MediumId = ImageData.Image.NewImageFromBits(bits, 320, 400).Id;
            p.LargeId = ImageData.Image.NewImageFromBits(bits, 570, 800).Id;
            Db.SubmitChanges();
        }
        protected override void OnPreRender(EventArgs e)
        {
            const string getimage = "~/Image.ashx?portrait=1&id=";
            base.OnPreRender(e);
            switch (HiddenField1.Value)
            {
                case "small":
                    ImageButton1.ImageUrl = getimage + person.Picture.SmallId;
                    break;
                case "medium":
                    ImageButton1.ImageUrl = getimage + person.Picture.MediumId;
                    break;
                case "large":
                    ImageButton1.ImageUrl = getimage + person.Picture.LargeId;
                    break;
            }
        }

        protected void ImageButton1_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            var s = HiddenField1.Value;
            HiddenField1.Value = s == "small" ? "medium" : s == "medium" ? "large" : "small";
        }
    }
}
