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
using ScanUploader.cmsweb;
using System.ServiceModel;
using System.Configuration;

namespace ScanUploader
{
    public partial class Form1 : Form, IMessageFilter
    {
        cmsSoapClient ws = new cmsSoapClient();
        ServiceAuthHeader header = new ServiceAuthHeader();

        public Form1()
        {
            InitializeComponent();
            tw = new Twain();
            tw.Init(this.Handle);
            header.Username = "bvcms";
            header.Password = "bvcms";
#if DEBUG
#else
#endif
            ws.Endpoint.Address = new EndpointAddress(
                ConfigurationSettings.AppSettings["serviceUrl"]);

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
                    ws.UploadImage(header, null, UserInfo.Text, 1, "image/jpeg", bits);
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
            TypeImage.Text = "Type: VBS Registration";
        }

        private void volunteerAppToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TypeImage.Text = "Type: Volunteer App";
        }

        private void Search_Click(object sender, EventArgs e)
        {

        }
    }
}
