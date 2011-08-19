using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using UtilityExtensions;
using System.Text;
using CmsData.Codes;

namespace CmsWeb.Models
{
    public partial class OnlineRegPersonModel
    {
        public OrganizationMember Enroll(Transaction ti, string paylink, bool? testing, string others)
        {
            var om = OrganizationMember.InsertOrgMembers(DbUtil.Db,
                org.OrganizationId, person.PeopleId,
                MemberTypeCode.Member, DateTime.Now, null, false);
            om.Amount = TotalAmount();
            om.AmountPaid = AmountToPay();

            var reg = person.RecRegs.SingleOrDefault();

            if (reg == null)
            {
                reg = new RecReg();
                person.RecRegs.Add(reg);
            }
            if (setting.AskShirtSize == true)
            {
                om.ShirtSize = shirtsize;
                reg.ShirtSize = shirtsize;
            }
            if (setting.AskChurch == true)
            {
                reg.ActiveInAnotherChurch = otherchurch;
                reg.Member = memberus;
            }
            if (setting.AskAllergies == true)
            {
                reg.MedAllergy = medical.HasValue();
                reg.MedicalDescription = medical;
            }
            if (setting.AskParents == true)
            {
                reg.Mname = mname;
                reg.Fname = fname;
            }
            if (setting.AskEmContact == true)
            {
                reg.Emcontact = emcontact;
                reg.Emphone = emphone;
            }
            if (setting.AskDoctor == true)
            {
                reg.Docphone = docphone;
                reg.Doctor = doctor;
            }
            if (setting.AskCoaching == true)
                reg.Coaching = coaching;
            if (setting.AskInsurance == true)
            {
                reg.Insurance = insurance;
                reg.Policy = policy;
            }
            if (setting.AskGrade == true)
                om.Grade = grade.ToInt();
            if (setting.AskTickets == true)
                om.Tickets = ntickets;

            foreach (var yn in setting.YesNoQuestions)
            {
                om.RemoveFromGroup(DbUtil.Db, "Yes:" + yn.SmallGroup);
                om.RemoveFromGroup(DbUtil.Db, "No:" + yn.SmallGroup);
            }
            if (setting.YesNoQuestions.Count > 0)
                foreach (var g in YesNoQuestion)
                    om.AddToGroup(DbUtil.Db, (g.Value == true ? "Yes:" : "No:") + g.Key);
            foreach (var ck in setting.Checkboxes)
                om.RemoveFromGroup(DbUtil.Db, ck.SmallGroup);
            if (setting.Checkboxes.Count > 0 && Checkbox != null)
                foreach (var g in Checkbox)
                    om.AddToGroup(DbUtil.Db, g);
            if (setting.Checkboxes2.Count > 0 && Checkbox2 != null)
                foreach (var g in Checkbox2)
                    om.AddToGroup(DbUtil.Db, g);
            if (setting.ExtraQuestions.Count > 0)
                foreach (var g in ExtraQuestion)
                    if (g.Value.HasValue())
                        om.AddToMemberData("{0}: {1}".Fmt(g.Key, g.Value));
            if (setting.MenuItems.Count > 0)
                foreach (var i in MenuItem)
                    om.AddToGroup(DbUtil.Db, i.Key, i.Value);

            foreach (var op in Options())
                om.RemoveFromGroup(DbUtil.Db, op.Value);
            if (setting.Dropdown1.Count > 0)
                om.AddToGroup(DbUtil.Db, option);

            foreach (var op in ExtraOptions())
                om.RemoveFromGroup(DbUtil.Db, op.Value);
            if (setting.Dropdown2.Count > 0)
                om.AddToGroup(DbUtil.Db, option2);

            if (setting.GradeOptions.Count > 0)
                om.Grade = gradeoption.ToInt();

            foreach (var ag in setting.AgeGroups)
                om.RemoveFromGroup(DbUtil.Db, ag.SmallGroup);
            if (setting.AgeGroups.Count > 0)
                om.AddToGroup(DbUtil.Db, AgeGroup());

            if (setting.LinkGroupsFromOrgs.Count > 0)
            {
                var q = from omt in DbUtil.Db.OrgMemMemTags
                        where setting.LinkGroupsFromOrgs.Contains(omt.OrgId)
                        where omt.PeopleId == om.PeopleId
                        select omt.MemberTag.Name;
                foreach (var name in q)
                    om.AddToGroup(DbUtil.Db, name);
            }

            string tstamp = Util.Now.ToString("MMM d yyyy h:mm tt");
            om.AddToMemberData(tstamp);
            var tran = "{0:C} ({1}{2})".Fmt(
                    om.AmountPaid.ToString2("C"), ti.TransactionId, testing == true ? " test" : "");
            if (om.AmountPaid > 0)
            {
                om.AddToMemberData(tran);
                if (others.HasValue())
                    om.AddToMemberData("Others: " + others);
            }
            if (setting.MenuItems.Count > 0)
            {
                var menulabel = "Menu Items";
                foreach (var i in MenuItemsChosen())
                {
                    om.AddToMemberData(menulabel);
                    string desc;
                    if (i.amt > 0)
                        desc = "{0} {1} (at {2:N2})".Fmt(i.number, i.desc, i.amt);
                    else
                        desc = "{0} {1}".Fmt(i.number, i.desc);
                    om.AddToMemberData(desc);
                    menulabel = string.Empty;
                }
            }

            if (setting.AskTylenolEtc == true)
            {
                reg.Tylenol = tylenol;
                reg.Advil = advil;
                reg.Robitussin = robitussin;
                reg.Maalox = maalox;
            }

            reg.AddToComments("-------------");
            reg.AddToComments(email);
            if (request.HasValue())
            {
                reg.AddToComments("Request: " + request);
                om.Request = request;
            }

            if (om.AmountPaid > 0)
            {
                if (AmountDue() > 0)
                    reg.AddToComments("{0:C} due".Fmt(AmountDue().ToString("C")));
                reg.AddToComments(tran);
            }
            if (paylink.HasValue())
            {
                om.PayLink = paylink;
                reg.AddToComments(paylink);
            }
            reg.AddToComments(tstamp);
            reg.AddToComments("{0} - {1}".Fmt(org.DivisionName, org.OrganizationName));

            DbUtil.Db.SubmitChanges();
            return om;
        }
        public string PrepareSummaryText(Transaction ti)
        {
            var om = GetOrgMember();
            var sb = new StringBuilder();
            sb.Append("<table>");
            sb.AppendFormat("<tr><td>Org:</td><td>{0}</td></tr>\n", org.OrganizationName);
            sb.AppendFormat("<tr><td>First:</td><td>{0}</td></tr>\n", person.PreferredName);
            sb.AppendFormat("<tr><td>Last:</td><td>{0}</td></tr>\n", person.LastName);

            //sb.AppendFormat("<tr><td>DOB:</td><td>{0:d}</td></tr>\n", person.DOB);
            //sb.AppendFormat("<tr><td>Gender:</td><td>{0}</td></tr>\n", 
            //    person.GenderId == 1 ? "M" : 
            //    person.GenderId == 2 ? "F" : "U");
            //sb.AppendFormat("<tr><td>Addr:</td><td>{0}</td></tr>\n", person.PrimaryAddress);
            //sb.AppendFormat("<tr><td>City:</td><td>{0}</td></tr>\n", person.PrimaryCity);
            //sb.AppendFormat("<tr><td>State:</td><td>{0}</td></tr>\n", person.PrimaryState);
            //sb.AppendFormat("<tr><td>Zip:</td><td>{0}</td></tr>\n", person.PrimaryZip.Zip5());
            //sb.AppendFormat("<tr><td>Home Phone:</td><td>{0}</td></tr>\n", person.Family.HomePhone.FmtFone());
            //sb.AppendFormat("<tr><td>Cell Phone:</td><td>{0}</td></tr>\n", person.CellPhone.FmtFone());

            var rr = person.RecRegs.Single();

            if (setting.AskTickets == true)
                sb.AppendFormat("<tr><td>Tickets:</td><td>{0}</td></tr>\n", om.Tickets);
            if (setting.AskShirtSize == true)
                sb.AppendFormat("<tr><td>Shirt:</td><td>{0}</td></tr>\n", om.ShirtSize);
            if (setting.AskEmContact == true)
            {
                sb.AppendFormat("<tr><td>Emerg Contact:</td><td>{0}</td></tr>\n", rr.Emcontact);
                sb.AppendFormat("<tr><td>Emerg Phone:</td><td>{0}</td></tr>\n", rr.Emphone);
            }
            if (setting.AskDoctor == true)
            {
                sb.AppendFormat("<tr><td>Physician Name:</td><td>{0}</td></tr>\n", rr.Doctor);
                sb.AppendFormat("<tr><td>Physician Phone:</td><td>{0}</td></tr>\n", rr.Docphone);
            }
            if (setting.AskInsurance == true)
            {
                sb.AppendFormat("<tr><td>Insurance Carrier:</td><td>{0}</td></tr>\n", rr.Insurance);
                sb.AppendFormat("<tr><td>Insurance Policy:</td><td>{0}</td></tr>\n", rr.Policy);
            }
            if (setting.AskRequest == true)
                sb.AppendFormat("<tr><td>{1}:</td><td>{0}</td></tr>\n", om.Request, setting.RequestLabel.HasValue() ? setting.RequestLabel : "Request");
            if (setting.AskAllergies == true)
                sb.AppendFormat("<tr><td>Medical:</td><td>{0}</td></tr>\n", rr.MedicalDescription);

            if (setting.AskTylenolEtc == true)
            {
                sb.AppendFormat("<tr><td>Tylenol?: {0},", tylenol == true ? "Yes" : tylenol == false ? "No" : "");
                sb.AppendFormat(" Advil?: {0},", advil == true ? "Yes" : advil == false ? "No" : "");
                sb.AppendFormat(" Robitussin?: {0},", robitussin == true ? "Yes" : robitussin == false ? "No" : "");
                sb.AppendFormat(" Maalox?: {0}</td></tr>\n", maalox == true ? "Yes" : maalox == false ? "No" : "");
            }
            if (setting.AskChurch == true)
            {
                sb.AppendFormat("<tr><td>Member:</td><td>{0}</td></tr>\n", rr.Member);
                sb.AppendFormat("<tr><td>OtherChurch:</td><td>{0}</td></tr>\n", rr.ActiveInAnotherChurch);
            }
            if (setting.AskParents == true)
            {
                sb.AppendFormat("<tr><td>Mother's name:</td><td>{0}</td></tr>\n", rr.Mname);
                sb.AppendFormat("<tr><td>Father's name:</td><td>{0}</td></tr>\n", rr.Fname);
            }
            if (setting.AskCoaching == true)
                sb.AppendFormat("<tr><td>Coaching:</td><td>{0}</td></tr>\n", rr.Coaching);
            if (setting.AskGrade == true)
                sb.AppendFormat("<tr><td>Grade:</td><td>{0}</td></tr>\n", om.Grade);

            if (setting.AgeGroups.Count > 0)
                sb.AppendFormat("<tr><td>AgeGroup:</td><td>{0}</td></tr>\n", AgeGroup());

            if (setting.Dropdown1.Count > 0)
                sb.AppendFormat("<tr><td>{1}:</td><td>{0}</td></tr>\n", option, 
                    Util.PickFirst(setting.Dropdown1Label, "Options"));
            if (setting.Dropdown2.Count > 0)
                sb.AppendFormat("<tr><td>{1}:</td><td>{0}</td></tr>\n", option2, 
                    Util.PickFirst(setting.Dropdown2Label, "Extra Options"));
            if (setting.Dropdown3.Count > 0)
                sb.AppendFormat("<tr><td>{1}:</td><td>{0}</td></tr>\n", option3, 
                    Util.PickFirst(setting.Dropdown3Label, "Extra Options"));
            if (setting.MenuItems.Count > 0)
            {
                var menulabel = "Menu Items";
                foreach (var i in MenuItemsChosen())
                {
                    string row;
                    if (i.amt > 0)
                        row = "<tr><td>{0}</td><td>{1} {2} (at {3:N2}</td></tr>\n".Fmt(menulabel, i.number, i.desc, i.amt);
                    else
                        row = "<tr><td>{0}</td><td>{1} {2}</td></tr>\n".Fmt(menulabel, i.number, i.desc);
                    sb.AppendFormat(row);
                    menulabel = string.Empty;
                }
            }
            if (setting.Checkboxes.Count > 0)
            {
                var menulabel = setting.CheckBoxLabel;
                foreach (var i in CheckboxItemsChosen())
                {
                    string row;
                    if (i.Fee > 0)
                        row = "<tr><td>{0}</td><td>{1} (${2:N2}</td></tr>\n".Fmt(menulabel, i.Description, i.Fee);
                    else
                        row = "<tr><td>{0}</td><td>{1}</td></tr>\n".Fmt(menulabel, i.Description);
                    sb.AppendFormat(row);
                    menulabel = string.Empty;
                }
            }
            if (setting.Checkboxes2.Count > 0)
            {
                var menulabel = setting.CheckBox2Label;
                foreach (var i in Checkbox2ItemsChosen())
                {
                    string row;
                    if (i.Fee > 0)
                        row = "<tr><td>{0}</td><td>{1} (${2:N2}</td></tr>\n".Fmt(menulabel, i.Description, i.Fee);
                    else
                        row = "<tr><td>{0}</td><td>{1}</td></tr>\n".Fmt(menulabel, i.Description);
                    sb.AppendFormat(row);
                    menulabel = string.Empty;
                }
            }
            if (setting.GradeOptions.Count > 0)
                sb.AppendFormat("<tr><td>GradeOption:</td><td>{0}</td></tr>\n",
                    GradeOptions().SingleOrDefault(s => s.Value == (gradeoption ?? "00")).Text);
            if (setting.YesNoQuestions.Count > 0)
                foreach (var a in setting.YesNoQuestions)
                    sb.AppendFormat("<tr><td>{0}:</td><td>{1}</td></tr>\n".Fmt(a.Question, YesNoQuestion[a.SmallGroup] == true ? "Yes" : "No"));
            if (setting.ExtraQuestions.Count > 0)
                foreach (var a in ExtraQuestion)
                    if (a.Value.HasValue())
                        sb.AppendFormat("<tr><td>{0}:</td><td>{1}</td></tr>\n".Fmt(a.Key, a.Value));

            //if (donation > 0)
            //    sb.AppendFormat("<tr><td>Donation:</td><td>{0:c}</td></tr>\n", donation);

            if (ti.Amt > 0)
                sb.AppendFormat("<tr><td>Amount Paid:</td><td>{0:c}</td></tr>\n", ti.Amt);
            sb.Append("</table>");

            return sb.ToString();
        }
        private string AgeGroup()
        {
            foreach (var i in setting.AgeGroups)
                if (person.Age >= i.StartAge && person.Age <= i.EndAge)
                    return i.SmallGroup;
            return string.Empty;
        }
    }
}