using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using CmsData;
using System.Web.Mvc;
using System.Text;
using UtilityExtensions;
using System.Net;
using System.Xml.Linq;

namespace CmsWeb.Models
{
    public class SGMapModel
    {
        public int divid { get; set; }
        public SGMapModel(int id)
        {
            divid = id;
        }
        public class SGInfo
        {
            public string desc { get; set; }
            public string addr { get; set; }
            public string name { get; set; }
            public DateTime? schedule { get; set; }
            public string cmshost { get; set; }
            public int id { get; set; }
            public GeoCode gc { get; set; }
        }
        public class MarkerInfo
        {
            public string title { get; set; }
            public string html { get; set; }
            public double latitude { get; set; }
            public double longitude { get; set; }
        }
        public IEnumerable<MarkerInfo> Locations()
        {
            var q = from o in DbUtil.Db.Organizations
                    where o.Location != null && o.Location != ""
                    where o.DivOrgs.Any(dd => dd.DivId == divid) || o.DivisionId == divid
                    join gc in DbUtil.Db.GeoCodes on o.Location equals gc.Address into g
                    from geocode in g.DefaultIfEmpty()
                    select new SGInfo
                    {
                        desc = o.OrganizationName, //o.Description,
                        addr = o.Location,
                        name = o.OrganizationName,
                        schedule = o.OrgSchedules.First().MeetingTime,
                        cmshost = DbUtil.Db.CmsHost,
                        id = o.OrganizationId,
                        gc = geocode,
                    };
            var qlist = q.ToList();
            var addlist = new List<GeoCode>();
            var ret = new List<MarkerInfo>();

            foreach (var i in qlist.Where(ii => ii.gc == null))
            {
                i.gc = addlist.SingleOrDefault(g => g.Address == i.addr);
                if (i.gc == null)
                {
                    i.gc = GetGeocode(i.addr);
                    addlist.Add(i.gc);
                }
            }
            if (addlist.Count > 0)
                DbUtil.Db.GeoCodes.InsertAllOnSubmit(addlist);
            DbUtil.Db.SubmitChanges();

            string template = @"
<div>
{0}<br />
{1:ddd h:mm tt}<br />
<a href='{2}OnlineReg/Index/{3}' target='_top'>Signup</a>
</div>";
            return from i in qlist
                   where i.gc.Latitude != 0
                   select new MarkerInfo
                   {
                       html = template.Fmt(i.desc, i.schedule, i.cmshost, i.id),
                       latitude = i.gc.Latitude,
                       longitude = i.gc.Longitude,
                   };
        }
        public StringBuilder sb = new StringBuilder();
        private GeoCode GetGeocode(string address)
        {
            var wc = new WebClient();
            var uaddress = HttpUtility.UrlEncode(address);
            var uri = new Uri("http://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false".Fmt(uaddress));
            var xml = wc.DownloadString(uri);
            var xdoc = XDocument.Parse(xml);
            var status = xdoc.Descendants("status").Single().Value;
            if (status == "ZERO_RESULTS")
                return new GeoCode { Address = address };
            try
            {
                var loc = xdoc.Document.Descendants("location");
                var lat = Convert.ToDouble(loc.Descendants("lat").First().Value);
                var lng = Convert.ToDouble(loc.Descendants("lng").First().Value);
                return new GeoCode
                {
                    Address = address,
                    Latitude = lat,
                    Longitude = lng,
                };
            }
            catch (Exception ex)
            {
                sb.AppendLine(address);
                sb.AppendLine(status);
                sb.Append(ex.Message);
                return new GeoCode { Address = address };
            }
        }
    }
}
