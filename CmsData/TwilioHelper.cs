using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Twilio;

namespace CmsData
{
    public class TwilioHelper
    {
        public static void sendSMS( String sFrom, String sTo, String sBody )
        {
            // Needs API keys. Removed to keep private
            var twilio = new TwilioRestClient("", "");
            var msg = twilio.SendSmsMessage(sFrom, sTo, sBody);

            var status = msg.Status;
        }
    }
}
