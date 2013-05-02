using System;
using System.Collections.Generic;
using System.Linq;
using UtilityExtensions;
using System.Text;
using System.Xml.Linq;
using System.Linq.Expressions;

namespace CmsData
{
    public enum CompareType
    {
        Equal,
        NotEqual,
        Greater,
        Less,
        LessEqual,
        GreaterEqual,
        Contains,
        DoesNotContain,
        StartsWith,
        DoesNotStartWith,
        EndsWith,
        DoesNotEndWith,
        OneOf,
        NotOneOf,
        AllTrue,
        AnyTrue,
        AllFalse,
        After,
        Before,
        BeforeOrSame,
        AfterOrSame,
        AnyValue,
        IsNull,
        IsNotNull,
    }
}
