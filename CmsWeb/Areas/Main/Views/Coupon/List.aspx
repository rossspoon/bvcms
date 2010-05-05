<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.CouponModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>List</title>
    <style>
        .alt 
        {
            background-color: #eee;
        }
    </style>
</head>
<body style="font-family:Arial">
    <table cellpadding="4" cellspacing="0">
        <tr>
            <th></th>
            <th>
                Coupon
            </th>
            <th>
                Purchaser
            </th>
            <th>
                Amount
            </th>
            <th>
                Registration
            </th>
            <th>
                Created
            </th>
            <th>
                Person
            </th>
            <th>
                Used
            </th>
            <th>
                Canceled
            </th>
        </tr>

    <%  var n = 0;
        foreach (var item in Model.Coupons()) 
        {
            n++; %>
        <tr <%= (n % 2) == 0? "class='alt'" : "" %>>
            <td>
                <%= Html.ActionLink("Cancel", "Cancel", new { id=item.Code })%>
            </td>
            <td>
                <span style="font-family:Courier New"><%= Html.Encode(item.Coupon) %></span>
            </td>
            <td>
                <%= Html.Encode(item.Name) %>
            </td>
            <td align="right">
                <%= Html.Encode(item.Amount.ToString2("C")) %>
            </td>
            <td>
                <%= item.OrgDivName %>
            </td>
            <td>
                <%= Html.Encode(item.Created.ToString("M/d/yy h:mm tt")) %>
            </td>
            <td>
                <%= Html.Encode(item.Person) %>
            </td>
            <td>
                <%= Html.Encode(item.Used.ToString2("M/d/yy h:mm tt"))%>
            </td>
            <td>
                <%= Html.Encode(item.Canceled.ToString2("M/d/yy h:mm tt"))%>
            </td>
        </tr>
    
    <% } %>

    </table>

    <p>
        <%= Html.ActionLink("Coupon", "Index") %>
    </p>

</body>
</html>

