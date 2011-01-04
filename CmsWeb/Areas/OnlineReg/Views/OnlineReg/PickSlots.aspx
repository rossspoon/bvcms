<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site2.Master" Inherits="System.Web.Mvc.ViewPage<CmsWeb.Models.SlotModel>" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <title>Prayer Signup</title>
    <link href="/Content/jquery.tooltip.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%= SquishIt.Framework.Bundle.JavaScript()
        .Add("/Content/js/jquery.blockUI.js")
        .Add("/Content/js/jquery.tooltip.js")
        .Add("/Scripts/pickslots2.js")
        .Render("/Content/PickSlots#.js")
            %>        
    <h2>
        Committment Times for <%=Model.person.Name %> <span style="font-size:10pt">(<%=Model.Count()%> total commitments)</span></h2>
        <%= Html.Hidden("pid", Model.person.PeopleId) %>
        <%= Html.Hidden("oid", Model.org.OrganizationId) %>
   <table align="center" border="0" cellspacing="0" cellpadding="0"><tr><td>
   <table border="0" cellpadding="0" cellspacing="0" id="slots">
        <thead>
            <tr>
                <th></th>
<%  foreach (var c in Model.cols)
    { %>
                <th><%=c.Value %></th>
<%  } %>
            </tr>
        </thead>
        <tbody>
<% foreach(var ts in Model.FetchSlots())
   { %>
            <tr>
                <th><%=ts.rowtitle %></th>
    <% foreach (var si in ts.slots)
       {
           if (si.slot != null)
               Html.RenderPartial("Special/PickSlot", si);
           else
           { %>
                <td></td>
        <% }
       } %>
            </tr>
<% } %>
        </tbody>
    </table>
   </td><td valign="top">
   <table id="leg"  cellspacing="0">
  <col/>
  <col align="left"/>
   <tr><td colspan="3">Check a box to claim a spot</td></tr>
   <tr><td>&nbsp;</td><td>&nbsp;</td><td>empty</td></tr>
   <tr><td class="m1"></td><td class="o1"></td><td>few</td></tr>
   <tr><td class="m2"></td><td class="o2"></td><td>some</td></tr>
   <tr><td class="m3"></td><td class="o3"></td><td>many</td></tr>
   <tr><td colspan="3">green = yours</td></tr>
   <tr><td colspan="3">unchecked = available</td></tr>
   </table>
   <form method="post" action="/OnlineReg/ConfirmSlots/<%=Model.person.PeopleId %>?orgid=<%=Model.org.OrganizationId %>">
   <p style="text-align: center">
   <input id="submitit" type="submit" class="submitbutton" 
    value='Send Confirmation Email' /></p></form>
   </td></tr></table>
    <div class="growlUI" style="display: none;">
        <h1>Growl Notification</h1>
        <h2>Have a nice day!</h2>
    </div>
</asp:Content>
