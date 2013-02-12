using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using Twilio;
using UtilityExtensions;
using CmsData;

namespace CmsData
{
    public class TwilioHelper
    {
        public const Boolean bTestMode = true;

        public static void QueueSMS(int iQBID, int iSendGroupID, string sMessage)
        {
            var q = DbUtil.Db.PeopleQuery(iQBID);

            // Create new SMS send list
            var list = new SMSList();

            list.Created = DateTime.Now;
            list.SendAt = DateTime.Now;
            list.SenderID = Util2.CurrentPeopleId;
            list.SendGroupID = iSendGroupID;
            list.Message = sMessage;

            DbUtil.Db.SMSLists.InsertOnSubmit(list);
            DbUtil.Db.SubmitChanges();

            // Check for how many people have cell numbers and want to receive texts
            var qSMS = from p in q
                where p.CellPhone != null
                where p.ReceiveSMS == true
                select p;

            var countSMS = qSMS.Count();

            if (countSMS > 0)
            {
                foreach (var i in qSMS)
                {
                    var item = new SMSItem();

                    item.ListID = list.Id;
                    item.PeopleID = i.PeopleId;
                    item.Number = i.CellPhone;

                    DbUtil.Db.SMSItems.InsertOnSubmit(item);
                }

                DbUtil.Db.SubmitChanges();
            }

            // Add counts for SMS, e-Mail and none
            list.SentSMS = countSMS;
            list.SentNone = q.Count() - countSMS;

            DbUtil.Db.SubmitChanges();

            ProcessQueue(list.Id);
        }

        public static void ProcessQueue( int iNewListID )
        {
            string sHost = Util.Host;
            string sSID = getSID();
            string sToken = getToken();
            int iListID = iNewListID;

            if (sSID.Length == 0 || sToken.Length == 0) return;

            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

                string stSID = sSID;
                string stToken = sToken;
                int itListID = iListID;

                try
                {
                    var Db = new CMSDataContext(Util.GetConnectionString(sHost));
                    Db.Host = sHost;

                    var smsList = (from e in Db.SMSLists
                                   where e.Id == itListID
                                   select e).Single();

                    var smsItems = from e in Db.SMSItems
                                   where e.ListID == itListID
                                   select e;

                    var smsGroup = (from e in Db.SMSNumbers
                                    where e.GroupID == smsList.SendGroupID
                                    select e).ToList();

                    int iCount = 0;

                    foreach (var item in smsItems)
                    {
                        sendSMS( stSID, stToken, smsGroup[iCount].Number, item.Number, smsList.Message );

                        iCount++;
                        if( iCount >= smsGroup.Count() ) iCount = 0;
                    }

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            });
        }


        public static void sendSMS( String sSID, String sToken, String sFrom, String sTo, String sBody )
        {
            // Needs API keys. Removed to keep private

            if (bTestMode)
            {
                Debug.WriteLine("Message sending to " + sTo + " from " + sFrom + ": " + sBody + " --- via " + sSID + " / " + sToken);
            }
            else
            {
                var twilio = new TwilioRestClient(sSID, sToken);
                var msg = twilio.SendSmsMessage(sFrom, sTo, sBody);
                var status = msg.Status;
            }
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

        public static List<SMSGroup> getAvailableLists(int iUserID)
        {
            var groups = (from e in DbUtil.Db.SMSGroups
                          where e.SMSGroupMembers.Any( f => f.UserID == iUserID )
                          select e).ToList();

            return groups;
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
