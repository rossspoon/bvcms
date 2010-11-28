using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Diagnostics;


namespace MassEmailer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if (!DEBUG)
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new MassEmailer() 
			};
            ServiceBase.Run(ServicesToRun);
#else
            if (!EventLog.SourceExists("MassEmailer"))
                EventLog.CreateEventSource("MassEmailer", "MassEmailerLog");

            var service = new MassEmailer();
            for(;;)
            {
                service.CheckQueue();
                Thread.Sleep(15000);
            }
#endif
        }
    }
}
