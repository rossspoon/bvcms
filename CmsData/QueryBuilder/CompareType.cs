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
        IsNull,
        IsNotNull,
        AllTrue,
        AnyTrue,
        AllFalse,
        StrGreater,
        StrLess,
        StrLessEqual,
        StrGreaterEqual,
        AnyValue,
    }
}
