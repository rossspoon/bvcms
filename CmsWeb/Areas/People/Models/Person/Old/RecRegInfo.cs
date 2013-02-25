using System.Linq;
using CmsData;
using UtilityExtensions;

namespace CmsWeb.Areas.People.Models.Person
{
    public class RecRegInfo
    {
        public int PeopleId { get; set; }

        public string Comments { get; set; }
        public string shirtsize { get; set; }
        public bool? custody { get; set; }
        public bool? transport { get; set; }
        public string emcontact { get; set; }
        public string emphone { get; set; }
        public string insurance { get; set; }
        public string policy { get; set; }
        public string doctor { get; set; }
        public string docphone { get; set; }
        public string medical { get; set; }
        public bool? tylenol { get; set; }
        public bool? advil { get; set; }
        public bool? robitussin { get; set; }
        public bool? maalox { get; set; }
        public string mname { get; set; }
        public string fname { get; set; }
        public bool member { get; set; }
        public bool otherchurch { get; set; }
        public bool? coaching { get; set; }

        public static RecRegInfo GetRecRegInfo(int id)
        {
            var q = from r in DbUtil.Db.RecRegs
                    where r.PeopleId == id
                    orderby r.Id descending
                    select new RecRegInfo
                    {
                        PeopleId = id,
                        Comments = r.Comments,
                        coaching = r.Coaching,
                        docphone = r.Docphone,
                        emcontact = r.Emcontact,
                        doctor = r.Doctor,
                        emphone = r.Emphone,
                        fname = r.Fname,
                        insurance = r.Insurance,
                        medical = r.MedicalDescription,
                        tylenol = r.Tylenol,
                        advil = r.Advil,
                        robitussin = r.Robitussin,
                        maalox = r.Maalox,
                        member = r.Member ?? false,
                        mname = r.Mname,
                        otherchurch = r.ActiveInAnotherChurch ?? false,
                        policy = r.Policy,
                        shirtsize = r.ShirtSize,
                        custody = r.Person.CustodyIssue,
                        transport = r.Person.OkTransport,
                    };
            var rr = q.FirstOrDefault();
            if (rr == null)
                rr = new RecRegInfo { PeopleId = id };
            return rr;
        }
        public void UpdateRecReg()
        {
            var p = DbUtil.Db.LoadPersonById(PeopleId);
            var rr = p.RecRegs.SingleOrDefault(r => r.PeopleId == PeopleId);
            if (rr == null)
            {
                rr = new RecReg();
                p.RecRegs.Add(rr);
            }
            rr.Comments = Comments;
            rr.Coaching = coaching;
            rr.ActiveInAnotherChurch = otherchurch;
            rr.Docphone = docphone;
            rr.Doctor = doctor;
            rr.Emcontact = emcontact;
            rr.Emphone = emphone;
            rr.Fname = fname;
            rr.Insurance = insurance;
            rr.MedAllergy = medical.HasValue();
            rr.MedicalDescription = medical;
            rr.Tylenol = tylenol;
            rr.Advil = advil;
            rr.Robitussin = robitussin;
            rr.Maalox = maalox;
            rr.Member = member;
            rr.Mname = mname;
            rr.Policy = policy;
            rr.ShirtSize = shirtsize;
            p.CustodyIssue = custody;
            p.OkTransport = transport;

            DbUtil.Db.SubmitChanges();
            DbUtil.LogActivity("Updated RecReg: {0}".Fmt(p.Name));
        }
    }
}
