using System;
using System.Collections.Generic;
using System.Linq;
using UtilityExtensions;
using System.Text;

namespace CmsData
{
    public partial class Division
    {
        private CMSDataContext _Db;
        public CMSDataContext Db
        {
            get
            {
                if (_Db == null)
                    _Db = this.GetDataContext() as CMSDataContext;
                return _Db;
            }
        }
        public bool ToggleTag(int progid)
        {
            var pd = ProgDivs.SingleOrDefault(d => d.ProgId == progid);
            if (pd == null)
            {
                ProgDivs.Add(new ProgDiv { ProgId = progid });
                return true;
            }
            ProgDivs.Remove(pd);
            if (ProgId == progid)
                ProgId = null;
            Db.ProgDivs.DeleteOnSubmit(pd);
            return false;
        }
    }
}
