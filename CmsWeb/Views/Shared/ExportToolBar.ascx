<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<ul class="sf-tab">
    <li class="headlink"><a href=''><img src="/images/Mail.png" /> 
        Email</a>
        <ul>
            <li><a href='/EmailPeople.aspx?id=<%=ViewData["queryid"]%>'><img src="/images/Mail.png" /> 
                Individuals</a></li>
        </ul>
    </li>
    <li class="headlink"><a href=''><img src="/images/BulkMailing.png" /> 
        Export</a>
        <ul>
            <li><a href='/Report/LabelsRpt.aspx?id=<%=ViewData["queryid"]%>' class="ChooseLabelType" 
                title="Labels (pdf for label printer)"><img src="/images/tags.png" /> 
                Labels</a></li>
            <li><a href='/ExportExcel.ashx?id=<%=ViewData["queryid"]%>' class="ChooseLabelType" 
                title="For mail merge"><img src="/images/Excel.png" /> 
                Excel</a></li>
            <li><a href='/bulkmail.ashx?id=<%=ViewData["queryid"]%>' class="ChooseLabelType" 
                title="Comma separated values text file, opens in excel, for bulk mailings"><img src="/images/Excel.png" /> 
                Bulk (csv)</a></li>
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
            <li><a href='/ExportExcel.ashx?id=<%=ViewData["queryid"]%>&format=Involvement' 
                target="_blank" title="Personal, Contact and Enrollment Info"><img src="/images/Excel.png" />
                Involvement</a></li>
            <li><a href='/ExportExcel.ashx?id=<%=ViewData["queryid"]%>&format=Attend' 
                target="_blank" title="Contains attendance information for their class"><img src="/images/Excel.png" />
                Attendance</a></li>
            <li><a href='/ExportExcel.ashx?id=<%=ViewData["queryid"]%>&format=Children' 
                target="_blank" title="Contains emergency contact, who brought child info"><img src="/images/Excel.png" />
                Children</a></li>
            <li><a href='/ExportExcel.ashx?id=<%=ViewData["queryid"]%>&format=Church' 
                target="_blank" title="Contains other Church Info"><img src="/images/Excel.png" />
                Other Church</a></li>
            <li><a href='/Report/ChoirMeeting.aspx?id=<%=ViewData["queryid"]%>' 
                target="_blank" title="Labels for Choir Attendance"><img src="/images/tags.png" />
                Choir Attendance Labels</a></li>
            <li><a href='/Report/ContactReport.aspx?id=<%=ViewData["queryid"]%>' 
                target="_blank" title="Report for Robo-calling Contacts"><img src="/images/Report.png" />
                Contact Report</a></li>
        </ul>
    </li>
    <li class="headlink"><a href='#'><img src="/images/Tag.png" />
        Tag</a>
        <ul>
            <li><a id="TagAll" href='#'><img src="/images/Tag.png" />
                Add All</a></li>
            <li><a id="UnTagAll" href='#'><img src="/images/Tag.png" />
                Remove All</a></li>
        </ul>
    </li>
    <li class="headlink"><a href=''><img src="/images/Tag.png" />
        Other</a>
        <ul>
            <li><a href='#' 
                onclick='confirm("Are you sure you want to add a contact for all these people?")'><img src="/images/Tag.png" />
                Add Contact</a></li>
        </ul>
    </li>
</ul>
<div id="ChooseLabelType" class="modalPopup" style="width: 350px; padding: 10px">
    <table>
        <tr>
            <td style="margin: 3px; border: solid thin black"><ul>
                <li title="Addressed to individuals">
                <%=Html.RadioButton("addressedto", "Individual", true) %> Individual</li>
                <li title="Addressed as a family when there are children">
                <%=Html.RadioButton("addressedto", "Family") %> Family</li>
                <li title="Addressed to a couple, if both are in selection">
                <%=Html.RadioButton("addressedto", "CouplesBoth") %> Couples (both)</li>
                <li title="Addressed to a couple, if one or both are in selection">
                <%=Html.RadioButton("addressedto", "CouplesEither") %> Couples (either)</li>
                <li title="Addressed to parents or parent">
                <%=Html.RadioButton("addressedto", "ParentsOf") %> Parents Of</li>
            </ul></td>
            <td><%=Html.CheckBox("UseTitle") %> Use Titles</td>
        </tr>
        <tr>
            <td colspan="2" align="right">
                <input id="cmdOK" type="button" value="OK" />
            </td>
        </tr>
    </table>
</div>
