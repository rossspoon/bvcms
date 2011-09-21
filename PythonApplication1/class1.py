import System
import System.Text
import clr
clr.AddReferenceByName("CmsData")
from CmsData import QueryFunctions
from System import *
from System.Text import *

class VitalStats(object):
    def Run(self, m):
        days = 8
        fmt = '<tr><td align="right">{0,25}:</td><td align="right">{1,8:n0}</td></tr>\r\n'
        sb = StringBuilder()

        sb.AppendLine('<table cellspacing="5" class="grid">')
        sb.AppendFormat(fmt, "Members", m.QueryCount("S01:Members"))
        sb.AppendFormat(fmt, "Decisions", m.QueryCount("S02:Recent Decision"))
        sb.AppendFormat(fmt, "Attended Recently", m.QueryCount("S03:Attended Recently"))
        sb.AppendFormat(fmt, "New Attends", m.QueryCount("S04:New Attends"))
        #sb.AppendFormat(fmt, "Contacts", m.QueryCount("S05:Contacts"))
        sb.AppendFormat(fmt, "Meeting", m.MeetingCount(days, 0, 0, 0))
        sb.AppendFormat(fmt, "Num Present", m.NumPresent(days, 0, 0, 0))
        sb.AppendFormat(fmt, "Registrations", m.RegistrationCount(days, 0, 0, 0))
        sb.AppendLine("<tr><td colspan=2>Contributions-----------------</td></tr>")
        sb.AppendFormat(fmt, "Total 7dys", m.ContributionTotals(days, 0, 0))
        sb.AppendFormat(fmt, "Count 7dys", m.ContributionCount(days, 0, 0))
        sb.AppendFormat(fmt, "Average per Capita Year", \
            m.ContributionTotals(365, 0, 0) / m.ContributionCount(365, 0, 0))
        sb.AppendFormat(fmt, "Weekly 4 week average", m.ContributionTotals(7*5, 7, 0) / 4)
        sb.AppendFormat(fmt, "Weekly average past 52wks", m.ContributionTotals(52*7, 0, 0) / 52)
        sb.AppendFormat(fmt, "Weekly average prev 52wks", \
            m.ContributionTotals(52*7*2, 52*7, 0) / 52)
        sb.AppendLine('</table>')
        return sb.ToString()


m = QueryFunctions()
vs = VitalStats()
ret = vs.Run(m)

Console.Write(ret);
Console.Write('press any key')
Console.ReadKey(True)