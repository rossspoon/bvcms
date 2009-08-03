using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using CmsData;
using System.Web.Mvc;
using System.Text;
using System.Configuration;
using UtilityExtensions;
using System.Data.Linq.SqlClient;
using CMSPresenter;

namespace CMSWeb.Models
{
    public class PromotionSetupModel
    {
        public IEnumerable<Promotion> Promotions()
        {
            return DbUtil.Db.Promotions.OrderBy(p => p.Sort).ThenBy(p => p.Description);
        }
        public IEnumerable<SelectListItem> Programs()
        {
            var q = from c in DbUtil.Db.Programs
                    orderby c.BFProgram descending, c.Name
                    select new 
                    SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name,
                        Selected = (c.BFProgram ?? false) == true
                    };
            return q;
        }

    }
}
