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
using com.qas.proweb;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;

namespace CmsWeb
{
    public class AddressVerify
    {
        public static string GetZip4(Person p)
        {
            var a = LookupAddress(p.PrimaryAddress, p.PrimaryAddress2, p.PrimaryCity, p.PrimaryState, p.PrimaryZip);
            if (a.found)
                return a.Zip;
            else
                return p.PrimaryZip;
        }
        public static string GetZip4(AddressResult a)
        {
            var a2 = LookupAddress(a.Line1, a.Line2, a.City, a.State, a.Zip);
            if (a2.found)
                return a2.Zip;
            else
                return a.Zip;
        }
        public class AddressResult
        {
            public bool found { get; set; }
            //public string selector { get; set; }
            public string address { get; set; }
            public string Line1 { get; set; }
            public string Line2 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Zip { get; set; }
        }
        public static AddressResult LookupAddress(string line1, string line2, string city, string st, string zip)
        {
            string url = DbUtil.Db.Setting("QAServer", "");
            if (!url.HasValue())
                return new AddressResult { found = false };

            var ws = new QuickAddress(url);
            ws.Engine = QuickAddress.EngineTypes.Verification;
            var address = new string[] { line1, line2, city, st, zip };

            var result = ws.Search("USA", address, PromptSet.Types.Default);
            if (result.VerifyLevel == SearchResult.VerificationLevels.PremisesPartial)
            {
                var a = result.Picklist.Items[0].PartialAddress;
            }
            else if (result.VerifyLevel == SearchResult.VerificationLevels.Verified
                || result.VerifyLevel == SearchResult.VerificationLevels.InteractionRequired)
            {
                var addr = new AddressResult();
                var ads = result.Address.AddressLines;
                if (ads[0].Line.HasValue())
                    addr.found = true;
                addr.Line1 = ads[0].Line;
                addr.Line2 = ads[1].Line;
                addr.City = ads[2].Line;
                addr.State = ads[3].Line;
                addr.Zip = ads[4].Line;

                string lab = addr.Line1;
                if (addr.Line2.HasValue())
                    lab += "\n" + addr.Line2;
                lab += "\n" + Util.FormatCSZ4(addr.City, addr.State, addr.Zip);
                addr.address = lab;

                return addr;
            }
            return new AddressResult { found = false };
        }
    }
}
