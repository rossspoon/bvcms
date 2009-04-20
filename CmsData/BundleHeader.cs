using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CmsData
{
    public partial class BundleHeader
    {
        public enum StatusCode
        {
            Closed = 0,
            Open = 1,
        }
        public enum TypeCode
        {
            GenericEnvelope = 1,
            ChecksAndCash = 2,
            PreprintedEnvelope = 3,
            Online = 4,
        }
        public StatusCode StatusEnum
        {
            get { return (StatusCode)BundleStatusId; }
            set { BundleStatusId = (int)value; }
        }
        public TypeCode TypeEnum
        {
            get { return (TypeCode)BundleHeaderTypeId; }
            set { BundleHeaderTypeId = (int)value; }
        }
    }
}
