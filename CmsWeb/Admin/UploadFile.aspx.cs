using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using UtilityExtensions;

namespace CmsWeb.Admin
{
    public partial class UploadFile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
                try
                {
                    var fpath = Server.MapPath("~/Upload/" + Util.Host + "/");
                    if (!Directory.Exists(fpath))
                        Directory.CreateDirectory(fpath);
                    FileUpload1.SaveAs(fpath + FileUpload1.FileName);
                    var url = Util.ResolveServerUrl("/Upload/" + Util.Host + "/");
                    if (Path.GetExtension(FileUpload1.FileName) == ".zip")
                    {
                        string fn = Path.GetFileNameWithoutExtension(FileUpload1.FileName);
                        url += fn;
                        UnZipFiles(fpath + FileUpload1.FileName, fpath);
                    }
                    else
                        url += FileUpload1.PostedFile.FileName;

                    Label1.Text = "File name: " + url + "<br>" +
                             FileUpload1.PostedFile.ContentLength + " kb<br>" +
                             "Content type: " +
                             FileUpload1.PostedFile.ContentType;
                }
                catch (Exception ex)
                {
                    Label1.Text = "ERROR: " + ex.Message.ToString();
                }
            else
            {
                Label1.Text = "You have not specified a file.";
            }
        }
        public static void UnZipFiles(string zipPathAndFile, string outputFolder)
        {
            var s = new ZipInputStream(File.OpenRead(zipPathAndFile));
            ZipEntry theEntry;
            var tmpEntry = String.Empty;
            while ((theEntry = s.GetNextEntry()) != null)
            {
                var directoryName = outputFolder;
                var fileName = Path.GetFileName(theEntry.Name);
                if (directoryName != "")
                    Directory.CreateDirectory(directoryName);
                if (fileName != String.Empty)
                    if (theEntry.Name.IndexOf(".ini") < 0)
                    {
                        var fullPath = directoryName + "\\" + theEntry.Name;
                        fullPath = fullPath.Replace("\\ ", "\\");
                        string fullDirPath = Path.GetDirectoryName(fullPath);
                        if (!Directory.Exists(fullDirPath)) Directory.CreateDirectory(fullDirPath);
                        FileStream streamWriter = File.Create(fullPath);
                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
                            if (size > 0)
                                streamWriter.Write(data, 0, size);
                            else
                                break;
                        }
                        streamWriter.Close();
                    }
            }
            s.Close();
            File.Delete(zipPathAndFile);
        }
    }
}
