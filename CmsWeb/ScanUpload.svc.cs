using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using CmsData;

namespace CMSWeb
{
    // NOTE: If you change the class name "ScanUpload" here, you must also update the reference to "ScanUpload" in Web.config.
    public class ScanUpload : IScanUpload
    {
        public void UploadVBSApp(int? PeopleId, string UserInfo, int TypeId, string mimetype, byte[] bits)
        {
            var Db = DbUtil.Db;
            if (TypeId == 1)
            {
                var vb = new VBSApp();
                vb.UserInfo = UserInfo;
                vb.PeopleId = PeopleId;
                Db.VBSApps.InsertOnSubmit(vb);
                vb.ImgId = InsertImage(mimetype, bits);
                vb.Uploaded = DateTime.Now;
            }
            Db.SubmitChanges();
        }
        public void UploadRecApp(int? PeopleId, string UserInfo, int TypeId, string mimetype, byte[] bits)
        {
            throw new NotImplementedException();
        }
        private static int? InsertImage(string mimetype, byte[] bits)
        {
            var img = new ImageData.Image();
            switch (mimetype)
            {
                case "image/jpeg":
                case "image/pjpeg":
                case "image/gif":
                    return ImageData.Image.NewImageFromBits(bits).Id;
                case "application/pdf":
                case "application/msword":
                case "application/vnd.ms-excel":
                    return ImageData.Image.NewImageFromBits(bits, mimetype).Id;
            }
            return null;
        }
    }
}
