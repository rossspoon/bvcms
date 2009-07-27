using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CMSWeb
{
    // NOTE: If you change the interface name "IScanUpload" here, you must also update the reference to "IScanUpload" in Web.config.
    [ServiceContract]
    public interface IScanUpload
    {
        [OperationContract]
        void UploadVBSApp(int? PeopleId, string UserInfo, int TypeId, string mimetype, byte[] bits);
        [OperationContract]
        void UploadRecApp(int? PeopleId, string UserInfo, int TypeId, string mimetype, byte[] bits);
    }
}
