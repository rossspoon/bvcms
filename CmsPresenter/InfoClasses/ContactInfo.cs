using System;
using UtilityExtensions;

namespace CMSPresenter.InfoClasses
{
    public class ContactInfo
    {
       public int ContactId { get; set; }
       public string Comments{ get; set; }
       public DateTime ContactDate{ get; set; }
       public string TypeOfContact { get; set; }
       public string ContactReason { get; set; }
       public string Program { get; set; }
       public string Teacher { get; set; }
    }
}
