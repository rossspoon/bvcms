using System;
using System.Windows.Forms;
using System.IO;
using CmsCheckin.Classes;

namespace CmsCheckin
{
    public partial class PrintingServer : Form
    {
        private DateTime dtLastPrint;
        private const int INT_count = 10;

        public PrintingServer()
        {
            InitializeComponent();
        }

        private void PrintingServer_Load(object sender, EventArgs e)
        {
            timer1 = new Timer();
            timer1.Interval = 1000;
            timer1.Tick += new EventHandler(timer1_Tick);
            count = INT_count;
            timer1.Start();
        }

        private void CheckNow_Click(object sender, EventArgs e)
        {
            CheckServer();
        }

        private void CheckServer()
        {
            int iLabelSize = PrinterHelper.getPageHeight(Program.Printer);

            timer1.Stop();
            Countdown.Text = "Checking...";
            Refresh();
            var pj = Util.FetchPrintJob();
            if (pj.jobs.Count > 0)
            {
                dtLastPrint = DateTime.Now;

                foreach (var j in pj.jobs)
                {
                    Program.SecurityCode = j.securitycode;

					if (Program.UseNewLabels)
					{
						foreach (var e in j.list) { e.securitycode = Program.SecurityCode; }
						PrinterHelper.doPrinting(j.list);
					}
					else
					{
						var doprint = new DoPrinting();
						var ms = new MemoryStream();
                        if (iLabelSize >= 170 && iLabelSize <= 230)
							doprint.PrintLabels2(ms, j.list);
						else
							doprint.PrintLabels(ms, j.list);
						doprint.FinishUp(ms);
					}
                }
            }
            count = INT_count;
            timer1.Start();
        }
        int count = 0;
        Timer timer1;
        void timer1_Tick(object sender, EventArgs e)
        {
            if (count == 0)
                CheckServer();
            else
            {
                Countdown.Text = count.ToString();
                Refresh();
                count--;
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            const int WM_KEYDOWN = 0x100;
            const int WM_SYSKEYDOWN = 0x104;

            if ((msg.Msg == WM_KEYDOWN) || (msg.Msg == WM_SYSKEYDOWN))
            {
                switch (keyData)
                {
                    case Keys.Space:
                    case Keys.Return:
                        CheckServer();
                        break;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
