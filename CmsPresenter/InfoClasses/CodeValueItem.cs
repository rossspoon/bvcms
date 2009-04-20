/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using CmsData;
using System.Data.Linq;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using UtilityExtensions;

namespace CMSPresenter
{
    public class CodeValueItem
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
        public string IdCode { get { return "{0},{1}".Fmt(Id, Code); } }
        public string IdValue { get { return "{0},{1}".Fmt(Id, Value); } }
        public override string ToString()
        {
            return "{0}: {1}, {2}".Fmt(Id, Code, Value);
        }
    }
    public class MemberTypeItem : CodeValueItem
    {
        public int AttendanceTypeId { get; set; }
    }
    public static class CodeValueItemUtil
    {
        public static string ItemValue(this List<CodeValueItem> list, int id)
        {
            var item = list.SingleOrDefault(i => i.Id == id);
            return item == null ? "(null)" : item.Value;
        }
        public static string ItemValue(this List<MemberTypeItem> list, int id)
        {
            var item = list.SingleOrDefault(i => i.Id == id);
            return item == null ? "(null)" : item.Value;
        }
    }
}