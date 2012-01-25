/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using CmsData;
using CmsData.View;
using System.Collections;
using UtilityExtensions;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Collections.Specialized;
using System.Xml.Serialization;
using System.IO;
using System.Web.Configuration;
using System.Web;

namespace CmsWeb
{
    public class AddressVerify
    {
        public class AddressResult
        {
            public bool found { get; set; }
            public string address { get; set; }
            public string Line1 { get; set; }
            public string Line2 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Zip { get; set; }
        }
        public static AddressResult LookupAddress(string line1, string line2, string city, string st, string zip)
        {
            string url = WebConfigurationManager.AppSettings["amiurl"];
            string password = WebConfigurationManager.AppSettings["amipassword"];

            if (!password.HasValue())
            {
                string f = HttpContext.Current.Server.MapPath("/amipassword.txt");
                if (File.Exists(f))
                    password = File.ReadAllText(f);
            }
            if (!url.HasValue() || !password.HasValue())
                return new AddressResult { found = false };

            var wc = new WebClient();
            var coll = new NameValueCollection();
            coll.Add("line1", line1);
            coll.Add("line2", line2);
            coll.Add("csz", Util.FormatCSZ(city, st, zip));
            coll.Add("passcode", password);
            try
            {
                var resp = wc.UploadValues(url, "POST", coll);
                var s = Encoding.ASCII.GetString(resp);
                var serializer = new XmlSerializer(typeof(AddressResult));
                var reader = new StringReader(s);
                var ret = (AddressResult)serializer.Deserialize(reader);
                return ret;
            }
            catch (Exception)
            {
                return new AddressResult { Line1 = "error" };
            }
        }
    }
}
