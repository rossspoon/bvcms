<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.CouponModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Coupons</h2>
    <form action="/Coupon/Create" method="post">
        <div>
            <fieldset>
                <table style="empty-cells:show">
                 <tr>
                    <td>Registration Type</td>
                    <td><%=Html.DropDownList("regid", Model.OnlineRegs()) %></td>
                </tr>
                <tr>
                    <td>Name</td>
                    <td><%=Html.TextBox("name") %></td>
                </tr>
                <tr>
                    <td>Amount</td>
                    <td><%=Html.TextBox("amount") %></td>
                </tr>
                <tr>
                    <td></td><td><input type="submit" value="Create Label" /></td>
                </tr>
                </table>
            </fieldset>
        </div>
        <%=Html.ActionLink("List", "List") %>
    </form>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">

</asp:Content>
