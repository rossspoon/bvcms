<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.CouponModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Create</title>
</head>
<body>
    <fieldset>
        <legend>Registration Coupon</legend>
        <table cellpadding="5">
        <tr>
        <td>Registration</td>
        <td><%= Html.Encode(Model.Registration()) %></td>
        </tr>
        <tr>
        <td>Amount</td>
        <td><%= Html.Encode(String.Format("{0:C}", Model.amount)) %></td>
        </tr>
        <tr>
        <td>Name</td>
        <td><%= Html.Encode(Model.name) %></td>
        </tr>
        <tr>
        <td>Coupon Code</td>
        <td><span style="font-family: Courier New;font-weight:bold"><%= Html.Encode(Model.couponcode) %></span></td>
        </tr>
        </table>
    </fieldset>
    <p>
        <%= Html.ActionLink("Back to List", "Index") %>
    </p>

</body>
</html>

