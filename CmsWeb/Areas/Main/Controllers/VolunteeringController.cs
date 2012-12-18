using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using UtilityExtensions;

namespace CmsWeb.Areas.Main.Controllers
{
    public class VolunteeringController : Controller
    {
        public ActionResult Index(int id)
        {
            Volunteer vol = DbUtil.Db.Volunteers.SingleOrDefault(e => e.PeopleId == id);

            if (vol == null) vol = initVolunteer(id);

            return View(vol);
        }

        public ActionResult Edit(int id)
        {
            Volunteer vol = DbUtil.Db.Volunteers.SingleOrDefault(e => e.PeopleId == id);

            if (vol == null) initVolunteer(id);

            return View(vol);
        }

        public ActionResult Update(Volunteer v)
        {
            Volunteer vol = DbUtil.Db.Volunteers.SingleOrDefault(e => e.PeopleId == v.PeopleId);

            vol.Leader = v.Leader;
            vol.ProcessedDate = v.ProcessedDate;
            vol.Standard = v.Standard;
            vol.StatusId = v.StatusId;
            vol.Children = v.Children;
            vol.Comments = v.Comments;

            DbUtil.Db.SubmitChanges();
            return View("Index", vol);
        }

        public ActionResult Upload(int PeopleID, HttpPostedFileBase file)
        {
            Volunteer vol = DbUtil.Db.Volunteers.SingleOrDefault(e => e.PeopleId == PeopleID);

            var f = new VolunteerForm { UploaderId = Util.UserId1, PeopleId = vol.PeopleId };
            f.AppDate = Util.Now;
            f.UploaderId = Util.UserId1;

            var bits = new byte[file.ContentLength];
            file.InputStream.Read(bits, 0, bits.Length);

            var mimetype = file.ContentType.ToLower();

            switch (mimetype)
            {
                case "image/jpeg":
                case "image/pjpeg":
                case "image/gif":
                {
                    f.IsDocument = false;

                    try
                    {
                        f.SmallId = ImageData.Image.NewImageFromBits(bits, 165, 220).Id;
                        f.MediumId = ImageData.Image.NewImageFromBits(bits, 675, 900).Id;
                        f.LargeId = ImageData.Image.NewImageFromBits(bits).Id;
                    }
                    catch
                    {
                        return View("Index", vol);
                    }

                    break;
                }

                case "text/plain":
                case "application/pdf":
                case "application/msword":
                case "application/vnd.ms-excel":
                {
                    f.MediumId = ImageData.Image.NewImageFromBits(bits, mimetype).Id;
                    f.SmallId = f.MediumId;
                    f.LargeId = f.MediumId;
                    f.IsDocument = true;
                    break;
                }

                default: return View("Index", vol);
            }

            DbUtil.Db.VolunteerForms.InsertOnSubmit(f);
            DbUtil.Db.SubmitChanges();
            DbUtil.LogActivity("Uploading VolunteerApp for {0}".Fmt(vol.Person.Name));

            return Redirect("/Volunteering/Index/" + vol.PeopleId);
        }

        public ActionResult Delete(int id, int PeopleID)
        {
            var form = DbUtil.Db.VolunteerForms.Single(f => f.Id == id);

            ImageData.Image.DeleteOnSubmit(form.SmallId);
            ImageData.Image.DeleteOnSubmit(form.MediumId);
            ImageData.Image.DeleteOnSubmit(form.LargeId);

            DbUtil.Db.VolunteerForms.DeleteOnSubmit(form);
            DbUtil.Db.SubmitChanges();

            return Redirect("/Volunteering/Index/" + PeopleID);
        }

        private Volunteer initVolunteer(int id)
        {
            Volunteer vol = new Volunteer { PeopleId = id };
            DbUtil.Db.Volunteers.InsertOnSubmit(vol);
            DbUtil.Db.SubmitChanges();
            DbUtil.LogActivity("Viewing VolunteerApp for {0}".Fmt(vol.Person.Name));
            return vol;
        }

        public ActionResult CreateCheck(int id, string sCombo)
        {
            ProtectMyMinistryHelper.create(id, sCombo);
            return Redirect("/Volunteering/Index/" + id);
        }

        public ActionResult SubmitCheck(int id, int iPeopleID, string sSSN, string sDLN, int iStateID = 0 )
        {
            String sResponseURL = Request.Url.Scheme + "://" + Request.Url.Authority + ProtectMyMinistryHelper.PMM_Append;

            ProtectMyMinistryHelper.submit(id, sSSN, sDLN, sResponseURL, iStateID);

            Volunteer vol = DbUtil.Db.Volunteers.SingleOrDefault(e => e.PeopleId == iPeopleID);
            vol.ProcessedDate = DateTime.Now;
            DbUtil.Db.SubmitChanges();

            return Redirect("/Volunteering/Index/" + iPeopleID);
        }

        public ActionResult DialogSubmit(int id)
        {
            BackgroundCheck bc = (from e in DbUtil.Db.BackgroundChecks
                                  where e.Id == id
                                  select e).Single();

            switch( bc.ServiceCode )
            {
                case "Combo":
                {
                    return View("SubmitCombo", bc);
                }

                case "MVR":
                {
                    return View("SubmitMVR", bc);
                }

                default: return View();
            }
        }

        public ActionResult DialogType(int id)
        {
            Person p = (from e in DbUtil.Db.People
                        where e.PeopleId == id
                        select e).Single();
            return View( p );
        }
    }
}
