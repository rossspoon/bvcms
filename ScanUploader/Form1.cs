/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using TwainLib;
using GdiPlusLib;
using System.Drawing.Imaging;
using System.Net;
using System.Configuration;
using System.Collections.Specialized;
using System.ServiceModel;

namespace ScanUploader
{
    public partial class Form1 : Form, IMessageFilter
    {
        cmsws.WebServiceSoapClient wsvc = new cmsws.WebServiceSoapClient();
        cmsws.ServiceAuthHeader header = new cmsws.ServiceAuthHeader();

        public Form1()
        {
            InitializeComponent();
            tw = new Twain();
            tw.Init(this.Handle);

            var f = new Signin();
            f.ShowDialog();
            header.Username = f.Username;
            header.Password = f.Password;
#if DEBUG
#else
            wsvc.Endpoint.Address = new EndpointAddress(
                ConfigurationSettings.AppSettings["serviceUrl"]);
#endif
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void EndingScan()
        {
            if (msgfilter)
            {
                Application.RemoveMessageFilter(this);
                msgfilter = false;
                this.Enabled = true;
                this.Activate();
            }
        }
        private bool msgfilter;
        private Twain tw;

        public bool PreFilterMessage(ref Message m)
        {
            var cmd = tw.PassMessage(ref m);
            if (cmd == TwainCommand.Not)
                return false;

            switch (cmd)
            {
                case TwainCommand.CloseRequest:
                    EndingScan();
                    tw.CloseSrc();
                    break;
                case TwainCommand.CloseOk:
                    EndingScan();
                    tw.CloseSrc();
                    break;
                case TwainCommand.DeviceEvent:
                    break;
                case TwainCommand.TransferReady:
                    var img = tw.TransferPictures()[0];
                    EndingScan();
                    tw.CloseSrc();
                    this.BackgroundImage = Gdip.GetBitmap(img);
                    var bits = ConvertImageToByteArray(this.BackgroundImage, ImageFormat.Jpeg);
                    var qs = new NameValueCollection();
                    wsvc.UploadImage(header, null, UserInfo.Text, (int)TypeImage.Tag, "image/jpeg", bits);
                    break;
            }
            return true;
        }
        private static byte[] ConvertImageToByteArray(Image image, ImageFormat format)
        {
            byte[] b;
            try
            {

                using (var ms = new MemoryStream())
                {
                    image.Save(ms, format);
                    b = ms.ToArray();
                }
            }
            catch (Exception) { throw; }
            return b;
        } 

        private void ConfigScanner_Click(object sender, EventArgs e)
        {
            tw.Acquire(true);
        }

        private void AcquireImage_Click(object sender, EventArgs e)
        {
            if (TypeImage.Text.Contains("Not Specified"))
            {
                MessageBox.Show("choose a type");
                return;
            }
            if (!msgfilter)
            {
                this.Enabled = false;
                msgfilter = true;
                Application.AddMessageFilter(this);
            }
            tw.Acquire(false);
        }

        private void SelectScanner_Click(object sender, EventArgs e)
        {
            tw.Select();
        }

        private void vBSRegistrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TypeImage.Text = "Type: VBSReg";
            TypeImage.Tag = 1;
        }

        private void volunteerAppToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TypeImage.Text = "Type: VolApp";
            TypeImage.Tag = 3;
        }

        private void Search_Click(object sender, EventArgs e)
        {

        }

        //public void UploadBytes(byte[] data, string url,
        //    string file, string contenttype, NameValueCollection nv,
        //    CookieContainer cookies)
        //{
        //    if (string.IsNullOrEmpty(file))
        //        file = "file";

        //    if (string.IsNullOrEmpty(contenttype))
        //        contenttype = "application/octet-stream";

        //    var qs = "?";
        //    if (nv != null)
        //        foreach (string key in nv.Keys)
        //            qs += key + "=" + nv.Get(key) + "&";
        //    var site = ConfigurationSettings.AppSettings["serviceUrl"];
        //    var uri = new Uri(site + url + qs);

        //    string boundary = "----------" + DateTime.Now.Ticks.ToString("x");

        //    var req = WebRequest.Create(uri) as HttpWebRequest;
        //    req.CookieContainer = cookies;
        //    req.ContentType = "multipart/form-data; boundary=" + boundary;
        //    req.Method = "POST";

        //    // Build up the post message header

        //    var sb = new StringBuilder();
        //    sb.Append("--");
        //    sb.Append(boundary);
        //    sb.Append("\r\n");
        //    sb.Append("Content-Disposition: form-data; name=\"file\"; filename=\"file\"\r\n");
        //    sb.Append("Content-Type: ");
        //    sb.Append(contenttype);
        //    sb.Append("\r\n");
        //    sb.Append("\r\n");

        //    var postHeaderBytes = Encoding.UTF8.GetBytes(sb.ToString());

        //    // Build the trailing boundary string as a byte array
        //    // ensuring the boundary appears on a line by itself
        //    var boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

        //    req.ContentLength = postHeaderBytes.Length + data.Length + boundaryBytes.Length;
        //    var rs = req.GetRequestStream();
        //    rs.Write(postHeaderBytes, 0, postHeaderBytes.Length);
        //    rs.Write(data, 0, data.Length);
        //    rs.Write(boundaryBytes, 0, boundaryBytes.Length);

        //    var s = req.GetResponse().GetResponseStream();
        //    var sr = new StreamReader(s);

        //    var res = sr.ReadToEnd();
        //}

        private void recRegToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TypeImage.Text = "Type: RecReg";
            TypeImage.Tag = 2;
        }
    }
}
