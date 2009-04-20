using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CmsData
{
    public partial class NewContact
    {
        public enum ContactTypeCode
        {
            PersonalVisit = 1,
            PhoneCall = 2,
            LetterSent = 3,
            CardSent = 4,
            EmailSent = 5,
            InfoPackSent = 6,
            Other = 7,
            PhoneIn = 11,
            SurveyEE = 12,
        }
        public ContactTypeCode ContactTypeEnum
        {
            get { return (ContactTypeCode)ContactTypeId; }
            set { ContactTypeId = (int)value; }
        }
        public enum ContactReasonCode
        {
            Unknown = 99,
            Bereavement = 100,
            Health = 110,
            Personal = 120,
            OutReach = 130,
            ComeAndSee = 131,
            InReach = 140,
            Information = 150,
            Other = 160,
        }
        public ContactReasonCode ContactReasonEnum
        {
            get { return (ContactReasonCode)ContactReasonId; }
            set { ContactReasonId = (int)value; }
        }
    }
}
