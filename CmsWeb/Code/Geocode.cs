/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Net;
using System.Web.UI;
using CmsData;
using System.Linq;
using UtilityExtensions;
using System.Configuration;

namespace CMSWeb
{

    public static class Geocoding
    {
        private class Coordinate
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }
        public static string GoogleAPIKey
        {
            get { return ConfigurationManager.AppSettings["GoogleMapsKey"]; }
        }
        private static Uri GetGeocodeUri(string address)
        {
            address = HttpUtility.UrlEncode(address);
            return new Uri(String.Format("{0}{1}&output={2}&key={3}", 
                "http://maps.google.com/maps/geo?q=", address, "csv", GoogleAPIKey));
        }
        public static GeoCode GetGeocode(string address)
        {
            var client = new WebClient();
            var uri = GetGeocodeUri(address);
            //status code, accuracy, latitude, longitude
            var a = client.DownloadString(uri).Split(',');
            return new GeoCode
            {
                Address = address,
                Latitude = Convert.ToDouble(a[2]),
                Longitude = Convert.ToDouble(a[3])
            };
        }
        public static string GetMapWithPushPins(this IEnumerable<OrganizationMember> q)
        {
            var q2 = from om in q
                     let addr = om.Person.PrimaryAddress + ";" 
                        + Util.FormatCSZ4(om.Person.PrimaryCity, 
                                om.Person.PrimaryState, 
                                om.Person.PrimaryZip)
                     join gc in DbUtil.Db.GeoCodes on addr equals gc.Address into g
                     from geocode in g.DefaultIfEmpty() // left outer join
                     select new { geocode, addr };

            var sb = new StringBuilder();
            var list = new List<GeoCode>();
            foreach (var i in q2)
            {
                var gc = i.geocode;
                if (gc == null)
                {
                    gc = list.SingleOrDefault(g => g.Address == i.addr);
                    if (gc == null)
                        gc = DbUtil.Db.GeoCodes.SingleOrDefault(g => g.Address == i.addr);
                    if (gc == null)
                    {
                        // fetch a new geocode
                        gc = GetGeocode(i.addr);
                        list.Add(gc);
                    }
                    else if (gc.Latitude == 0)
                    {
                        var g = GetGeocode(i.addr);
                        gc.Latitude = g.Latitude;
                        gc.Longitude = g.Longitude;
                    }
                }
                if (gc.Latitude != 0) // if we have a valid geocode
                    sb.AppendFormat("{0},{1}|", gc.Latitude, gc.Longitude);
            }
            if (list.Count > 0)
                DbUtil.Db.GeoCodes.InsertAllOnSubmit(list);
            DbUtil.Db.SubmitChanges();

            if (sb.Length == 0)
                return VirtualPathUtility.ToAppRelative("~/images/EmptyMap.png", 
                    HttpContext.Current.Request.ApplicationPath);

            sb.Length = sb.Length - 1;
            sb.Insert(0, "http://maps.google.com/staticmap?size=512x512&markers=");
            sb.Append("&format=png32&key=" + GoogleAPIKey);
            return sb.ToString();
        }
    }
}
