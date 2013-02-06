using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Twilio;

namespace CmsData
{
    public class TwilioHelper
    {
        public const int SENT_NONE = 0;
        public const int SENT_SMS = 1;
        public const int SENT_EMAIL = 2;

        public static void QueueSMS(int iQBID, string sMessage)
        {
            var q = DbUtil.Db.PeopleQuery(iQBID);

            // Create new SMS send list
            var list = new SMSList();

            list.Created = DateTime.Now;
            list.SendAt = DateTime.Now;
            list.SenderID = Util2.CurrentPeopleId;
            list.Message = sMessage;

            DbUtil.Db.SMSLists.InsertOnSubmit(list);
            DbUtil.Db.SubmitChanges();

            // Check for how many people have cell numbers and want to receive texts
            var qSMS = from p in q
                where p.CellPhone != null
                where p.ReceiveSMS > 0
                select p;

            var countSMS = qSMS.Count();

            if (countSMS > 0)
            {
                foreach (var i in qSMS)
                {
                    var item = new SMSItem();

                    item.ListID = list.Id;
                    item.SendToID = i.PeopleId;
                    item.SendBy = SENT_SMS;
                    item.SendAddress = i.CellPhone;

                    DbUtil.Db.SMSItems.InsertOnSubmit(item);
                }

                DbUtil.Db.SubmitChanges();
            }

            // Check for how many people do not have cell numbers or don't want to receive texts but have e-mails
            var qEMail = from p in q
                         where p.ReceiveSMS == 0
                         where p.EmailAddress != null
                         where p.EmailAddress != ""
                         where (p.SendEmailAddress1 ?? true) || (p.SendEmailAddress2 ?? false)
                         select p;

            var countEMail = qEMail.Count();
            if ( countEMail > 0)
            {
                foreach (var i in qEMail)
                {
                    var item = new SMSItem();

                    item.ListID = list.Id;
                    item.SendToID = i.PeopleId;
                    item.SendBy = SENT_EMAIL;
                    item.SendAddress = i.EmailAddress;

                    DbUtil.Db.SMSItems.InsertOnSubmit(item);
                }

                DbUtil.Db.SubmitChanges();
            }

            // Add counts for SMS, e-Mail and none
            list.SentSMS = countSMS;
            list.SentEMail = countEMail;
            list.SentNone = q.Count() - countSMS - countEMail;
            DbUtil.Db.SubmitChanges();
        }

        public static void sendSMS( String sFrom, String sTo, String sBody )
        {
            // Needs API keys. Removed to keep private
            var twilio = new TwilioRestClient("", "");
            var msg = twilio.SendSmsMessage(sFrom, sTo, sBody);

            var status = msg.Status;
        }

        public static List<IncomingPhoneNumber> getNumberList()
        {
            var twilio = new TwilioRestClient( getSID(), getToken() );
            var numbers = twilio.ListIncomingPhoneNumbers();

            return numbers.IncomingPhoneNumbers;
        }

        public static List<IncomingPhoneNumber> getUnusedNumberList()
        {
            var twilio = new TwilioRestClient(getSID(), getToken());
            var numbers = twilio.ListIncomingPhoneNumbers();

            var used = (from e in DbUtil.Db.SMSNumbers
                        select e).ToList();

            for (var iX = numbers.IncomingPhoneNumbers.Count() - 1; iX > -1; iX--)
            {
                if (used.Where(n => n.Number == numbers.IncomingPhoneNumbers[iX].PhoneNumber).Count() > 0)
                    numbers.IncomingPhoneNumbers.RemoveAt(iX);
            }

            return numbers.IncomingPhoneNumbers;
        }

        public static List<UserRole> getUnassignedPeople(int id)
        {
            var role = (from e in DbUtil.Db.Roles
                        where e.RoleName == "SendSMS"
                        select e).SingleOrDefault();

            // If no results on the role, send back empty list
            if( role == null ) return new List<UserRole>();


            var assigned = (from e in DbUtil.Db.SMSGroupMembers
                            where e.GroupID == id
                            select e).ToList();

            var people = (from e in DbUtil.Db.UserRoles
                          where e.RoleId == role.RoleId
                          select e).ToList();

            for (var iX = people.Count() - 1; iX > -1; iX--)
            {
                if (assigned.Where(n => n.UserID == people[iX].UserId).Count() > 0)
                    people.RemoveAt(iX);
            }

            return people;
        }

        public static string getSID()
        {
            return DbUtil.Db.Setting("TwilioSID", "");
        }

        public static string getToken()
        {
            return DbUtil.Db.Setting("TwilioToken", "");
        }
   
    }
}
