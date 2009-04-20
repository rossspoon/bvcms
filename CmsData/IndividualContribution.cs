using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CmsData
{
    public partial class Contribution
    {
        public enum StatusCode
        {
            Recorded = 0,
            Reversed = 1,
            Returned = 2,
        }
        public enum TypeCode
        {
            CheckCash = 1,
            BrokeredProperty = 3,
            GraveSite = 4,
            ReturnedCheck = 6,
            Reversed = 7,
            Pledge = 8,
        }
        public StatusCode StatusEnum
        {
            get { return (StatusCode)ContributionStatusId; }
            set { ContributionStatusId = (int)value; }
        }
        public TypeCode TypeEnum
        {
            get { return (TypeCode)ContributionTypeId; }
            set { ContributionTypeId = (int)value; }
        }
    }
}
