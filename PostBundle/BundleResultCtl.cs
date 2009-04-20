using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PostBundle
{
    public partial class BundleResultCtl : UserControl
    {
        public BundleResultCtl(CmsWs.BundleResult b)
        {
            InitializeComponent();
            bundle = b;
        }
        private string DisplayInfo
        {
            get
            {
                return string.Format("{0:d}\n{1:C}\n{2}", _bundle.Date, _bundle.Total, _bundle.Fund);
            }
        }
        private CmsWs.BundleResult _bundle;
        public CmsWs.BundleResult bundle
        {
            get { return _bundle; }
            set
            {
                _bundle = value;
                linkLabel1.Text = value.BundleId.ToString();
                label1.Text = DisplayInfo;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Globals.Sheet1.BundleId.Value = bundle.BundleId;
            Globals.Sheet1.BundleFund.Value = bundle.Fund;
            Globals.Sheet1.BundleTotal.Value = bundle.Total;
            Globals.Sheet1.BundleDate.Value = bundle.Date;
        }
    }
}
