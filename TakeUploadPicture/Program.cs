using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO.Pipes;
using System.IO;

namespace TakeUploadPicture
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length == 1)
            {
                var arg = args[0];
                if(arg.StartsWith("tkup:"))
                    arg = arg.Substring(5);
                var a = arg.Split(',');
                PeopleId = int.Parse(a[0]);
                Guid = a[1];
                if (a.Length > 2)
                    Host = a[2];
                else
                    Host = "http://cms.bellevue.org/";
                Application.Run(new StandAloneForm());
            }
        }
        public static int PeopleId { get; set; }
        public static string Guid { get; set; }
        public static string Host { get; set; }
    }
}