using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Configuration;
using System.IO;
using System.Net;


namespace CmsCheckin
{
    class MemberList
    {
        public static void PrintList(string orgid)
        {

            Uri url;
            string str;
            XDocument x;
            url = new Uri(new Uri(Util.ServiceUrl()),
                string.Format("Checkin/Class/{0}", Util.GetDigits(orgid)));
            var wc = new WebClient();
            str = wc.DownloadString(url + Program.QueryString);
            if (string.IsNullOrEmpty(str))
                return;
            x = XDocument.Parse(str);
            var list = x.Root.Descendants("Name").Select(m => m.Value).ToList();
            var n = list.Count / 5;
            if (n % 5 > 0)
                n++;
            n = n * 5 - 1;
            for (; n > 0; n -= 5)
                PrintLabel5(list, n);

            list = new List<string>();
            list.Add(x.Root.Attribute("Name").Value);
            list.Add(x.Root.Attribute("Teacher").Value);
            list.Add(x.Root.Attribute("Date").Value);
            list.Add(x.Root.Attribute("Time").Value);
            list.Add("Count: " + x.Root.Attribute("Count").Value);
            PrintLabel5(list, 4);

            PrintBlankLabel();
        }
        static void PrintLabel5(List<string> list, int n)
        {
            string printer = ConfigurationSettings.AppSettings["PrinterName"];
            if (!RawPrinterHelper.HasPrinter(printer))
                return;
            var memStrm = new MemoryStream();
            var sw = new StreamWriter(memStrm);
            sw.WriteLine("\x02n");
            sw.WriteLine("\x02M0500");
            sw.WriteLine("\x02O0220");
            sw.WriteLine("\x02V0");
            sw.WriteLine("\x02SG");
            sw.WriteLine("\x02d");
            sw.WriteLine("\x01D");
            sw.WriteLine("\x02L");
            sw.WriteLine("D11");
            sw.WriteLine("PG");
            sw.WriteLine("pC");
            sw.WriteLine("SG");
            sw.WriteLine("ySPM");
            sw.WriteLine("A2");
            if (list.Count > n)
                sw.WriteLine("1911A1000040010" + list[n]);
            n--;
            if (list.Count > n)
                sw.WriteLine("1911A1000210010" + list[n]);
            n--;
            if (list.Count > n)
                sw.WriteLine("1911A1000370010" + list[n]);
            n--;
            if (list.Count > n)
                sw.WriteLine("1911A1000540010" + list[n]);
            n--;
            if (list.Count > n)
                sw.WriteLine("1911A1000700010" + list[n]);
            n--;
            sw.WriteLine("Q0001");
            sw.WriteLine("E");
            sw.Flush();

            memStrm.Position = 0;
            RawPrinterHelper.SendDocToPrinter(printer, memStrm);
            sw.Close();
        }
        private static void PrintBlankLabel()
        {
            string printer = ConfigurationSettings.AppSettings["PrinterName"];
            if (!RawPrinterHelper.HasPrinter(printer))
                return;
            var ms = new MemoryStream();
            var st = new StreamWriter(ms);
            st.WriteLine("\x02L");
            st.WriteLine("H07");
            st.WriteLine("D11");
            st.WriteLine("E");
            st.Flush();
            ms.Position = 0;
            RawPrinterHelper.SendDocToPrinter(printer, ms);
            st.Close();
        }

    }
}
