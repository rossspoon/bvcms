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
            if (args.Length > 0)
                CampusId = int.Parse(args[0]);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
        public static int? CampusId { get; set; }
        public static string CampusArg
        {
            get
            {
                if (CampusId.HasValue)
                    return string.Format("?campus={0}",CampusId);
                return "";
            }
        }
    }
}
