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

            var list = new SMSList();

            list.Created = DateTime.Now;
            list.SendAt = DateTime.Now;
            list.SenderID = Util2.CurrentPeopleId;
            list.Message = sMessage;

            DbUtil.Db.SMSLists.InsertOnSubmit(list);
            DbUtil.Db.SubmitChanges();

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
    }
}
