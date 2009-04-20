using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CmsData
{
    public interface ITaskNotify
    {
        void EmailNotification(Person from, Person to, string subject, string message);
    }
}
