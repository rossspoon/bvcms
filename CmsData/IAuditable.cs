using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CmsData
{
    public interface IAuditable
    {
        int CreatedBy { get; set; }
        DateTime CreatedDate { get; set; }
        int? ModifiedBy { get; set; }
        DateTime? ModifiedDate { get; set; }
    }
}