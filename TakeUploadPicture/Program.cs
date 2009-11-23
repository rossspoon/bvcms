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
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool createdNew;
            var m = new Mutex(true, "TakeUploadPicture", out createdNew);

            if (!createdNew)
            {
                // see if we can find the other app and Bring it to front
                IntPtr hWnd = FindWindowByCaption(IntPtr.Zero, "Take Upload Picture");
                
                if (hWnd != IntPtr.Zero)
                {
                    var placement = new WINDOWPLACEMENT();
                    placement.length = Marshal.SizeOf(placement);
                    GetWindowPlacement(hWnd, ref placement);
                    if (placement.showCmd != SW_NORMAL)
                    {
                        placement.showCmd = SW_RESTORE;
                        SetWindowPlacement(hWnd, ref placement);
                    }
                    SetForegroundWindow(hWnd);

                    using (var pipeClient = new NamedPipeClientStream(".", "tkup", PipeDirection.Out))
                    {
                        pipeClient.Connect();
                        try
                        {
                            using (var sw = new StreamWriter(pipeClient))
                            {
                                sw.AutoFlush = true;
                                sw.WriteLine(args[0]);
                            }
                        }
                        catch (IOException e)
                        {
                            Console.WriteLine("ERROR: {0}", e.Message);
                        }
                    }
                }
                return;
            }

            if (args.Length == 1)
                PeopleId = int.Parse(args[0]);

            var f = new Signin();
            var r = f.ShowDialog();
            if (r == DialogResult.Cancel)
                return;
            header.Username = f.Username;
            header.Password = f.Password;

            var t = new Thread(Listener);
            t.Start();

            Application.Run(new StandAloneTestForm());
            // keep the mutex reference alive until the normal termination of the program
            GC.KeepAlive(m);
        }
        internal static cmsws.ServiceAuthHeader header = new cmsws.ServiceAuthHeader();
        static void Listener()
        {
            using (var pipeServer = new NamedPipeServerStream("tkup", PipeDirection.In))
            {
                pipeServer.WaitForConnection();
                try
                {
                    using (var sr = new StreamReader(pipeServer))
                    {
                        string temp;
                        while ((temp = sr.ReadLine()) != null)
                            PeopleId = int.Parse(temp.Split(':')[1]);
                    }
                }
                catch (IOException e)
                {
                    Console.WriteLine("ERROR: {0}", e.Message);
                }
            }
        }
        public static int PeopleId { get; set; }

        private const int SW_NORMAL = 1; // see WinUser.h for definitions
        private const int SW_RESTORE = 9;

        [DllImport("User32", EntryPoint = "FindWindow")]
        static extern IntPtr FindWindow(string className, string windowName);
        
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);
        
        [DllImport("User32", EntryPoint = "SendMessage")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("User32", EntryPoint = "SetForegroundWindow")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("User32", EntryPoint = "SetWindowPlacement")]
        private static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl);

        [DllImport("User32", EntryPoint = "GetWindowPlacement")]
        private static extern bool GetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl);
        
        private struct POINTAPI
        {
            public int x;
            public int y;
        }

        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        private struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public int showCmd;
            public POINTAPI ptMinPosition;
            public POINTAPI ptMaxPosition;
            public RECT rcNormalPosition;
        }
    }
}