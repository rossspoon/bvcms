<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/onlinereg.Master" Inherits="System.Web.Mvc.ViewPage<CmsWeb.Models.ManageSubsModel>" %> 
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%= SquishIt.Framework.Bundle.JavaScript()
        .Add("/Content/js/jquery-1.4.2.js")
        .Add("/Content/js/jquery-ui-1.8.2.custom.js")
        .Add("/Content/js/jquery.blockUI.js")
        .Add("/Content/js/jquery.tooltip.js")
        .Render("/Content/ManageSubs#.js") %>        
    <h2><%=Model.Division.Name %></h2>
    <h4>Email Subscription Options for <%=Model.person.Name %> &lt;<%=Model.person.EmailAddress %>&gt;</h4>
   <form method="post" action="/OnlineReg/ConfirmSubscriptions">
    <%= Html.Hidden("pid", Model.pid) %>
    <%= Html.Hidden("divid", Model.divid) %>
   <p style="text-align: center">
   <table id="subs">
   <% foreach (var o in Model.FetchSubs())
      { %>
        <tr><td valign="top"><input type="checkbox" name="Subscribe" <%=o.CHECKED %> value="<%=o.OrgId %>"></td>
            <td><h3><%=o.Name %></h3>
                <h4><%=o.Description %></h4>
            </td></tr>
   <% } %>
   </table>
   <input id="submitit" type="submit" class="submitbutton" value='Submit' /></p>
    </form>
</asp:Content>
