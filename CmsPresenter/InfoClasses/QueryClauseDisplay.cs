using System;
using CmsData;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using UtilityExtensions;
using System.Linq.Expressions;
using System.Web.UI.WebControls;

namespace CMSPresenter
{
    public class QueryClauseDisplay
    {
        public Unit Level { get; set; }
        public int Id
        {
            get { return Clause.QueryId; }
        }
        public string ClauseHtml
        {
            get { return Clause.ToString(); }
        }
        public QueryBuilderClause Clause;
    }
}
