<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.CouponModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Coupons</h2>
    <% using (Html.BeginForm()) 
       { %>
        <div>
            <fieldset>
                <table style="empty-cells:show">
                <col style="width: 13em; text-align:right" />
                <col />
                <col />
                 <tr>
                    <td><label for="end">Start Date</label></td>
                    <td><%=Html.DropDownList("Div", Model.OnlineRegs()) %></td>
                </tr>
                <tr>
                    <td>&nbsp;</td><td><input type="submit" value="Run" /></td>
                </tr>
                </table>
            </fieldset>
        </div>
    <% } %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">

</asp:Content>
