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
using CMSPresenter;
using System.Web.UI.WebControls;

namespace CmsWeb
{
    public partial class VolunteerApp : System.Web.UI.Page
    {
        public Volunteer vol;

        protected void Page_Load(object sender, EventArgs e)
        {
            var id = this.QueryString<int>("id");
            vol = DbUtil.Db.Volunteers.SingleOrDefault(v => v.PeopleId == id);
            if (!Page.IsPostBack)
            {
                if (vol == null)
                {
                    vol = new Volunteer { PeopleId = id };
                    DbUtil.Db.Volunteers.InsertOnSubmit(vol);
                    DbUtil.Db.SubmitChanges();
                    DbUtil.LogActivity("Viewing VolunteerApp for {0}".Fmt(vol.Person.Name));
                }
            }
            EditUpdateButton1.DataBind();
        }

        protected void Upload_Click(object sender, EventArgs e)
        {
            var f = new VolunteerForm { UploaderId = Util.UserId1, PeopleId = vol.PeopleId };
            DbUtil.Db.VolunteerForms.InsertOnSubmit(f);
            f.AppDate = Util.Now;
            f.UploaderId = Util.UserId1;
            var bits = new byte[ImageFile.PostedFile.ContentLength];
            ImageFile.PostedFile.InputStream.Read(bits, 0, bits.Length);
            var mimetype = ImageFile.PostedFile.ContentType.ToLower();
            switch (mimetype)
            {
                case "image/jpeg":
                case "image/pjpeg":
                case "image/gif":
                    f.IsDocument = false;
                    try
                    {
                        f.SmallId = ImageData.Image.NewImageFromBits(bits, 165, 220).Id;
                        f.MediumId = ImageData.Image.NewImageFromBits(bits, 675, 900).Id;
                        f.LargeId = ImageData.Image.NewImageFromBits(bits).Id;
                    }
                    catch
                    {
                        CheckImage.IsValid = false;
                        return;
                    }
                    break;
                case "text/plain":
                case "application/pdf":
                case "application/msword":
                case "application/vnd.ms-excel":
                    f.MediumId = ImageData.Image.NewImageFromBits(bits, mimetype).Id;
                    f.SmallId = f.MediumId;
                    f.LargeId = f.MediumId;
                    f.IsDocument = true;
                    break;
                default:
                    CheckImage.IsValid = false;
                    return;
            }
            DbUtil.Db.SubmitChanges();
            DbUtil.LogActivity("Uploading VolunteerApp for {0}".Fmt(vol.Person.Name));
            DataList1.DataBind();
        }

        protected void EditUpdateButton1_Click(object sender, EventArgs e)
        {
            if (EditUpdateButton1.Updating)
            {
                DbUtil.Db.SubmitChanges();
                DbUtil.LogActivity("Updating VolunteerApp for {0}".Fmt(vol.Person.Name));
                EditUpdateButton1.DataBind();
            }
        }

        protected void DataList1_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item)
            {
                var link = e.Item.FindControl("HyperLink1") as HyperLink;
                var d = e.Item.DataItem as VolunteerAppController.AppInfo;
                if (d.IsDocument.HasValue && d.IsDocument.Value)
                {
                    link.NavigateUrl = "~/Image.aspx?id={0}".Fmt(d.Docid);
                    link.ImageUrl = "~/images/adobe.png";
                    link.Target = "docpage";
                }
                else
                {
                    link.NavigateUrl = "~/AppReview/FormImage.aspx?id={0}".Fmt(d.Id);
                    link.ImageUrl = "~/Image.aspx?id={0}".Fmt(d.ThumbId);
                }
            }

        }

        protected void DataList1_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "delete")
            {
                int id = e.CommandArgument.ToInt();
                var form = DbUtil.Db.VolunteerForms.Single(f => f.Id == id);

                ImageData.Image.DeleteOnSubmit(form.SmallId);
                ImageData.Image.DeleteOnSubmit(form.MediumId);
                ImageData.Image.DeleteOnSubmit(form.LargeId);

                DbUtil.Db.VolunteerForms.DeleteOnSubmit(form);
                DbUtil.Db.SubmitChanges();
                DataList1.DataBind();
            }
        }
    }
}
