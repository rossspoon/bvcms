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
    public class Print
    {
        public static void MemberList(string orgid)
        {
            Uri url;
            string str;
            XDocument x;
            url = new Uri(new Uri(Util.ServiceUrl()),
                string.Format("Checkin2/Class/{0}", Util.GetDigits(orgid)));
            var wc = Util.CreateWebClient();
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

            BlankLabel(true);
        }
        private static void PrintLabel5(List<string> list, int n)
        {
            var memStrm = new MemoryStream();
            var sw = new StreamWriter(memStrm);
            if (Program.Printer.Contains("Datamax"))
            {
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
            }
            else if (Program.Printer.Contains("ZDesigner"))
            {
                sw.WriteLine(@"^XA~TA000~JSN^LT0^MNW^MTD^PON^PMN^LH0,0^JMA^PR2,2~SD15^JUS^LRN^CI0^XZ");
                sw.WriteLine(@"^XA");
                sw.WriteLine(@"^MMT");
                sw.WriteLine(@"^PW609");
                sw.WriteLine(@"^LL0406");
                sw.WriteLine(@"^LS0");
                if (list.Count > n)
                    sw.WriteLine(string.Format(@"^FT24,179^A0N,28,28^FH\^FD{0}^FS", list[n]));
                n--;
                if (list.Count > n)
                    sw.WriteLine(string.Format(@"^FT24,144^A0N,28,28^FH\^FD{0}^FS", list[n]));
                n--;
                if (list.Count > n)
                    sw.WriteLine(string.Format(@"^FT24,110^A0N,28,28^FH\^FD{0}^FS", list[n]));
                n--;
                if (list.Count > n)
                    sw.WriteLine(string.Format(@"^FT24,76^A0N,28,28^FH\^FD{0}^FS", list[n]));
                n--;
                if (list.Count > n)
                    sw.WriteLine(string.Format(@"^FT24,41^A0N,28,28^FH\^FD{0}^FS", list[n]));
                n--;
                sw.WriteLine(string.Format(@"^PQ1,0,1,Y^XZ"));
            }
            else
            {
                if (list.Count > n)
                    sw.WriteLine(list[n]);
                n--;
                if (list.Count > n)
                    sw.WriteLine(list[n]);
                n--;
                if (list.Count > n)
                    sw.WriteLine(list[n]);
                n--;
                if (list.Count > n)
                    sw.WriteLine(list[n]);
                n--;
                if (list.Count > n)
                    sw.WriteLine(list[n]);
                n--;
            }
            sw.Flush();

            memStrm.Position = 0;
            PrintRawHelper.SendDocToPrinter(Program.Printer, memStrm);
            sw.Close();
        }

        public static void BlankLabel(bool datamaxonly)
        {
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);
            if (Program.Printer.Contains("Datamax"))
            {
                sw.WriteLine("\x02L");
                sw.WriteLine("H07");
                sw.WriteLine("D11");
                sw.WriteLine("E");
                sw.Flush();
                ms.Position = 0;
                PrintRawHelper.SendDocToPrinter(Program.Printer, ms);
            }
            else if (!datamaxonly && Program.Printer.Contains("ZDesigner"))
            {
                sw.WriteLine("^XA~TA000~JSN^LT0^MNW^MTD^PON^PMN^LH0,0^JMA^PR2,2~SD15^JUS^LRN^CI0^XZ");
                sw.WriteLine("^XA");
                sw.WriteLine("^MMT");
                sw.WriteLine("^PW609");
                sw.WriteLine("^LL0406");
                sw.WriteLine("^LS0");
                sw.WriteLine("^PQ1,0,1,Y^XZ");
                sw.Flush();
                ms.Position = 0;
                PrintRawHelper.SendDocToPrinter(Program.Printer, ms);
            }
            sw.Close();
        }
        public static void LabelKiosk(LabelInfo li)
        {
            if (!Program.Printer.HasValue())
                return;
            var memStrm = new MemoryStream();
            var sw = new StreamWriter(memStrm);
            if (Program.Printer.Contains("ZDesigner"))
            {
                //sw.WriteLine("CT~~CD,~CC^~CT~");
                sw.WriteLine("^XA~TA000~JSN^LT0^MNW^MTD^PON^PMN^LH0,0^JMA^PR2,2~SD15^JUS^LRN^CI0^XZ");
                sw.WriteLine("^XA");
                sw.WriteLine("^MMT");
                sw.WriteLine("^PW609");
                sw.WriteLine("^LL0203");
                sw.WriteLine("^LS0");
                sw.WriteLine(string.Format(@"^FT592,122^A0I,79,79^FH\^FD{0}^FS", li.first));
                sw.WriteLine(string.Format(@"^FT583,69^A0I,34,33^FH\^FD{0}^FS", li.last));
                var al = "na";
                if (li.allergies.HasValue())
                    al = li.allergies;
                sw.WriteLine(string.Format(@"^FT585,26^A0I,23,24^FH\^FD{0}, ({1})^FS", li.org, al));
                sw.WriteLine(string.Format(@"^^FT133,65^A0I,51,50^FH\^FD{0}^FS", li.location));
                sw.WriteLine(string.Format("^PQ{0},0,1,Y^XZ", li.n));
            }
            else if (Program.Printer.Contains("Datamax"))
            {
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
                sw.WriteLine("1911A2400490006" + li.first);
                sw.WriteLine("1911A1200280008" + li.last);
                sw.WriteLine(string.Format("1911A0800050007{0}, ({1})", li.org, li.allergies));
                sw.WriteLine("1911A2400280190" + li.location);
                sw.WriteLine("Q" + li.n.ToString("0000"));
                sw.WriteLine("E");
            }
            else
            {
                sw.WriteLine(li.first);
                sw.WriteLine(li.last);
                sw.WriteLine(string.Format("{0}, ({1})", li.org, li.allergies));
                sw.WriteLine(li.location);
            }

            sw.Flush();

            memStrm.Position = 0;
            PrintRawHelper.SendDocToPrinter(Program.Printer, memStrm);
            sw.Close();
            BlankLabel(true);
        }
        public static int SecurityLabel(DateTime time, string code)
        {
            if (!Program.Printer.HasValue())
                return 0;
            var memStrm = new MemoryStream();
            var sw = new StreamWriter(memStrm);
            if (Program.Printer.Contains("ZDesigner"))
            {
                sw.WriteLine("^XA~TA000~JSN^LT0^MNW^MTD^PON^PMN^LH0,0^JMA^PR2,2~SD15^JUS^LRN^CI0^XZ");
                sw.WriteLine("^XA");
                sw.WriteLine("^MMT");
                sw.WriteLine("^PW609");
                sw.WriteLine("^LL0406");
                sw.WriteLine("^LS0");
                sw.WriteLine(string.Format(@"^FT81,115^A0N,73,72^FH\^FD{0}^FS", code));
                sw.WriteLine(string.Format(@"^FT82,170^A0N,34,33^FH\^FD{0:M/d/yy}^FS", time));
                sw.WriteLine(string.Format(@"^FT395,170^A0N,34,33^FH\^FD{0:M/d/yy}^FS", time));
                sw.WriteLine(string.Format(@"^FT397,115^A0N,73,72^FH\^FD{0}^FS", code));
                sw.WriteLine(@"^FO310,15^GB0,175,2^FS");
                sw.WriteLine("^PQ1,0,1,Y^XZ");
            }
            else if (Program.Printer.Contains("Datamax"))
            {
                sw.WriteLine("\x02n");
                sw.WriteLine("\x02M0500");
                sw.WriteLine("\x02O0220");
                sw.WriteLine("\x02V0");
                sw.WriteLine("\x02SG");
                sw.WriteLine("\x02d");
                sw.WriteLine("\x01D");
                sw.WriteLine("\x02ICAFgfx0");
                sw.WriteLine("0000FF98");
                sw.WriteLine("8001E0");
                sw.WriteLine("FFFF");
                sw.WriteLine("\x02L");
                sw.WriteLine("D11");
                sw.WriteLine("PG");
                sw.WriteLine("pC");
                sw.WriteLine("SG");
                sw.WriteLine("ySPM");
                sw.WriteLine("A2");
                sw.WriteLine("1911A2400360035" + code);
                sw.WriteLine("1911A1200050036" + time.ToString("M/d/yy"));
                sw.WriteLine("1911A1200050190" + time.ToString("M/d/yy"));
                sw.WriteLine("1911A2400360191" + code);
                sw.WriteLine("1Y1100000020148gfx0");
                sw.WriteLine("Q0001");
                sw.WriteLine("E");
            }
            else if (Program.Printer.Contains("Godex"))
            {
                sw.WriteLine("^Q51,3");
                sw.WriteLine("^W76");
                sw.WriteLine("^H10");
                sw.WriteLine("^P1");
                sw.WriteLine("^S4");
                sw.WriteLine("^AD");
                sw.WriteLine("^C1");
                sw.WriteLine("^R0");
                sw.WriteLine("~Q+0");
                sw.WriteLine("^O0");
                sw.WriteLine("^D0");
                sw.WriteLine("^E12");
                sw.WriteLine("~R200");
                sw.WriteLine("^L");
                sw.WriteLine("Dy2-me-dd");
                sw.WriteLine("Th:m:s");
                sw.WriteLine("AH,68,78,1,1,0,0," + code);
                sw.WriteLine("AH,376,78,1,1,0,0," + code);
                sw.WriteLine("Lo,296,36,303,379");
                sw.WriteLine("AE,74,174,1,1,0,0,{0:M/d/yy}".Fmt(time));
                sw.WriteLine("AE,380,174,1,1,0,0,{0:M/d/yy}".Fmt(time));
                sw.WriteLine("E");
            }
            sw.Flush();
            memStrm.Position = 0;
            PrintRawHelper.SendDocToPrinter(Program.Printer, memStrm);
            sw.Close();
            return 1;
        }
        public static int Label(LabelInfo li, int nlabels, string code)
        {
            if (nlabels <= 0 || !Program.Printer.HasValue())
                return 0;
            var memStrm = new MemoryStream();
            var sw = new StreamWriter(memStrm);
            if (Program.Printer.Contains("ZDesigner"))
            {
                sw.WriteLine("^XA~TA000~JSN^LT0^MNW^MTD^PON^PMN^LH0,0^JMA^PR2,2~SD15^JUS^LRN^CI0^XZ");
                sw.WriteLine("^XA");
                sw.WriteLine("^MMT");
                sw.WriteLine("^PW609");
                sw.WriteLine("^LL0406");
                sw.WriteLine("^LS0");
                sw.WriteLine(@"^FT29,75^A0N,62,62^FH\^FD{0}^FS".Fmt(li.first));
                sw.WriteLine(@"^FT29,118^A0N,28,28^FH\^FD{0}^FS".Fmt(li.last));
                sw.WriteLine(@"^FT29,153^A0N,28,28^FH\^FD{0} |{1}{2}{3}^FS".Fmt(li.mv,
                    li.allergies.HasValue() ? " A |" : "",
                    li.transport ? " T |" : "",
                    li.custody ? " C |" : ""));
                sw.WriteLine(@"^FT29,185^A0N,28,28^FH\^FD{0}^FS".Fmt(li.org));
                sw.WriteLine(@"^FT473,48^A0N,28,28^FH\^FD{0:M/d/yy}^FS".Fmt(li.hour));
                sw.WriteLine(@"^FT474,77^A0N,28,28^FH\^FD{0:H:mm tt}^FS".Fmt(li.hour));
                sw.WriteLine(@"^FT470,152^A0N,73,72^FH\^FD{0}^FS".Fmt(code));
                sw.WriteLine("^PQ{0},0,1,Y^XZ".Fmt(nlabels));
            }
            else if (Program.Printer.Contains("Datamax"))
            {
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
                sw.WriteLine("1911A1800500010" + li.first);
                sw.WriteLine("1911A1000350011" + li.last);
                sw.WriteLine("1911A1000190011{0} |{1}{2}{3}".Fmt(li.mv,
                    li.allergies.HasValue() ? " A |" : "",
                    li.transport ? " T |" : "",
                    li.custody ? " C |" : ""));
                sw.WriteLine("1911A1000020011" + li.org);
                sw.WriteLine("1911A1000610222{0:M/d/yy}".Fmt(li.hour));
                sw.WriteLine("1911A1000470222{0:h:mm tt}".Fmt(li.hour));
                sw.WriteLine("1911A2400140219" + code);

                sw.WriteLine("Q" + nlabels.ToString("0000"));
                sw.WriteLine("E");
            }
            
            sw.Flush();

            memStrm.Position = 0;
            PrintRawHelper.SendDocToPrinter(Program.Printer, memStrm);
            sw.Close();
            return nlabels;
        }
        public static int Label2(IEnumerable<LabelInfo> li, int nlabels, string code)
        {
            if (nlabels <= 0 || !Program.Printer.HasValue())
                return 0;
            var memStrm = new MemoryStream();
            var sw = new StreamWriter(memStrm);
            if (Program.Printer.Contains("Godex"))
            {
                sw.WriteLine("^Q51,3");
                sw.WriteLine("^W76");
                sw.WriteLine("^H10");
                sw.WriteLine("^P" + nlabels);
                sw.WriteLine("^S4");
                sw.WriteLine("^AD");
                sw.WriteLine("^C1");
                sw.WriteLine("^R0");
                sw.WriteLine("~Q+0");
                sw.WriteLine("^O0");
                sw.WriteLine("^D0");
                sw.WriteLine("^E12");
                sw.WriteLine("~R200");
                sw.WriteLine("^L");
                sw.WriteLine("Dy2-me-dd");
                sw.WriteLine("Th:m:s");
                sw.WriteLine("AH,16,0,1,1,0,0," + li.First().first);
                sw.WriteLine("AE,18,84,1,1,0,0," + li.First().last);
                sw.WriteLine("AH,428,184,1,1,0,0," + code);
                sw.WriteLine("AD,441,156,1,1,0,0,{0:M/d/yy}".Fmt(li.First().hour));
                sw.WriteLine("AD,19,129,1,1,0,0,{0}{1}{2}".Fmt(
                    li.First().allergies.HasValue() ? " A |" : "",
                    li.First().transport ? " T |" : "",
                    li.First().custody ? " C |" : ""));

                var vertpos = 290;
                foreach (var i in li)
                {
                    sw.WriteLine("AC,16,{0},1,1,0,0,{1}".Fmt(vertpos, i.location));
                    sw.WriteLine("AC,120,{0},1,1,0,0,{1:h:mm t}".Fmt(vertpos, i.hour));
                    sw.WriteLine("AB,220,{0},1,1,0,0,{1} ({2})".Fmt(vertpos + 4, i.org, i.mv));
                    vertpos += 35;
                }
                sw.WriteLine("E");
            }
            sw.Flush();

            memStrm.Position = 0;
            PrintRawHelper.SendDocToPrinter(Program.Printer, memStrm);
            sw.Close();
            return nlabels;
        }

        public static int AllergyLabel(LabelInfo li)
        {
            if (li.n == 0 || !Program.Printer.HasValue())
                return 0;
            if (!li.mv.Contains("G"))
                return 0;
            var memStrm = new MemoryStream();
            var sw = new StreamWriter(memStrm);
            if (Program.Printer.Contains("ZDesigner"))
            {
                sw.WriteLine("^XA~TA000~JSN^LT0^MNW^MTD^PON^PMN^LH0,0^JMA^PR2,2~SD15^JUS^LRN^CI0^XZ");
                sw.WriteLine("^XA");
                sw.WriteLine("^MMT");
                sw.WriteLine("^PW609");
                sw.WriteLine("^LL0406");
                sw.WriteLine("^LS0");
                sw.WriteLine(@"^FT29,83^A0N,62,62^FH\^FD{0}^FS".Fmt(li.first));
                sw.WriteLine(@"^^FT30,134^A0N,34,33^FH\^FD{0}^FS".Fmt(li.last));
                sw.WriteLine(@"^FT30,183^A0N,28,28^FH\^FDGuest{0}^FS".Fmt(li.allergies.HasValue() ? " (" + li.allergies + ")" : ""));
                sw.WriteLine("^PQ1,0,1,Y^XZ");
            }
            else if (Program.Printer.Contains("Datamax"))
            {
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
                sw.WriteLine("1911A1800500010" + li.first);
                sw.WriteLine("1911A1200320010" + li.last);
                sw.WriteLine("1911A1000060011Guest" + (li.allergies.HasValue() ? " (" + li.allergies + ")" : ""));
                sw.WriteLine("Q0001");
                sw.WriteLine("E");
            }
            sw.Flush();
            memStrm.Position = 0;
            PrintRawHelper.SendDocToPrinter(Program.Printer, memStrm);
            sw.Close();
            return 1;
        }
        public static int LocationLabel(LabelInfo li)
        {
            if (li.n == 0 || !Program.Printer.HasValue())
                return 0;
            if (!li.mv.Contains("G") || !li.location.HasValue())
                return 0;
            var memStrm = new MemoryStream();
            var sw = new StreamWriter(memStrm);
            if (Program.Printer.Contains("ZDesigner"))
            {
                sw.WriteLine("^XA~TA000~JSN^LT0^MNW^MTD^PON^PMN^LH0,0^JMA^PR2,2~SD15^JUS^LRN^CI0^XZ");
                sw.WriteLine("^XA");
                sw.WriteLine("^MMT");
                sw.WriteLine("^PW609");
                sw.WriteLine("^LL0406");
                sw.WriteLine("^LS0");
                sw.WriteLine(@"^FT29,83^A0N,62,62^FH\^FD{0}^FS".Fmt(li.first));
                sw.WriteLine(@"^FT30,155^A0N,45,45^FH\^FDLocation/time: {0}, {1:h:mm tt}^FS"
                    .Fmt(li.location, li.hour));
                sw.WriteLine("^PQ1,0,1,Y^XZ");
            }
            else if (Program.Printer.Contains("Datamax"))
            {
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
                sw.WriteLine("1911A1800450014" + li.first);
                sw.WriteLine("1911A1400120013Location: {0}, {1:h:mm tt}"
                    .Fmt(li.location, li.hour));
                sw.WriteLine("Q0001");
                sw.WriteLine("E");
            }
            sw.Flush();
            memStrm.Position = 0;
            PrintRawHelper.SendDocToPrinter(Program.Printer, memStrm);
            sw.Close();
            return 1;
        }
        public static int LocationLabel2(IEnumerable<LabelInfo> list)
        {
            var li = list.First();
            if (li.n == 0 || !Program.Printer.HasValue())
                return 0;
            if (!li.mv.Contains("G") || !li.location.HasValue())
                return 0;
            var memStrm = new MemoryStream();
            var sw = new StreamWriter(memStrm);
            if (Program.Printer.Contains("Godex"))
            {
                sw.WriteLine("^XA~TA000~JSN^LT0^MNW^MTD^PON^PMN^LH0,0^JMA^PR2,2~SD15^JUS^LRN^CI0^XZ");
                sw.WriteLine("^XA");
                sw.WriteLine("^MMT");
                sw.WriteLine("^PW609");
                sw.WriteLine("^LL0406");
                sw.WriteLine("^LS0");
                sw.WriteLine(@"^FT29,83^A0N,62,62^FH\^FD{0}^FS".Fmt(li.first));
                sw.WriteLine(@"^FT30,155^A0N,45,45^FH\^FDLocation/time: {0}, {1:h:mm tt}^FS"
                    .Fmt(li.location, li.hour));
                sw.WriteLine("^PQ1,0,1,Y^XZ");
            }
            
            sw.Flush();
            memStrm.Position = 0;
            PrintRawHelper.SendDocToPrinter(Program.Printer, memStrm);
            sw.Close();
            return 1;
        }
    }
}
