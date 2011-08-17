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
        [NonSerialized]
        private Dictionary<int, RegSettings> _settings;
        public Dictionary<int, RegSettings> settings
        {
            get
            {
                if (_settings == null)
                    _settings = HttpContext.Current.Items["RegSettings"] as Dictionary<int, RegSettings>;
                return _settings;
            }
        }
        public bool RequiredAddr()
        {
            if (org != null)
                return setting.NotReqAddr == false;
            return !settings.Values.Any(o => o.NotReqAddr);
        }
        public bool RequiredDOB()
        {
            if (ComputesOrganizationByAge())
                return true;
            if (org != null)
                return setting.NotReqDOB == false;
            return !settings.Values.Any(i => i.NotReqDOB);
        }
        public bool RequiredZip()
        {
            if (org != null)
                return setting.NotReqZip == false;
            return !settings.Values.Any(o => o.NotReqZip);
        }
        public bool RequiredMarital()
        {
            if (org != null)
                return setting.NotReqMarital == false;
            return !settings.Values.Any(o => o.NotReqMarital);
        }
        public bool RequiredGender()
        {
            if (org != null)
                return setting.NotReqGender == false;
            return !settings.Values.Any(o => o.NotReqGender);
        }
        public bool RequiredPhone()
        {
            if (org != null)
                return setting.NotReqPhone == false;
            return !settings.Values.Any(o => o.NotReqPhone);
        }
    }
}