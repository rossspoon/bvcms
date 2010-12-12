using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMSPresenter
{
    public class SavedQueryInfo
    {
        public int QueryId { get; set; }
        public bool IsPublic { get; set; }
        public string User { get; set; }
        public string Description { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}