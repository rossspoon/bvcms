<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<CmsData.Zip>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="/Content/js/jquery.jeditable.js" type="text/javascript"></script>
    <script type="text/javascript">
        //id=elements_id&value=user_edited_content
        $(function() {
            $(".clickSelect").editable("/Scaffold/MetroZip/Edit/", {
                indicator: '<img src="/images/loading.gif">',
                loadurl: "/Scaffold/MetroZip/ResidentCodes/",
                type: "select",
                submit: "OK",
                style: 'display: inline'
            });
            $("a.delete").click(function(ev) {
                if (confirm("are you sure?"))
                    $.post("/Scaffold/MetroZip/Delete/" + $(this).attr("id"), null, function(ret) {
                        window.location = "/Scaffold/MetroZip/";
                    });
                return false;
            });
        });
    </script>
    <h2>Zips</h2>

    <table>
        <tr>
            <th>
                ZipCode
            </th>
            <th>
                MetroMarginalCode
            </th>
            <th></th>
        </tr>

    <% foreach (var item in Model) 
       { %>
        <tr>
            <td><%= Html.Encode(item.ZipCode)%></td>
            <td>
                <span id='r<%=item.ZipCode %>' 
                    class='clickSelect'>
                <%=item.ResidentCode == null ? "" : item.ResidentCode.Description%>
                </span>
            </td>
            <td>
                <a id='d<%=item.ZipCode %>' href="#" class="delete"><img border="0" src="/images/delete.gif" /></a>
            </td>
        </tr>
    <% } %>

    </table>

    <% using (Html.BeginForm("Create", "MetroZip"))
       { %>
    <p>
        New ZipCode: <%= Html.TextBox("zipcode") %>
        <input type="submit" value="Create" />
    </p>
    <% } %>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

