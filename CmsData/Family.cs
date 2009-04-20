using System;
using System.Linq;
using UtilityExtensions;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;
using System.ComponentModel;

namespace CmsData
{
    public partial class Family : IAuditable
    {
        public string CityStateZip
        {
            get { return Util.FormatCSZ4(CityName,StateCode,ZipCode); }
        }
        public string AddrCityStateZip
        {
            get { return AddressLineOne + ";" + CityStateZip; }
        }
        public string Addr2CityStateZip
        {
            get { return AddressLineTwo + ";" + CityStateZip; }
        }


        public string HohName
        {
            get
            {
                if (HeadOfHouseholdId.HasValue) return People.Single(p => p.PeopleId == HeadOfHouseholdId.Value).Name;
                return "";
            }
        }

        public string HohSpouseName
        {
            get
            {
                if (HeadOfHouseholdSpouseId.HasValue) return People.Single(p => p.PeopleId == HeadOfHouseholdSpouseId.Value).Name;
                return "";
            }
        }
        public string FamilyName
        {
            get { return "The " + HohName + " Family"; }
        }

        public int MemberCount
        {
            get { return People.Count; }
        }
        private CMSDataContext _Db;
        public CMSDataContext Db
        {
            get
            {
                if (_Db == null)
                    _Db = this.GetDataContext() as CMSDataContext;
                return _Db;
            }
        }

        //#region Tags
        //private string _PublicTagString;
        //public string PublicTagString
        //{
        //    get
        //    {
        //        if (_PublicTagString == null)
        //            _PublicTagString = GetTagString(Util.PublicTagTypeId, null);
        //        return _PublicTagString;
        //    }
        //    set
        //    {
        //        AddRemoveTags(value, Util.PublicTagTypeId, null);
        //        _PrivateTagString = value;
        //    }
        //}

        //private string _PrivateTagString;
        //public string PrivateTagString
        //{
        //    get
        //    {
        //        if (_PrivateTagString == null)
        //            _PrivateTagString = GetTagString(Util.PersonalTagTypeId, Util.UserName);
        //        return _PrivateTagString;
        //    }
        //    set
        //    {
        //        AddRemoveTags(value, Util.PersonalTagTypeId, Util.UserName);
        //        _PrivateTagString = value;
        //    }
        //}
        //public bool ToggleTag()
        //{
        //    return ToggleTag(Util.CurrentTagName, Util.CurrentTagOwner, Util.PersonalTagTypeId);
        //}
        //public bool ToggleTag(string TagName, string TagOwner, int TagTypeId)
        //{
        //    var tag = Db.Tags.SingleOrDefault(t => t.Name == TagName && t.Owner == TagOwner);
        //    if (tag == null)
        //    {
        //        tag = new Tag { Owner = TagOwner, Name = TagName, TypeId = TagTypeId };
        //        Db.Tags.InsertOnSubmit(tag);
        //    }
        //    var tp = Tags.SingleOrDefault(t => t.Tag.Name == TagName && t.Tag.Owner == TagOwner);
        //    if (tp == null)
        //    {
        //        Tags.Add(new TagPerson { Tag = tag });
        //        return true;
        //    }
        //    Tags.Remove(tp);
        //    Db.TagPeople.DeleteOnSubmit(tp);
        //    return false;
        //}
        //private string GetTagString(int tagtype, string user)
        //{
        //    var sb = new StringBuilder();
        //    var q = from tp in Tags
        //            where tp.Tag.Owner == user && tp.Tag.TypeId == tagtype
        //            select tp;
        //    if (user.HasValue())
        //        q = q.Where(tp => tp.Tag.Owner == user);
        //    foreach (var tp in q)
        //        sb.Append(tp.Tag.Name + ";");
        //    if (sb.Length > 0)
        //        sb.Remove(sb.Length - 1, 1);
        //    return sb.ToString();
        //}
        //private void AddRemoveTags(string value, int tagtype, string user)
        //{
        //    var TagSet = Tags.Where(t => t.Tag.TypeId == tagtype);
        //    if (user.HasValue())
        //        TagSet = TagSet.Where(t => t.Tag.Owner == user);

        //    var tagnames = value.Split(';');

        //    var TagDeletes = TagSet.Where(t => !tagnames.Contains(t.Tag.Name));
        //    Db.TagPeople.DeleteAllOnSubmit(TagDeletes);

        //    var TagAdds = from tagname in tagnames
        //                  join t in TagSet on tagname equals t.Tag.Name into joined
        //                  from t in joined.DefaultIfEmpty()
        //                  where t == null
        //                  select tagname;

        //    foreach (var tagname in TagAdds)
        //    {
        //        var tag = Db.Tags.SingleOrDefault(t =>
        //            (t.Owner == user || !user.HasValue()) && t.Name == tagname && t.TypeId == tagtype);

        //        if (tag == null)
        //        {
        //            tag = new Tag { Name = tagname, Owner = user, TypeId = tagtype };
        //            Db.Tags.InsertOnSubmit(tag);
        //        }
        //        Tags.Add(new TagPerson { Tag = tag });
        //    }
        //}
        //#endregion
 
    }
}

