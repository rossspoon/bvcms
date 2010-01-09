<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<CmsData.RecAgeDivision>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="/Content/js/jquery.jeditable.js" type="text/javascript"></script>
    <script type="text/javascript">
        //id=elements_id&value=user_edited_content
        $(function() {
            $(".clickEdit").editable("/Setup/Recreation/Edit/", {
                indicator: "<img src='/images/loading.gif'>",
                tooltip: "Click to edit...",
                style: 'display: inline',
                width: '200px'
            });
            $(".clickSelectDiv").editable("/Setup/Recreation/EditDiv/", {
                indicator: '<img src="/images/loading.gif">',
                loadurl: "/Setup/Recreation/Divisions/",
                loadtype: "POST",
                type: "select",
                submit: "OK",
                style: 'display: inline',
                callback: function() {
                    var sid = '#o' + this.id.substring(1);
                    $(sid).text('click to select');
                }
            });
            $(".clickSelectOrg").editable("/Setup/Recreation/EditOrg/", {
                indicator: '<img src="/images/loading.gif">',
                loadurl: "/Setup/Recreation/Organizations/",
                loadtype: "POST",
                type: "select",
                submit: "OK",
                style: 'display: inline'
            });
            $(".clickSelectGender").editable("/Setup/Recreation/EditGender/", {
                indicator: '<img src="/images/loading.gif">',
                loadurl: "/Setup/Recreation/Genders/",
                loadtype: "GET",
                type: "select",
                submit: "OK",
                style: 'display: inline'
            });
            $("a.delete").click(function(ev) {
                if (confirm("are you sure?"))
                    $.post("/Setup/Recreation/Delete/" + $(this).attr("id"), null, function(ret) {
                        window.location = "/Setup/Recreation/";
                    });
                return false;
            });
        });
    </script>
    <h2>Recreation Setup</h2>

    <table>
        <tr>
            <th>
                League
            </th>
            <th>
                Division
            </th>
            <th>
                StartAge
            </th>
            <th>
                EndAge
            </th>
            <th>
                AgeDate
            </th>
            <th>
                Gender
            </th>
            <th>
                Fee
            </th>
            <th>
                ExtraFee
            </th>
            <th>
                ExpirationDt
            </th>
            <th></th>
        </tr>

    <% foreach (var item in Model) 
       { %>
        <tr>
            <td>
                <span id='d<%=item.Id %>'
                    class='clickSelectDiv'><%=item.DivId == null? "click to set" : item.Division.Name%></span>
            </td>
            <td>
                <span id='o<%=item.Id %>'
                    class='clickSelectOrg'><%=item.OrgId == null? "click to set" : item.Organization.OrganizationName%></span>
            </td>
            <td>
                <span id='s<%=item.Id %>' 
                    class='clickEdit'><%=item.StartAge%></span>
            </td>
            <td>
                <span id='e<%=item.Id %>' 
                    class='clickEdit'><%=item.EndAge%></span>
            </td>
            <td>
                <span id='a<%=item.Id %>' 
                    class='clickEdit'><%=item.AgeDate%></span>
            </td>
            <td>
                <span id='g<%=item.Id %>'
                    class='clickSelectGender'><%=item.GenderId == null ? "click to set" : item.Gender.Description%></span>
            </td>
            <td>
                <span id='f<%=item.Id %>' 
                    class='clickEdit'><%=item.Fee%></span>
            </td>
            <td>
                <span id='p<%=item.Id %>' 
                    class='clickEdit'><%=item.ExtraFee%></span>
            </td>
            <td>
                <span id='z<%=item.Id %>' 
                    class='clickEdit'><%=item.ExpirationDt%></span>
            </td>
            <td>
                <a id='x<%=item.Id %>' href="#" class="delete"><img border="0" src="/images/delete.gif" /></a>
            </td>
        </tr>
    <% } %>

    </table>

    <% using (Html.BeginForm("Create", "RecreationSetup"))
       { %>
    <p><input type="submit" value="Create" /></p>
    <% } %>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

