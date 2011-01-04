<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<CmsData.Zip>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="/Content/js/jquery.jeditable.js" type="text/javascript"></script>
    <script type="text/javascript">
        //id=elements_id&value=user_edited_content
        $(function() {
            $(".clickSelect").editable("/Setup/MetroZip/Edit/", {
                indicator: '<img src="/images/loading.gif">',
                loadurl: "/Setup/MetroZip/ResidentCodes/",
                loadtype: "POST",
                type: "select",
                submit: "OK",
                style: 'display: inline'
            });
            $("a.delete").click(function(ev) {
                if (confirm("are you sure?"))
                    $.post("/Setup/MetroZip/Delete/" + $(this).attr("id"), null, function(ret) {
                        window.location = "/Setup/MetroZip/";
                    });
                return false;
            });
        });
    </script>
    <h2>Zips</h2>
    <h3><span style="color:#008000"><%=ViewData["msg"] %></span></h3>

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
                    class='clickSelect'><%=item.ResidentCode == null ? "" : item.ResidentCode.Description%></span>
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
    <% using (Html.BeginForm("UpdateMetroZips", "MetroZip"))
       { %>
    <p>
        <input type="submit" value="Update all Metro Zips" />
    </p>
    <% } %>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

