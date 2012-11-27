using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using UtilityExtensions;

namespace CmsWeb.MobileAPI
{
    public class MobileTaskStatus
    {
        public int id = 0;
        public string code = "";
        public string description = "";

        public MobileTaskStatus populate(CmsData.TaskStatus t)
        {
            id = t.Id;
            code = t.Code;
            description = t.Description;

            return this;
        }
    }
}