using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using UtilityExtensions;
using System.Text;

namespace CmsWeb.Models
{
    public partial class OnlineRegPersonModel
    {
        public OrganizationMember Enroll(string TransactionID, string paylink, bool? testing, string others)
        {
            //(int)RegistrationEnum.AttendMeeting)
            var om = OrganizationMember.InsertOrgMembers(org.OrganizationId, person.PeopleId,
                (int)OrganizationMember.MemberTypeCode.Member, DateTime.Now, null, false);
            om.Amount = TotalAmount();
            om.AmountPaid = AmountToPay();

            var reg = person.RecRegs.SingleOrDefault();

            if (reg == null)
            {
                reg = new RecReg();
                person.RecRegs.Add(reg);
            }
            if (org.AskShirtSize == true)
            {
                om.ShirtSize = shirtsize;
                reg.ShirtSize = shirtsize;
            }
            if (org.AskChurch == true)
            {
                reg.ActiveInAnotherChurch = otherchurch;
                reg.Member = memberus;
            }
            if (org.AskAllergies == true)
            {
                reg.MedAllergy = medical.HasValue();
                reg.MedicalDescription = medical;
            }
            if (org.AskParents == true)
            {
                reg.Mname = mname;
                reg.Fname = fname;
            }
            if (org.AskEmContact == true)
            {
                reg.Emcontact = emcontact;
                reg.Emphone = emphone;
            }
            if (org.AskDoctor == true)
            {
                reg.Docphone = docphone;
                reg.Doctor = doctor;
            }
            if (org.AskCoaching == true)
                reg.Coaching = coaching;
            if (org.AskInsurance == true)
            {
                reg.Insurance = insurance;
                reg.Policy = policy;
            }
            if (org.AskGrade == true)
                om.Grade = grade.ToInt();
            if (org.AskTickets == true)
                om.Tickets = ntickets;

            foreach (var yn in YesNoQuestions())
            {
                om.RemoveFromGroup(DbUtil.Db, "Yes:" + yn.name);
                om.RemoveFromGroup(DbUtil.Db, "No:" + yn.name);
            }
            if (org.YesNoQuestions.HasValue())
                foreach (var g in YesNoQuestion)
                    om.AddToGroup(DbUtil.Db, (g.Value == true ? "Yes:" : "No:") + g.Key);
            if (org.ExtraQuestions.HasValue())
                foreach (var g in ExtraQuestion)
                    if (g.Value.HasValue())
                        om.AddToMemberData("{0}: {1}".Fmt(g.Key, g.Value));
            if (org.MenuItems.HasValue())
                foreach (var i in MenuItem)
                    om.AddToGroup(DbUtil.Db, i.Key, i.Value);

            foreach (var op in Options())
                om.RemoveFromGroup(DbUtil.Db, op.Value);
            if (org.AskOptions.HasValue())
                om.AddToGroup(DbUtil.Db, option);

            foreach (var op in ExtraOptions())
                om.RemoveFromGroup(DbUtil.Db, op.Value);
            if (org.ExtraOptions.HasValue())
                om.AddToGroup(DbUtil.Db, option2);

            if (org.GradeOptions.HasValue())
                om.Grade = gradeoption.ToInt();

            foreach (var ag in AgeGroups())
                om.RemoveFromGroup(DbUtil.Db, ag.Name);
            if (org.AgeGroups.HasValue())
                om.AddToGroup(DbUtil.Db, AgeGroup());

            if (org.LinkGroupsFromOrgs.HasValue())
            {
                var a = org.LinkGroupsFromOrgs.Split(',').Select(s => s.ToInt()).ToArray();
                var q = from omt in DbUtil.Db.OrgMemMemTags
                        where a.Contains(omt.OrgId) && omt.PeopleId == om.PeopleId
                        select omt.MemberTag.Name;
                foreach (var name in q)
                    om.AddToGroup(DbUtil.Db, name);
            }

            string tstamp = Util.Now.ToString("MMM d yyyy h:mm tt");
            om.AddToMemberData(tstamp);
            var tran = "{0:C} ({1}{2})".Fmt(
                    om.AmountPaid.ToString2("C"), TransactionID, testing == true ? " test" : "");
            if (om.AmountPaid > 0)
            {
                om.AddToMemberData(tran);
                if (others.HasValue())
                    om.AddToMemberData("Others: " + others);
            }
            if (org.MenuItems.HasValue())
            {
                var menulabel = "Menu Items";
                foreach (var i in MenuItemsChosen())
                {
                    om.AddToMemberData(menulabel);
                    om.AddToMemberData("{0} {1} (at {2:N2})".Fmt(i.number, i.desc, i.amt));
                    menulabel = string.Empty;
                }
            }

            if (org.AskTylenolEtc == true)
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
        public string PrepareSummaryText()
        {
            var om = GetOrgMember();
            var sb = new StringBuilder();
            sb.Append("<table>");
            sb.AppendFormat("<tr><td>Org:</td><td>{0}</td></tr>\n", org.OrganizationName);
            sb.AppendFormat("<tr><td>First:</td><td>{0}</td></tr>\n", person.PreferredName);
            sb.AppendFormat("<tr><td>Last:</td><td>{0}</td></tr>\n", person.LastName);

            sb.AppendFormat("<tr><td>DOB:</td><td>{0:d}</td></tr>\n", person.DOB);
            sb.AppendFormat("<tr><td>Gender:</td><td>{0}</td></tr>\n", person.GenderId == 1 ? "M" : "F");
            sb.AppendFormat("<tr><td>Addr:</td><td>{0}</td></tr>\n", person.PrimaryAddress);
            sb.AppendFormat("<tr><td>City:</td><td>{0}</td></tr>\n", person.PrimaryCity);
            sb.AppendFormat("<tr><td>State:</td><td>{0}</td></tr>\n", person.PrimaryState);
            sb.AppendFormat("<tr><td>Zip:</td><td>{0}</td></tr>\n", person.PrimaryZip.Zip5());
            sb.AppendFormat("<tr><td>Home Phone:</td><td>{0}</td></tr>\n", person.Family.HomePhone.FmtFone());
            sb.AppendFormat("<tr><td>Cell Phone:</td><td>{0}</td></tr>\n", person.CellPhone.FmtFone());

            var rr = person.RecRegs.Single();

            if (org.AskTickets == true)
                sb.AppendFormat("<tr><td>Tickets:</td><td>{0}</td></tr>\n", om.Tickets);
            if (org.AskShirtSize == true)
                sb.AppendFormat("<tr><td>Shirt:</td><td>{0}</td></tr>\n", om.ShirtSize);
            if (org.AskEmContact == true)
            {
                sb.AppendFormat("<tr><td>Emerg Contact:</td><td>{0}</td></tr>\n", rr.Emcontact);
                sb.AppendFormat("<tr><td>Emerg Phone:</td><td>{0}</td></tr>\n", rr.Emphone);
            }
            if (org.AskDoctor == true)
            {
                sb.AppendFormat("<tr><td>Physician Name:</td><td>{0}</td></tr>\n", rr.Doctor);
                sb.AppendFormat("<tr><td>Physician Phone:</td><td>{0}</td></tr>\n", rr.Docphone);
            }
            if (org.AskInsurance == true)
            {
                sb.AppendFormat("<tr><td>Insurance Carrier:</td><td>{0}</td></tr>\n", rr.Insurance);
                sb.AppendFormat("<tr><td>Insurance Policy:</td><td>{0}</td></tr>\n", rr.Policy);
            }
            if (org.AskRequest == true)
                sb.AppendFormat("<tr><td>{1}:</td><td>{0}</td></tr>\n", om.Request, om.Organization.RequestLabel.HasValue() ? om.Organization.RequestLabel : "Request");
            if (org.AskAllergies == true)
                sb.AppendFormat("<tr><td>Medical:</td><td>{0}</td></tr>\n", rr.MedicalDescription);

            if (org.AskTylenolEtc == true)
            {
                sb.AppendFormat("<tr><td>Tylenol?: {0},", tylenol == true ? "Yes" : tylenol == false ? "No" : "");
                sb.AppendFormat(" Advil?: {0},", advil == true ? "Yes" : advil == false ? "No" : "");
                sb.AppendFormat(" Robitussin?: {0},", robitussin == true ? "Yes" : robitussin == false ? "No" : "");
                sb.AppendFormat(" Maalox?: {0}</td></tr>\n", maalox == true ? "Yes" : maalox == false ? "No" : "");
            }
            if (org.AskChurch == true)
            {
                sb.AppendFormat("<tr><td>Member:</td><td>{0}</td></tr>\n", rr.Member);
                sb.AppendFormat("<tr><td>OtherChurch:</td><td>{0}</td></tr>\n", rr.ActiveInAnotherChurch);
            }
            if (org.AskParents == true)
            {
                sb.AppendFormat("<tr><td>Mother's name:</td><td>{0}</td></tr>\n", rr.Mname);
                sb.AppendFormat("<tr><td>Father's name:</td><td>{0}</td></tr>\n", rr.Fname);
            }
            if (org.AskCoaching == true)
                sb.AppendFormat("<tr><td>Coaching:</td><td>{0}</td></tr>\n", rr.Coaching);
            if (org.AskGrade == true)
                sb.AppendFormat("<tr><td>Grade:</td><td>{0}</td></tr>\n", om.Grade);

            if (org.AgeGroups.HasValue())
                sb.AppendFormat("<tr><td>AgeGroup:</td><td>{0}</td></tr>\n", AgeGroup());

            if (org.AskOptions.HasValue())
                sb.AppendFormat("<tr><td>{1}:</td><td>{0}</td></tr>\n", option, Util.PickFirst(om.Organization.OptionsLabel, "Options"));
            if (org.ExtraOptions.HasValue())
                sb.AppendFormat("<tr><td>{1}:</td><td>{0}</td></tr>\n", option2, Util.PickFirst(om.Organization.ExtraOptionsLabel, "Extra Options"));
            if (org.MenuItems.HasValue())
            {
                var menulabel = "Menu Items";
                foreach (var i in MenuItemsChosen())
                {
                    sb.AppendFormat("<tr><td>{0}</td><td>{1} {2} (at {3:N2}</td></tr>\n", menulabel, i.number, i.desc, i.amt);
                    menulabel = string.Empty;
                }
            }
            if (org.GradeOptions.HasValue())
                sb.AppendFormat("<tr><td>GradeOption:</td><td>{0}</td></tr>\n",
                    GradeOptions().SingleOrDefault(s => s.Value == (gradeoption ?? "00")).Text);
            if (org.YesNoQuestions.HasValue())
                foreach (var a in YesNoQuestions())
                    sb.AppendFormat("<tr><td>{0}:</td><td>{1}</td></tr>\n".Fmt(a.desc, YesNoQuestion[a.name] == true ? "Yes" : "No"));
            if (org.ExtraQuestions.HasValue())
                foreach (var a in ExtraQuestion)
                    if (a.Value.HasValue())
                        sb.AppendFormat("<tr><td>{0}:</td><td>{1}</td></tr>\n".Fmt(a.Key, a.Value));

            var amt = AmountToPay();
            if (amt > 0)
                sb.AppendFormat("<tr><td>Amount Paid:</td><td>{0:c}</td></tr>\n", amt);
            sb.Append("</table>");

            return sb.ToString();
        }
        private string AgeGroup()
        {
            foreach (var i in AgeGroups())
                if (person.Age >= i.StartAge && person.Age <= i.EndAge)
                    return i.Name;
            return string.Empty;
        }
    }
}