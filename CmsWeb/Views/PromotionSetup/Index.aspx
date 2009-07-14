<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<CmsData.Promotion>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="/Content/js/jquery.jeditable.js" type="text/javascript"></script>
    <script type="text/javascript">
        //id=elements_id&value=user_edited_content
        $(function() {
            $(".clickEdit").editable("/PromotionSetup/Edit/", {
                indicator: "<img src='/images/loading.gif'>",
                tooltip: "Click to edit...",
                style: 'display: inline',
                width: '200px'
            });
            $(".clickSelect").editable("/PromotionSetup/EditDiv/", {
                indicator: '<img src="/images/loading.gif">',
                loadurl: "/PromotionSetup/DivisionCodes/",
                type: "select",
                submit: "OK",
                style: 'display: inline'
            });
            $("a.delete").click(function(ev) {
                if (confirm("are you sure?"))
                    $.post("/PromotionSetup/Delete/" + $(this).attr("id"), null, function(ret) {
                        window.location = "/PromotionSetup/";
                    });
                return false;
            });
        });
    </script>
    <h2>Promotion Setup</h2>

    <table>
        <tr>
            <th>
                Sort
            </th>
            <th>
                Description
            </th>
            <th>
                From Division
            </th>
            <th>
                To Division
            </th>
            <th></th>
        </tr>

    <% foreach (var item in Model) 
       { %>
        <tr>
            <td>
                <span id='s<%=item.Id %>' 
                    class='clickEdit'><%=item.Sort%></span>
            </td>
            <td>
                <span id='d<%=item.Id %>' 
                    class='clickEdit'><%=item.Description%></span>
            </td>
            <td>
                <span id='f<%=item.Id %>' 
                    class='clickSelect'><%=item.FromDivId == null? "click to set" : item.FromDivision.Name%></span>
            </td>
            <td>
                <span id='t<%=item.Id %>' 
                    class='clickSelect'><%=item.ToDivId == null ? "click to set" : item.ToDivision.Name%></span>
            </td>
            <td>
                <a id='x<%=item.Id %>' href="#" class="delete"><img border="0" src="/images/delete.gif" /></a>
            </td>
        </tr>
    <% } %>

    </table>

    <% using (Html.BeginForm("Create", "PromotionSetup"))
       { %>
    <p><input type="submit" value="Create" /></p>
    <% } %>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

