using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using System.Text;
using UtilityExtensions;
using System.Web.Mvc;
using System.Xml.Linq;

namespace CmsWeb.Models
{
    public class SlotModel
    {
        public SlotModel(int pid, int oid)
        {
            person = DbUtil.Db.People.SingleOrDefault(pp => pp.PeopleId == pid);
            org = DbUtil.Db.Organizations.SingleOrDefault(oo => oo.OrganizationId == oid);
            var x = XDocument.Parse("");//org.MenuItems);
            var qr = from r in x.Descendants("row")
                     select new
                     {
                         id = r.Attribute("id").Value,
                         text = r.Attribute("text").Value
                     };
            rows = qr.ToDictionary(r => r.id, r => r.text);
            var qc = from c in x.Descendants("col")
                     select new
                     {
                         id = c.Attribute("id").Value,
                         text = c.Attribute("text").Value
                     };
            cols = qc.ToDictionary(c => c.id, c => c.text);
            var qs = from s in x.Descendants("slot")
                     select new Slot
                     {
                         col = s.Attribute("col").Value,
                         row = s.Attribute("row").Value,
                         name = Slot.Name(s, this),
                     };
            slots = qs.ToDictionary(s => s.name);

            var q = from om in org.OrganizationMembers
                    select new MemberInfo
                    { 
                        PeopleId = om.PeopleId, 
                        Name = om.Person.Name,
                        groups = (from g in om.OrgMemMemTags select g.MemberTag.Name).ToList()
                    };
            orglist = q.ToList();
        }
        public class MemberInfo
        {
            public int PeopleId {get; set;}
            public string Name {get; set;}
            public List<string> groups {get; set;}
        }
        public List<MemberInfo> orglist { get; set; }
        public Organization org { get; set; }
        public Person person { get; set; }

        public class Slot
        {
            public string col { get; set; }
            public string row { get; set; }
            public string name { get; set; }
            public static string Name(string row, string col, SlotModel m)
            {
                return m.cols[col] + ": " + m.rows[row];
            }
            public static string Name(XElement rx, SlotModel m)
            {
                return Name(rx.Attribute("row").Value, rx.Attribute("col").Value, m);
            }
        }
        public class SlotInfo
        {
            public Slot slot { get; set; }
            public string[] owners { get; set; }
            public bool Mine { get; set; }
            public string classAttr
            { 
                get 
                {
                    var c = "slot";
                    if (owners.Length > 0)
                    {
                        if (Mine)
                            c += " m";
                        else
                            c += " o";
                        if (owners.Length == 1)
                            c += "1";
                        else if (owners.Length > 3)
                            c += "3";
                        else
                            c += "2";
                    }
                    return c;
                } 
            }
            public string status
            {
                get
                {
                    if (Mine)
                        return "Yours";
                    return "Open";
                }
            }
            public string checkedstr
            {
                get
                {
                    if (Mine)
                        return "checked='checked'";
                    return "";
                }
            }
        }
        public class SlotRow
        {
            public IEnumerable<SlotInfo> slots { get; set; }
            public string rowtitle { get; set; }
        }
        public Dictionary<string, string> rows;
        public Dictionary<string, string> cols;
        private Dictionary<string, Slot> slots;
        public Slot findslot(string name)
        {
            if (!slots.ContainsKey(name))
                return null;
            return slots[name];
        }
        private List<SlotRow> slotlist;
        public int Count()
        {
            if (slotlist == null)
                FetchSlots();
            return slotlist.Sum(r => r.slots.Sum(c => c.owners.Length));
        }

        public List<SlotRow> FetchSlots()
        {
            if (slotlist == null)
            {
                var q2 = from r in rows
                         select new SlotRow
                         {
                             rowtitle = r.Value,
                             slots = from c in cols
                                     let name = Slot.Name(r.Key, c.Key, this)
                                     select NewSlot(name)
                         };
                slotlist = q2.ToList();
            }
            return slotlist;
        }
        public SlotInfo NewSlot(string name)
        {
            var si = new SlotInfo
            {
                slot = findslot(name),
                owners = (from om in orglist
                          where om.groups.Contains(name)
                          select om.Name).ToArray(),
                Mine = orglist.Any(om => om.PeopleId == person.PeopleId && om.groups.Contains(name))
            };
            return si;
        }
        public IEnumerable<string> MySlots()
        {
            var q = from g in DbUtil.Db.OrgMemMemTags
                    where g.OrgId == this.org.OrganizationId && g.PeopleId == this.person.PeopleId
                    select findslot(g.MemberTag.Name).name;
            return q;
        }
    }
}
