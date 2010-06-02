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

namespace CMSWeb
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
            public string selector { get; set; }
            public string address { get; set; }
            public string Line1 { get; set; }
            public string Line2 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Zip { get; set; }
        }
        public static AddressResult LookupAddress(string line1, string line2, string city, string st, string zip)
        {
            string url = DbUtil.Settings("QAServer", "");
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
                var q = from ad in result.Address.AddressLines
                        where !string.IsNullOrEmpty(ad.Line)
                        select ad.Line;
                var sb = new StringBuilder();
                foreach (var s in q)
                {
                    if (sb.Length > 0)
                        sb.Append(";");
                    sb.Append(s);
                }
                var m = Regex.Match(sb.ToString(), @"(?<line1>[^;]*)(;(?<line2>[^;]*))*;(?<city>.*)\s(?<st>[^ ]+)\s+(?<zip>\d{5}(-\d{4})?)");
                var a = new AddressResult
                {
                    found = true,
                    Line1 = m.Groups["line1"].Value,
                    Line2 = m.Groups["line2"].Value,
                    City = m.Groups["city"].Value,
                    State = m.Groups["st"].Value,
                    Zip = m.Groups["zip"].Value
                };
                string lab = a.Line1;
                if (a.Line2.HasValue())
                    lab += "\n" + a.Line2;
                lab += "\n" + Util.FormatCSZ4(a.City, a.State, a.Zip);
                a.address = lab;

                return a;
            }
            return new AddressResult { found = false };
        }
    }
}
