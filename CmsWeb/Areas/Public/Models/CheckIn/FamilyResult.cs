using System;
using System.Collections.Generic;
using System.Web;
using System.Xml;
using System.Web.Mvc;
using System.Xml.Linq;
using UtilityExtensions;
using System.Linq;
using CmsData;

namespace CmsWeb.Models
{
    public class FamilyResult : ActionResult
    {
        int fid, campus, thisday, page;
        bool waslocked;
        bool kioskmode;

        public FamilyResult(int fid, int campus, int thisday, int page, bool waslocked, bool kioskmode)
        {
            this.fid = fid;
            this.campus = campus;
            this.thisday = thisday;
            this.page = page;
            this.waslocked = waslocked;
            this.kioskmode = kioskmode;
            if (fid > 0)
            {
                var db = new CMSDataContext(Util.ConnectionString);
                var lockf = db.FamilyCheckinLocks.SingleOrDefault(f => f.FamilyId == fid);
                if (lockf == null)
                {
                    lockf = new FamilyCheckinLock { FamilyId = fid, Created = DateTime.Now };
                    db.FamilyCheckinLocks.InsertOnSubmit(lockf);
                }
                lockf.Locked = true;
                if (!waslocked)
                    lockf.Created = DateTime.Now;
                db.SubmitChanges();
            }
        }
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "text/xml";
            var settings = new XmlWriterSettings() { Encoding = new System.Text.UTF8Encoding(false), Indent = true };

            using (var w = XmlWriter.Create(context.HttpContext.Response.OutputStream, settings))
            { 
                w.WriteStartElement("Attendees");
                var m = new CheckInModel();
                List<Attendee> q;
                if (kioskmode == true)
                    q = m.FamilyMembersKiosk(fid, campus, thisday);
                else
                    q = m.FamilyMembers(fid, campus, thisday);
                w.WriteAttributeString("familyid", fid.ToString());
                w.WriteAttributeString("waslocked", waslocked.ToString());

                var count = q.Count();

                if (page > 0)
                {
                    const int INT_PageSize = 10;
                    var startrow = (page - 1) * INT_PageSize;
                    if (count > startrow + INT_PageSize)
                        w.WriteAttributeString("next", (page + 1).ToString());
                    else
                        w.WriteAttributeString("next", "");
                    if (page > 1)
                        w.WriteAttributeString("prev", (page - 1).ToString());
                    else
                        w.WriteAttributeString("prev", "");
                    q = q.Skip(startrow).Take(INT_PageSize).ToList();
                }
                w.WriteAttributeString("maxlabels", DbUtil.Db.Setting("MaxLabels", "6"));
                var code = DbUtil.Db.NextSecurityCode(DateTime.Today).Select(c => c.Code).Single();
                w.WriteAttributeString("securitycode", code);

				var accommodateCheckInBug = DbUtil.Db.Setting("AccommodateCheckinBug", "false").ToBool();

                foreach (var c in q)
                {
                    double leadtime = 0;
                    if (c.Hour.HasValue)
                    {
                        var midnight = c.Hour.Value.Date;
                        var now = midnight.Add(Util.Now.TimeOfDay);
                        leadtime = c.Hour.Value.Subtract(now).TotalHours;
                        leadtime -= DbUtil.Db.Setting("TZOffset", "0").ToInt(); // positive to the east, negative to the west
                    }
                    w.WriteStartElement("attendee");
                    w.WriteAttributeString("id", c.Id.ToString());
                    w.WriteAttributeString("mv", c.MemberVisitor);
                    w.WriteAttributeString("name", c.DisplayName);
                    w.WriteAttributeString("preferredname", c.PreferredName);
					if (accommodateCheckInBug) // bug in checkin requires this
						w.WriteAttributeString("first", c.PreferredName); 
					else
	                    w.WriteAttributeString("first", c.First);
                    w.WriteAttributeString("last", c.Last);
                    w.WriteAttributeString("org", c.DisplayClass);
                    w.WriteAttributeString("orgname", c.OrgName);
                    w.WriteAttributeString("leader", c.Leader);
                    w.WriteAttributeString("orgid", c.OrgId.ToString());
                    w.WriteAttributeString("loc", c.Location);
                    w.WriteAttributeString("gender", c.gender.ToString());
                    w.WriteAttributeString("leadtime", leadtime.ToString());
                    w.WriteAttributeString("age", c.Age.ToString());
                    w.WriteAttributeString("numlabels", c.NumLabels.ToString());
                    w.WriteAttributeString("checkedin", c.CheckedIn.ToString());
                    w.WriteAttributeString("custody", c.Custody.ToString());
                    w.WriteAttributeString("transport", c.Transport.ToString());
                    w.WriteAttributeString("hour", c.Hour.FormatDateTm());
                    w.WriteAttributeString("requiressecuritylabel", c.RequiresSecurityLabel.ToString());
                    w.WriteAttributeString("church", c.church);

                    w.WriteAttributeString("email", c.email);
                    w.WriteAttributeString("dob", c.dob);
                    w.WriteAttributeString("goesby", c.goesby);
                    w.WriteAttributeString("addr", c.addr);
                    w.WriteAttributeString("zip", c.zip);
                    w.WriteAttributeString("home", c.home);
                    w.WriteAttributeString("cell", c.cell);
                    w.WriteAttributeString("marital", c.marital.ToString());
                    w.WriteAttributeString("allergies", c.allergies);
                    w.WriteAttributeString("grade", c.grade.ToString());
                    w.WriteAttributeString("parent", c.parent);
                    w.WriteAttributeString("emfriend", c.emfriend);
                    w.WriteAttributeString("emphone", c.emphone);
                    w.WriteAttributeString("activeother", c.activeother.ToString());
                    w.WriteAttributeString("haspicture", c.HasPicture.ToString());

                    w.WriteEndElement();
                }
                w.WriteEndElement();
            }
        }
    }
}