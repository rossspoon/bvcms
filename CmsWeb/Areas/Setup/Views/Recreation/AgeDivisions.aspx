<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<CmsData.Organization>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="/Content/js/jquery.jeditable.js" type="text/javascript"></script>
    <script type="text/javascript">
        //id=elements_id&value=user_edited_content
        $(function() {
            $(".clickEdit").editable("/Setup/Recreation/Edit/", {
                indicator: "<img src='/images/loading.gif'>",
                tooltip: "Click to edit...",
                style: 'display: inline',
                width: '200px',
                height: 25
            });
            $(".clickSelectOrg").editable("/Setup/Recreation/Edit/", {
                indicator: '<img src="/images/loading.gif">',
                loadurl: "/Setup/Recreation/Organizations/",
                loadtype: "POST",
                type: "select",
                submit: "OK",
                style: 'display: inline'
            });
            $(".clickSelectGender").editable("/Setup/Recreation/Edit/", {
                indicator: '<img src="/images/loading.gif">',
                loadurl: "/Setup/Recreation/Genders/",
                loadtype: "GET",
                type: "select",
                submit: "OK",
                style: 'display: inline'
            });
            $("a.delete").click(function(ev) {
                if (confirm("are you sure?"))
                    $.post("/Setup/Recreation/DeleteAgeDivision/" + $(this).attr("id"), null, function(ret) {
                        window.location = "/Setup/Recreation/AgeDivisions/" + $("#id").val();
                    });
                return false;
            });
        });
    </script>
    <h2><a href="/Setup/Recreation/Index"><%=ViewData["League"] %></a></h2>

    <table>
        <tr>
            <th>
                Age Division
            </th>
            <th>
                StartAge
            </th>
            <th>
                EndAge
            </th>
            <th>
                Gender
            </th>
            <th>
                Fee
            </th>
        </tr>

    <% foreach (var item in Model) 
       { %>
        <tr>
            <td><a href="/Organization/Index/<%=item.OrganizationId %>"><%=item.OrganizationName%></a></td>
            <td>
                <span id='s<%=item.OrganizationId %>' 
                    class='clickEdit'><%=item.GradeAgeStart%></span>
            </td>
            <td>
                <span id='e<%=item.OrganizationId %>' 
                    class='clickEdit'><%=item.GradeAgeEnd%></span>
            </td>
            <td>
                <span id='g<%=item.OrganizationId %>'
                    class='clickSelectGender'><%=item.GenderId == null ? "click to set" : item.Gender.Description%></span>
            </td>
            <td>
                <span id='f<%=item.OrganizationId %>' 
                    class='clickEdit'><%=item.Fee%></span>
            </td>
        </tr>
    <% } %>

    </table>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

