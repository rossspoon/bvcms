using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using CmsWeb.Areas.Main.Models.Other;
using UtilityExtensions;

namespace CmsWeb.Areas.Main.Controllers
{
    public class VolunteeringController : Controller
    {
        public ActionResult Index(int id)
        {
            var vol = new VolunteerModel(id);
            return View(vol);
        }

        public ActionResult Edit(int id)
        {
            var vol = new VolunteerModel(id);
            return View(vol);
        }

        public ActionResult Update(int id, string processDate, int statusId, string comments, List<int> approvals)
        {
            var m = new VolunteerModel(id);
            var dt = processDate.ToDate();
            m.Update(dt, statusId, comments, approvals);
            return RedirectToAction("Index", "Volunteering", new {id = id});
        }

        public ActionResult Upload(int id, HttpPostedFileBase file)
        {
            Volunteer vol = DbUtil.Db.Volunteers.SingleOrDefault(e => e.PeopleId == id);

            var f = new VolunteerForm
                        {
                            UploaderId = Util.UserId1, 
                            PeopleId = vol.PeopleId, 
                            Name = System.IO.Path.GetFileName(file.FileName).Truncate(100),
                            AppDate = Util.Now,
                        };

            var bits = new byte[file.ContentLength];
            file.InputStream.Read(bits, 0, bits.Length);

            var mimetype = file.ContentType.ToLower();

            switch (mimetype)
            {
                case "image/jpeg":
                case "image/pjpeg":
                case "image/gif":
                case "image/png":
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


        public ActionResult CreateCheck(int id, string code, int type, int label = 0)
        {
            ProtectMyMinistryHelper.create(id, code, type, label);
            return Redirect("/Volunteering/Index/" + id);
        }

        public ActionResult EditCheck(int id, int label = 0)
        {
            BackgroundCheck bc = (from e in DbUtil.Db.BackgroundChecks
                                  where e.Id == id
                                  select e).Single();

            bc.ReportLabelID = label;

            DbUtil.Db.SubmitChanges();

            return Redirect("/Volunteering/Index/" + bc.PeopleID);
        }

        public ActionResult DeleteCheck(int id)
        {
            int iPeopleID = 0;

            BackgroundCheck bc = (from e in DbUtil.Db.BackgroundChecks
                                  where e.Id == id
                                  select e).Single();

            iPeopleID = bc.PeopleID;

            DbUtil.Db.BackgroundChecks.DeleteOnSubmit(bc);
            DbUtil.Db.SubmitChanges();

            return Redirect("/Volunteering/Index/" + iPeopleID);
        }

        public ActionResult SubmitCheck(int id, int iPeopleID, string sSSN, string sDLN, string sUser = "", string sPassword = "", int iStateID = 0)
        {
            String sResponseURL = Request.Url.Scheme + "://" + Request.Url.Authority + ProtectMyMinistryHelper.PMM_Append;

            ProtectMyMinistryHelper.submit(id, sSSN, sDLN, sResponseURL, iStateID, sUser, sPassword);

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

            switch (bc.ServiceCode)
            {
                case "Combo":
                    return View("SubmitCombo", bc);
                case "MVR":
                    return View("SubmitMVR", bc);
                case "Credit":
                    return View("SubmitCredit", bc);
                default:
                    return Content("no view");
            }
        }

        public ActionResult DialogEdit(int id)
        {
            BackgroundCheck bc = (from e in DbUtil.Db.BackgroundChecks
                                  where e.Id == id
                                  select e).Single();

            return View(bc);
        }

        public ActionResult DialogDelete(int id)
        {
            ViewBag.ID = id;
            return View();
        }

        public ActionResult DialogType(int id, int type)
        {
            Person p = (from e in DbUtil.Db.People
                        where e.PeopleId == id
                        select e).Single();

            ViewBag.dialogType = type;

            return View(p);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult Edit(string id, string value)
        {
            var iid = id.Substring(2).ToInt();
            var f = DbUtil.Db.VolunteerForms.Single(m => m.Id == iid);
            if (id.StartsWith("n"))
                f.Name = value.Truncate(100);
            DbUtil.Db.SubmitChanges();
            var c = new ContentResult();
            c.Content = value;
            return c;
        }
    }
}
