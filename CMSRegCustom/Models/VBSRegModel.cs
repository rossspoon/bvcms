using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using CmsData;
using System.Web.Mvc;
using System.Text;
using System.Configuration;
using UtilityExtensions;
using CMSWebCommon.Models;

namespace CMSRegCustom.Models
{
    public interface IVBSFormBindable
    {
        string first { get; set; }
        string nickname { get; set; }
        string last { get; set; }
        string dob { get; set; }
        int? gender { get; set; }
        string grade { get; set; }
        string address { get; set; }
        string city { get; set; }
        string state { get; set; }
        string zip { get; set; }
        string locaddr { get; set; }
        string phone { get; set; }
        string parent { get; set; }
        string cell { get; set; }
        string email { get; set; }
        string emcontact { get; set; }
        string emphone { get; set; }
        string request { get; set; }
        string medical { get; set; }
        bool bellevue { get; set; }
        bool otherchurch { get; set; }
        string bringer { get; set; }
        string bringerphone { get; set; }
        int? parentvbs { get; set; }
        int? pubphoto { get; set; }
    }
    public class VBSRegModel : IVBSFormBindable
    {
        public string first {get; set;}
        public string nickname {get; set;}
        public string last {get; set;}
        public string dob {get; set;}
        private DateTime _Birthday;
        private DateTime birthday
        {
            get
            {
                if (_Birthday == DateTime.MinValue)
                    Util.DateValid(dob, out _Birthday);
                return _Birthday;
            }
        }
        public int? gender { get; set; }
        public string grade {get; set;}
        public string address {get; set;}
        public string city {get; set;}
        public string state { get; set; }
        public string zip {get; set;}
        public string locaddr {get; set;}
        public string phone {get; set;}
        public string parent {get; set;}
        public string cell {get; set;}
        public string email {get; set;}
        public string emcontact {get; set;}
        public string emphone {get; set;}
        public string request {get; set;}
        public string medical {get; set;}
        public bool bellevue {get; set;}
        public bool otherchurch {get; set;}
        public string bringer {get; set;}
        public string bringerphone {get; set;}
        public int? parentvbs {get; set;}
		public int? pubphoto { get; set; }
        public Person person { get; set; }

        private CMSDataContext Db = DbUtil.Db;

        public string PrepareSummaryText()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("First:\t{0}\n", first);
            sb.AppendFormat("Nick:\t{0}\n", nickname);
            sb.AppendFormat("Last:\t{0}\n", last);
            sb.AppendFormat("DOB:\t{0:d}\n", birthday);
            sb.AppendFormat("Gender:\t{0}\n", gender == 1 ? "M" : "F");
            sb.AppendFormat("Grade:\t{0}\n", grade);
            sb.AppendFormat("Addr:\t{0}\n", address);
            sb.AppendFormat("City:\t{0}\n", city);
            sb.AppendFormat("State:\t{0}\n", state);
            sb.AppendFormat("Zip:\t{0}\n", zip);
            sb.AppendFormat("Local:\t{0}\n", locaddr);
            //sb.AppendFormat("Phone:\t{0}\n", homephone.FmtFone());

            sb.AppendFormat("Parent:\t{0}\n", parent);
            //sb.AppendFormat("Cell:\t{0}\n", cell.FmtFone());
            sb.AppendFormat("Email:\t{0}\n", email);
            sb.AppendFormat("Emerg Contact:\t{0}\n", emcontact);
            //sb.AppendFormat("Emerg Phone:\t{0}\n", emphone.FmtFone());
            sb.AppendFormat("Request:\t{0}\n", request);
            sb.AppendFormat("Medical:\t{0}\n", medical);
            sb.AppendFormat("Bellevue:\t{0}\n", bellevue);
            sb.AppendFormat("Bringer:\t{0}\n", bringer);
			//sb.AppendFormat("BringerPh:\t{0}\n", bringerphone.FmtFone());
			sb.AppendFormat("WorksVBS:\t{0}\n", parentvbs == 1);
			sb.AppendFormat("PubPhoto:\t{0}\n", pubphoto == 1);
            sb.AppendFormat("OtherChurch:\t{0}\n", otherchurch);

