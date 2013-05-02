using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;
using CmsData;
using CmsData.Classes.SmallGroupFinder;
using System.Collections;

namespace CmsWeb.Areas.Public.Models
{
    public class SmallGroupFinderModel
    {
        public const int TYPE_SETTING = 1;
        public const int TYPE_FILTER = 2;

        SmallGroupFinder sgf;

        public void load(string sName)
        {
            var xml = DbUtil.Content("SGF-" + sName + ".xml", "");

            var xs = new XmlSerializer(typeof(SmallGroupFinder), new XmlRootAttribute("Finder"));
            var sr = new StringReader(xml);
            sgf = (SmallGroupFinder)xs.Deserialize(sr);
        }

        public Division getDivision()
        {
            var d = (from e in DbUtil.Db.Divisions
                     where e.Id == sgf.divisionid
                     select e).SingleOrDefault();

            return d;
        }

        public int getCount(int type)
        {
            if (sgf == null) return 0;

            switch (type)
            {
                case TYPE_SETTING:
                {
                    return sgf.Settings.Count();
                }

                case TYPE_FILTER:
                {
                    return sgf.Filters.Count();
                }

                default: return 0;
            }
        }

        public CmsData.Classes.SmallGroupFinder.Setting getSetting(int id)
        {
            return sgf.Settings[id];
        }

        public CmsData.Classes.SmallGroupFinder.Filter getFilter(int id)
        {
            return sgf.Filters[id];
        }

        public List<FilterItem> getFilterItems(int id)
        {
            var f = sgf.Filters[id];
            List<FilterItem> i = new List<FilterItem>();

            if (f.locked)
            {
                i.Add(new FilterItem { value = f.lockedvalue });
            }
            else
            {
                //i = (from e in DbUtil.Db.OrganizationExtras
               //      where e.Field == f.name
                //     select 
            }

            return i;
        }
    }

    public class FilterItem
    {
        public string value;
    }
}