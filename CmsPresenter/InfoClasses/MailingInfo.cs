using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace CMSPresenter
{
    public class MailingInfo : TaggedPersonInfo
    {
        public String LabelName { get; set; }
        public String LastName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }
}