            return sb.ToString();
        }

        public IEnumerable<SelectListItem> StateList()
        {
            var q = from r in Db.StateLookups
                    select new SelectListItem
                    {
                        Text = r.StateCode,
                        Selected = r.StateCode == DbUtil.Settings("DefaultState", "TN"),
                    };
            return q;
        }
        public int FindMember()
        {
            int count;
            person = SearchPeopleModel.FindPerson(phone, first, last, birthday, out count);
            return count;
        }

        public void ValidateModel(ModelStateDictionary modelState)
        {
            SearchPeopleModel.ValidateFindPerson(modelState, first, last, birthday, phone);

            if (!phone.HasValue())
                modelState.AddModelError("phone", "phone required");
            var d = cell.GetDigits().Length;
            if (cell.HasValue() && (d != 7 && d != 10))
                modelState.AddModelError("cell", "optional or 7 or 10 digits.");

            if (!gender.HasValue)
                modelState.AddModelError("gender2", "gender required");
            if (grade == "0")
                modelState.AddModelError("grade", "grade required");
            if (!address.HasValue())
                modelState.AddModelError("address", "address required");
            if (!city.HasValue())
                modelState.AddModelError("city", "city required");
            if (!zip.HasValue())
                modelState.AddModelError("zip", "zip required");
			if (!Util.ValidEmail(email))
				modelState.AddModelError("email", "Please specify a valid email address.");
			if (!pubphoto.HasValue)
				modelState.AddModelError("pubphoto2", "Please specify whether we can publish photo.");
			if (!emcontact.HasValue())
				modelState.AddModelError("emcontact", "emergency contact required");
			if (!emphone.HasValue())
				modelState.AddModelError("emphone", "emergency phone # required");
			if (!bringer.HasValue())
				modelState.AddModelError("bringer", "bringer contact required");
			if (!bringerphone.HasValue())
				modelState.AddModelError("bringerphone", "bringer phone # required");

            if (modelState.IsValid)
                FindMember();
        }
        public void SaveVBSApp()
        {
            var t = PrepareSummaryText();
            var bits = System.Text.ASCIIEncoding.ASCII.GetBytes(t);
            var vb = new VBSApp();
            Db.VBSApps.InsertOnSubmit(vb);
            var i = ImageData.Image.NewTextFromBits(bits);
            vb.ImgId = i.Id;
            vb.IsDocument = true;
            vb.Uploaded = DateTime.Now;
            vb.ActiveInAnotherChurch = otherchurch;
			vb.PubPhoto = pubphoto == 1;
            vb.GradeCompleted = grade;
            vb.Request = request;
            vb.MedAllergy = medical.HasValue();

            vb.Person = person;
            Db.SubmitChanges();
        }
		public IEnumerable<SelectListItem> GradeCompleteds()
		{
			var sa = new[]
			{ 
				new { Value="0", Text="(please select a grade)" }, 
				new { Value="Pre-K", Text="Pre-K (4 before Oct 1 last year)" }, 
				new { Value="K-5", Text="K-5 (5 before Oct 1 last year)" }, 
				new { Value="1st", Text="1st grade completed this May" }, 
				new { Value="2nd", Text="2nd grade completed this May" }, 
				new { Value="3rd", Text="3rd grade completed this May" }, 
				new { Value="4th", Text="4th grade completed this May" }, 
				new { Value="5th", Text="5th grade completed this May" }, 
				new { Value="Exceptional", Text="Exceptional" }, 
			};
			return from g in sa
				   select new SelectListItem
				   {
					   Text = g.Text,
					   Value = g.Value,
				   };
		}
	}
}
