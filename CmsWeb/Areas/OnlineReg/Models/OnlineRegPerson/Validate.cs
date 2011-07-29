using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using UtilityExtensions;
using System.Web.Mvc;
using CmsData.Codes;

namespace CmsWeb.Models
{
    public partial class OnlineRegPersonModel
    {
        public bool IsValidForExisting { get; set; }
        public bool IsValidForContinue { get; set; }
        private void ValidateBirthdayRange(ModelStateDictionary ModelState)
        {
            if(org != null)
                if (!birthday.HasValue && (org.BirthDayStart.HasValue || org.BirthDayEnd.HasValue))
                    ModelState.AddModelError(inputname("dob"), "birthday required");
                else if (birthday.HasValue)
                {
                    if ((org.BirthDayStart.HasValue && birthday < org.BirthDayStart)
                        || (org.BirthDayEnd.HasValue && birthday > org.BirthDayEnd))
                        ModelState.AddModelError(inputname("dob"), "birthday outside age allowed range");
                }
        }
        private void ValidBasic(ModelStateDictionary ModelState)
        {
            if (!first.HasValue())
                ModelState.AddModelError(inputname("first"), "first name required");
            if (!last.HasValue())
                ModelState.AddModelError(inputname("last"), "last name required");

            int n = 0;
            if (birthday.HasValue && birthday > DateTime.MinValue)
                n++;
            if (Util.ValidEmail(email))
                n++;
            var d = phone.GetDigits().Length;
            if (phone.HasValue() && d == 10)
                n++;
            if (n == 0)
                ModelState.AddModelError(inputname("dob"), "we require one of valid birthdate, email or phone to find your record");

            if (!Util.ValidEmail(email))
                ModelState.AddModelError(inputname("email"), "valid email required");
            if (phone.HasValue() && d != 10)
                ModelState.AddModelError(inputname("phone"), "10 digits required");
        }
        public void ValidateModelForFind(ModelStateDictionary ModelState, OnlineRegModel m)
        {
            IsValidForContinue = true; // true till proven false
            if (!this.PeopleId.HasValue)
                ValidBasic(ModelState);
            if (UserSelectsOrganization())
                if ((classid ?? 0) == 0)
                    if (IsFamily)
                        ModelState.AddModelError("classidfam", "please choose a group/event");
                    else
                        ModelState.AddModelError("classidguest", "please choose a group/event");
            if (ComputesOrganizationByAge() && !birthday.HasValue)
                ModelState.AddModelError(inputname("dob"), "birthday required");
            if (orgid == Util.CreateAccountCode && age < 16)
                ModelState.AddModelError(inputname("dob"), "must be 16 to create account");
            if (!IsFamily && (!email.HasValue() || !Util.ValidEmail(email)))
                ModelState.AddModelError(inputname("email"), "Please specify a valid email address.");
            if (ModelState.IsValid)
            {
                Found = person != null;
                if (count == 1)
                {
                    address = person.PrimaryAddress;
                    city = person.PrimaryCity;
                    state = person.PrimaryState;
                    zip = person.PrimaryZip;
                    gender = person.GenderId;
                    married = person.MaritalStatusId == 2 ? 2 : 1;

                    if ((ManageSubscriptions() || orgid == Util.CreateAccountCode) && !person.EmailAddress.HasValue())
                    {
                        ModelState.AddModelError(ErrorTarget, "No Email Address on record");
                        NotFoundText = @"We have found your record but we have no email address for you.<br/>
This means that we cannot proceed until we have that to protect your data.<br/>
Please call the church to resolve this before we can complete your information.";
                        IsValidForContinue = false;
                    }
                    else if (ComputesOrganizationByAge() && org == null)
                    {
                        ModelState.AddModelError(inputname("dob"), "Sorry, cannot find an appropriate age group");
                        IsValidForContinue = false;
                    }
                    else if (MemberOnly() && person.MemberStatusId != MemberStatusCode.Member)
                    {
                        ModelState.AddModelError(ErrorTarget, "Sorry, must be a member of church");
                        IsValidForContinue = false;
                    }
                    else if (org != null)
                    {
                        var om = org.OrganizationMembers.SingleOrDefault(mm => mm.PeopleId == PeopleId);
                        if (org.RegistrationTypeId == RegistrationEnum.CreateAccount)
                        {
#if DEBUG2
#else
                            if (person.Users.Count() > 0)
                            {
                                ModelState.AddModelError(ErrorTarget, "You already have an account");
                                IsValidForContinue = false;
                            }
                            if (!Util.ValidEmail(person.EmailAddress))
                            {
                                ModelState.AddModelError(ErrorTarget, "You must have a valid email address on record");
                                NotFoundText = @"We have found your record but we do not have a valid email for you.<br/>
For your protection, we cannot continue to create an account.<br />
We can't use the one you enter online here since we can't be sure this is you.<br />
Please call the church to resolve this before we can complete your account.<br />";
                                IsValidForContinue = false;
                            }
#endif
                        }
                        else if (om != null && org.RegistrationTypeId != RegistrationEnum.ChooseSlot)
                        {
#if DEBUG
#else
                            ModelState.AddModelError(ErrorTarget, "This person is already registered");
                            IsValidForContinue = false;
#endif
                        }
                        else if (org.ValidateOrgs.HasValue())
                        {
                            var q = from s in org.ValidateOrgs.Split(',')
                                    select s.ToInt();
                            var a = q.ToArray();
                            if (!person.OrganizationMembers.Any(mm => a.Contains(mm.OrganizationId)))
                            {
                                ModelState.AddModelError(ErrorTarget, "Must be member of specified organization");
                                IsValidForContinue = false;
                            }
                        }
                    }
                    if (m.List.Count(ii => ii.PeopleId == this.PeopleId) > 1)
                    {
                        ModelState.AddModelError(ErrorTarget, "Person already in Pending Registrations");
                        IsValidForContinue = false;
                    }
                }
                else if (count > 1)
                {
                    ModelState.AddModelError(ErrorTarget, "More than one match, sorry");
                    NotFoundText = @"We have found more than one record that matches your information
This is an unexpected error and we don't know which one is you.
Please call the church to resolve this before we can complete your registration.";
                    IsValidForContinue = false;
                }
                else if (count == 0)
                {
                    ModelState.AddModelError(ErrorTarget, "record not found");
                    NotFoundText = 
@"The first and last name in addition to either birthday, email, or phone,
must match a record in the system.
Please search with a different email, phone, or birthday.";
                }
            }
            ValidateBirthdayRange(ModelState);
            IsValidForExisting = ModelState.IsValid;
        }
        public bool IsValidForNew { get; set; }
        internal void ValidateModelForNew(ModelStateDictionary ModelState)
        {
            var isnewfamily = whatfamily == 3;
            ValidBasic(ModelState);
            if (!birthday.HasValue && RequiredDOB())
                ModelState.AddModelError(inputname("dob"), "birthday required");
            if (orgid == Util.CreateAccountCode && age < 16)
                ModelState.AddModelError(inputname("dob"), "must be 16 to create account");

            if (ComputesOrganizationByAge() && GetAppropriateOrg() == null)
                ModelState.AddModelError(inputname("dob"), "Sorry, cannot find an appropriate age group");

            ValidateBirthdayRange(ModelState);
            int n = 0;
            if (phone.HasValue() && phone.GetDigits().Length == 10)
                n++;
            if (ShowAddress && homephone.HasValue() && homephone.GetDigits().Length == 10)
                n++;

            if (RequiredPhone() && n == 0)
                ModelState.AddModelError(inputname("phone"), "cell or home phone required");

            if (email.HasValue())
                email = email.Trim();
            if (!email.HasValue() || !Util.ValidEmail(email))
                ModelState.AddModelError(inputname("email"), "Please specify a valid email address.");
            if (isnewfamily && !address.HasValue() && RequiredAddr())
                ModelState.AddModelError(inputname("address"), "address required.");
            if (isnewfamily && !city.HasValue() && RequiredZip())
                ModelState.AddModelError(inputname("city"), "city required.");
            if (isnewfamily && RequiredZip() && (!zip.HasValue() || zip.GetDigits().Length < 5))
                ModelState.AddModelError(inputname("zip"), "zip needs at least 5 digits.");
            if (isnewfamily && !state.HasValue() && RequiredZip())
                ModelState.AddModelError(inputname("state"), "state required");
            if (!gender.HasValue && RequiredGender())
                ModelState.AddModelError(inputname("gender"), "Please specify gender");
            if (!married.HasValue && RequiredMarital())
                ModelState.AddModelError(inputname("married"), "Please specify marital status");

            if (MemberOnly())
                ModelState.AddModelError(ErrorTarget, "Sorry, must be a member of church");
            else if (org != null && org.ValidateOrgs.HasValue())
                ModelState.AddModelError(ErrorTarget, "Must be member of specified organization");

            IsValidForNew = ModelState.IsValid;
        }
        public void ValidateModelForOther(ModelStateDictionary modelState)
        {
            if (org.AskEmContact == true)
            {
                if (!emcontact.HasValue())
                    modelState.AddModelError(inputname("emcontact"), "emergency contact required");
                if (!emphone.HasValue())
                    modelState.AddModelError(inputname("emphone"), "emergency phone # required");
            }

            if (org.AskInsurance == true)
            {
                if (!insurance.HasValue())
                    modelState.AddModelError(inputname("insurance"), "insurance carrier required");
                if (!policy.HasValue())
                    modelState.AddModelError(inputname("policy"), "insurnace policy # required");
            }

            if (org.AskDoctor == true)
            {
                if (!doctor.HasValue())
                    modelState.AddModelError(inputname("doctor"), "Doctor's name required");
                if (!docphone.HasValue())
                    modelState.AddModelError(inputname("docphone"), "Doctor's phone # required");
            }
            if (org.AskTylenolEtc == true)
            {
                if (!tylenol.HasValue)
                    modelState.AddModelError(inputname("tylenol"), "please indicate");
                if (!advil.HasValue)
                    modelState.AddModelError(inputname("advil"), "please indicate");
                if (!maalox.HasValue)
                    modelState.AddModelError(inputname("maalox"), "please indicate");
                if (!robitussin.HasValue)
                    modelState.AddModelError(inputname("robitussin"), "please indicate");
            }

            if (org.AskShirtSize == true)
                if (shirtsize == "0")
                    modelState.AddModelError(inputname("shirtsize"), "please select a shirt size");

            if (org.AskGrade == true)
            {
                int g = 0;
                if (!int.TryParse(grade, out g))
                    modelState.AddModelError(inputname("grade"), "please enter a grade");
                else if (g < org.GradeAgeStart || g > org.GradeAgeEnd)
                    modelState.AddModelError(inputname("grade"), "only grades from {0} to {1}".Fmt(org.GradeAgeStart, org.GradeAgeEnd));
            }

            if (org.AskCoaching == true)
                if (!coaching.HasValue)
                    modelState.AddModelError(inputname("coaching"), "please indicate");

            if (org.AskOptions.HasValue())
                if (option == "0")
                    modelState.AddModelError(inputname("option"), "please select an option");

            if (org.ExtraOptions.HasValue())
                if (option2 == "0")
                    modelState.AddModelError(inputname("option2"), "please select an option");

            if (org.GradeOptions.HasValue())
                if (gradeoption == "00")
                    modelState.AddModelError(inputname("gradeoption"), "please select a grade option");

            if (org.AskParents == true)
            {
                if (!mname.HasValue() && !fname.HasValue())
                    modelState.AddModelError(inputname("fname"), "please provide either mother or father name");
                else
                {
                    string mfirst, mlast;
                    Person.NameSplit(mname, out mfirst, out mlast);
                    if (mname.HasValue() && !mfirst.HasValue())
                        modelState.AddModelError(inputname("mname"), "provide first and last names");
                    string ffirst, flast;
                    Person.NameSplit(fname, out ffirst, out flast);
                    if (fname.HasValue() && !ffirst.HasValue())
                        modelState.AddModelError(inputname("fname"), "provide first and last names");
                }
            }
            if (org.AskTickets == true)
                if ((ntickets ?? 0) == 0)
                    modelState.AddModelError(inputname("ntickets"), "please enter a number of tickets");

            if (org.Deposit > 0)
                if (!paydeposit.HasValue)
                    modelState.AddModelError(inputname("paydeposit"), "please indicate");

            foreach (var a in YesNoQuestions())
            {
                if (YesNoQuestion == null || !YesNoQuestion.ContainsKey(a.name))
                    modelState.AddModelError(inputname("YesNoQuestion[" + a.n + "].Value"), "please select yes or no");
            }
            foreach (var q in ExtraQuestions())
            {
                if (ExtraQuestion == null || !ExtraQuestion.ContainsKey(q.question) || !ExtraQuestion[q.question].HasValue())
                    modelState.AddModelError(inputname("ExtraQuestion[" + q.n + "].Value"), "please give some answer");
            }
            if (org.CheckboxesMax.HasValue && Checkbox != null && Checkbox.Length > org.CheckboxesMax)
                modelState.AddModelError("checkboxes", "Max of {0} exceded".Fmt(org.CheckboxesMax));
            if (org.CheckboxesMin.HasValue && (Checkbox == null || Checkbox.Length < org.CheckboxesMin))
                modelState.AddModelError("checkboxes", "Min of {0} required".Fmt(org.CheckboxesMin));
            OtherOK = modelState.IsValid;
        }
    }
}