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
            get { return "/People/Person/Image/{0}?v={1}&s=1".Fmt(ThumbId ?? -1, CreatedDate.HasValue ? CreatedDate.Value.Ticks : 0); }
        }
        public string SmallUrl
        {
            get { return "/People/Person/Image/{0}?v={1}&s=2".Fmt(SmallId ?? -2, CreatedDate.HasValue ? CreatedDate.Value.Ticks : 0); }
        }
    }
}
