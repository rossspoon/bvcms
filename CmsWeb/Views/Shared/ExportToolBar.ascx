<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<ul class="sf-tab">
    <li class="headlink"><a href=''><img src="/images/Mail.png" /> 
        Email</a>
        <ul>
            <li><a href='/EmailPeople.aspx?id=<%=ViewData["queryid"]%>'><img src="/images/Mail.png" /> 
                Individuals</a></li>
            <li><a href='/EmailPeople.aspx?id=<%=ViewData["queryid"]%>&parents=true'><img src="/images/Mail.png" /> 
                Parents</a></li>
        </ul>
    </li>
    <li class="headlink"><a href=''><img src="/images/BulkMailing.png" /> 
        Export</a>
        <ul>
            <li><a href='/ExportExcel.aspx?id=<%=ViewData["queryid"]%>' class="ChooseLabelType" 
                title="For mail merge"><img src="/images/Excel.png" /> 
                Excel</a></li>
            <li><a href='/bulkmail.aspx?id=<%=ViewData["queryid"]%>' class="ChooseLabelType" 
                title="Comma separated values text file, opens in excel, for bulk mailings"><img src="/images/Excel.png" /> 
                Bulk (csv)</a></li>
<% if ((bool?)ViewData["OrganizationContext"] ?? false)
   { %>
            <li>
                <a href='/ExportExcel.aspx?id=<%=ViewData["queryid"] %>&format=Organization' title="Includes Org Member info"><img src="/images/Excel.png" /> 
                    Member Export</a></li>
            <li>
                <a href='/ExportExcel.aspx?id=<%=ViewData["queryid"] %>&format=SML' title="Includes Soulmate Live info"><img src="/images/Excel.png" />
                    Soulmate Export</a></li>
            <li>
                <a href='/ExportExcel.aspx?id=<%=ViewData["queryid"] %>&format=Promotion' title="Just for Promotion Mail Merge"><img src="/images/Excel.png" />
                    Promotion Export</a></li>
<% } %>
        </ul>
    </li>
    <li class="headlink"><a href=''><img src="/images/Report.png" /> 
        Reports</a>
        <ul>
            <li><a href='/Report/ProspectCardsrpt.aspx?id=<%=ViewData["queryid"]%>'
                target="_blank"><img src="/images/Report.png" /> 
                Prospect Form</a></li>
            <li><a href='/Report/InreachRpt.aspx?id=<%=ViewData["queryid"]%>' 
                target="_blank"><img src="/images/Report.png" /> 
                Inreach Form</a></li>
            <li><a href='/Reports/Contacts/<%=ViewData["queryid"]%>' 
                target="_blank" title="Report for Robo-calling Contacts"><img src="/images/Report.png" />
                Contact Report</a></li>
            <li><a href='/Reports/WeeklyAttendance/<%=ViewData["queryid"]%>' 
                target="_blank" title="General Attendance Stats"><img src="/images/Report.png" />
                Weekly Attend</a></li>
            <li><a href='/ExportExcel.aspx?id=<%=ViewData["queryid"]%>&format=Involvement' 
                target="_blank" title="Personal, Contact and Enrollment Info"><img src="/images/Excel.png" />
                Involvement</a></li>
            <li><a href='/Reports/Family/<%=ViewData["queryid"]%>' 
                target="_blank"><img src="/images/Report.png" /> 
                Family Report</a></li>
            <li><a href='/Volunteers/Index/<%=ViewData["queryid"]%>' 
                target="_blank"><img src="/images/Report.png" /> 
                Volunteer Report</a></li>
<% if ((bool?)ViewData["OrganizationContext"] ?? false)
   { %>
            <li><a href='/Reports/Registration/<%=ViewData["queryid"]%>?oid=<%=Util.CurrentOrgId %>' 
                target="_blank"><img src="/images/Report.png" /> 
                Registration Report</a></li>
<% }
   else
   { %>
            <li><a href='/Reports/Registration/<%=ViewData["queryid"]%>' 
                target="_blank"><img src="/images/Report.png" /> 
                Registration Report</a></li>
<% } %>   
            <li><a href='/ExportExcel.aspx?id=<%=ViewData["queryid"]%>&format=Attend' 
                target="_blank" title="Contains attendance information for their class"><img src="/images/Excel.png" />
                BF Attendance</a></li>
            <li><a href='/ExportExcel.aspx?id=<%=ViewData["queryid"]%>&format=Children' 
                target="_blank" title="Contains emergency contact, who brought child info"><img src="/images/Excel.png" />
                Children</a></li>
            <li><a href='/ExportExcel.aspx?id=<%=ViewData["queryid"]%>&format=Church' 
                target="_blank" title="Contains other Church Info"><img src="/images/Excel.png" />
                Other Churches</a></li>
        </ul>
    </li>
    <li class="headlink"><a href=''><img src="/images/BulkMailing.png" /> 
        Labels</a>
        <ul>
            <li><a href='/Report/LabelsRpt.aspx?id=<%=ViewData["queryid"]%>' class="ChooseLabelType" 
                title="Labels (pdf for Datamax label printer)" target="_blank"><img src="/images/tags.png" /> 
                Roll Labels</a></li>
<% if ((bool?)ViewData["OrganizationContext"] ?? false)
   { %>                
            <li>
                <a id="RollsheetLink" href='#' title="Rollsheet Report"><img src="/images/tags.png" />
                Rollsheet Report</a></li>
<% } %>
            <li><a href='/Reports/BarCodeLabels/<%=ViewData["queryid"]%>' 
                target="_blank" title="Labels for Choir Attendance"><img src="/images/tags.png" />
                Barcode Labels</a></li>
            <li>
                <a href='/Reports/Avery/<%=ViewData["queryid"] %>' 
                    title="Avery Name Labels" target="_blank">
                    <img src="/images/tags.png" />
                    Avery Labels</a></li>
            <li>
                <a href='/Reports/Avery3/<%=ViewData["queryid"] %>' 
                    title="Avery 3 Across Labels (person per row)" target="_blank">
                    <img src="/images/tags.png" />
                    Avery Labels 3</a></li>
            <li><a href='/Reports/AveryAddress/<%=ViewData["queryid"]%>' class="ChooseLabelType" 
                title="Address Labels"><img src="/images/tags.png" /> 
                Avery Address</a></li>
        </ul>
    </li>
    <li class="headlink"><a href='#'><img src="/images/Tag.png" />
        Tag</a>
        <ul>
            <li><a id="TagAll" href='<%=ViewData["TagAction"] %>'><img src="/images/Tag.png" />
                Add All</a></li>
            <li><a id="UnTagAll" href='<%=ViewData["UnTagAction"] %>'><img src="/images/Tag.png" />
                Remove All</a></li>
        </ul>
    </li>
    <li class="headlink"><a href=''><img src="/images/Tag.png" />
        Other</a>
        <ul>
            <li><a href='#' 
                onclick='confirm("Are you sure you want to add a contact for all these people?")'>
                Add Contact</a></li>
            <li><a href='/Task/NotesExcel/<%=ViewData["queryid"] %>'>
                Export Task Notes</a></li>
        </ul>
    </li>
</ul>
