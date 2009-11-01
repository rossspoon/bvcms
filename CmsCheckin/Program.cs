using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CmsCheckin
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var f = new StartUp();
            f.ShowDialog();
            CampusId = f.CampusId;
            ThisDay = f.DayOfWeek;
            f.Dispose();
            Application.Run(new Form1());
        }
        public static int? CampusId { get; set; }
        public static int? ThisDay { get; set; }
        public static string QueryString
        {
            get
            {
                return string.Format("?campus={0}&thisday={1}", CampusId, ThisDay);
            }
        }
    }
}
