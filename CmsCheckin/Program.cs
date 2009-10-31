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
            if (args.Length == 1)
                CampusId = int.Parse(args[0]);
            for (var i = 0; i < args.Length; i += 2)
                if (args[i] == "-d")
                    ThisDay = int.Parse(args[i + 1]);
                else if (args[i] == "-c")
                    CampusId = int.Parse(args[i + 1]);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
        public static int? CampusId { get; set; }
        public static int? ThisDay { get; set; }
        public static string CampusArg
        {
            get
            {
                var args = String.Empty;
                if (CampusId.HasValue)
                    args = string.Format("campus={0}", CampusId);
                if (ThisDay.HasValue)
                {
                    if (args.Length > 0)
                        args += "&";
                    args += string.Format("thisday={0}", ThisDay);
                }
                return args;
            }
        }
    }
}
