using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using UtilityExtensions;

namespace CmsWeb.Models
{
    public partial class OnlineRegPersonModel
    {
        public class NotRequiredFields
        {
            public bool NotReqAddr { get; set; }
            public bool NotReqDOB { get; set; }
            public bool NotReqZip { get; set; }
            public bool NotReqMarital { get; set; }
            public bool NotReqGender { get; set; }
            public bool NotReqPhone { get; set; }
        }
        private List<NotRequiredFields> _notreq;
        public static List<NotRequiredFields> BuildNotReq(int? divid)
        {
            var q = from o in DbUtil.Db.Organizations
                    where o.DivOrgs.Any(di => di.DivId == divid)
                    select new NotRequiredFields
                     {
                         NotReqAddr = o.NotReqAddr ?? false,
                         NotReqDOB = o.NotReqDOB ?? false,
                         NotReqGender = o.NotReqGender ?? false,
                         NotReqMarital = o.NotReqMarital ?? false,
                         NotReqZip = o.NotReqZip ?? false,
                         NotReqPhone = o.NotReqPhone ?? false,
                     };
            return q.ToList();
        }
        public List<NotRequiredFields> notreq
        {
            get
            {
                if (_notreq == null)
                    _notreq = BuildNotReq(divid);
                return _notreq;
            }
        }
        public bool RequiredAddr()
        {
            if (org != null)
                return !(org.NotReqAddr ?? false);
            return !notreq.Any(o => o.NotReqAddr);
        }
        public bool RequiredDOB()
        {
            if (ComputesOrganizationByAge())
                return true;
            if (org != null)
                return !(org.NotReqDOB ?? false);
            return !notreq.Any(i => i.NotReqDOB);
        }
        public bool RequiredZip()
        {
            if (org != null)
                return !(org.NotReqZip ?? false);
            return !notreq.Any(o => o.NotReqZip);
        }
        public bool RequiredMarital()
        {
            if (org != null)
                return !(org.NotReqMarital ?? false);
            return !notreq.Any(o => o.NotReqMarital);
        }
        public bool RequiredGender()
        {
            if (org != null)
                return !(org.NotReqGender ?? false);
            return !notreq.Any(o => o.NotReqGender);
        }
        public bool RequiredPhone()
        {
            //Person fp = null;
            //switch(whatfamily)
            //{
            //    case 1:
            //        fp = model.user;
            //        break;
            //    case 2:
            //        if (model.
            //            fp = DbUtil.Db.LoadPersonById(prevpid.Value);
            //        break;
            //}
            if (org != null)
                return !(org.NotReqPhone ?? false);
            return !notreq.Any(o => o.NotReqPhone);
                

        }
    }
}