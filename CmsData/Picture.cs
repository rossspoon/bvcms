using System;
using System.Collections.Generic;
using System.Linq;
using UtilityExtensions;
using System.Text;

namespace CmsData
{
    public partial class Picture
    {
        public string ThumbUrl
        {
            get { return "/Person2/Image/1/{0}/{1}".Fmt(ThumbId ?? -1, CreatedDate.HasValue ? CreatedDate.Value.Ticks : 0); }
        }
        public string SmallUrl
        {
            get { return "/Person2/Image/2/{0}/{1}".Fmt(SmallId ?? -2, CreatedDate.HasValue ? CreatedDate.Value.Ticks : 0); }
        }
    }
}
