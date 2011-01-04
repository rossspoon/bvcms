<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/onlinereg.Master"
    Inherits="System.Web.Mvc.ViewPage<CmsWeb.Models.PaymentForm>" %>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <%= SquishIt.Framework.Bundle.JavaScript()
        .Add("/Content/js/jquery-1.4.2.js")
        .Add("/Content/js/jquery-ui-1.8.2.custom.js")
        .Add("/Content/js/jquery.idle-timer.js")
        .Add("/Content/js/jquery.showpassword.js")
        .Add("/Scripts/OnlineRegPayment.js")
        .Render("/Content/OnLineRegPayment_#.js")
    %>
    <script type="text/javascript">
        $(function () {
            $(document).bind("idle.idleTimer", function () {
                window.location.href = '<%=Model.Url %>';
            });
            var tmout = parseInt('<%=ViewData["timeout"] %>');
            $.idleTimer(tmout);
        });
    </script>
    <h2>
        Payment Processing</h2>
<% if(ViewData.ContainsKey("Terms"))
   { %>
    <a id="displayterms" title="click to display terms" href="#">Display Terms</a>
    <div id="Terms" title="Terms of Agreement" class="modalPopup" style="display: none;
        width: 400px; padding: 10px">
        <%=ViewData["Terms"] %></div>
    <p>
        <%=Html.CheckBox("IAgree") %>
        I agree to the above terms and conditions.</p>
    <p>
        You must agree to the terms above for you or your minor child before you can continue.</p>
<% } %>
    <p>
    </p>
    <form action="/onlinereg/ProcessPayment/<%=Model.DatumId %>" method="post">
    <%=Html.Hidden("pf.DatumId", Model.DatumId) %>
    <%=Html.Hidden("pf.Amt", Model.Amt) %>
    <%=Html.Hidden("pf.Url", Model.Url) %>
    <table>
    <tr><td>Name</td><td><%=Html.TextBox("pf.Name", Model.Name) %></td></tr>
    <tr><td>Address</td><td><%=Html.TextBox("pf.Address", Model.Address) %></td></tr>
    <tr><td>City</td><td><%=Html.TextBox("pf.City", Model.City) %></td></tr>
    <tr><td>State</td><td><%=Html.TextBox("pf.State", Model.State) %></td></tr>
    <tr><td>Zip</td><td><%=Html.TextBox("pf.Zip", Model.Zip) %></td></tr>
    <tr><td>Phone</td><td><%=Html.TextBox("pf.Phone", Model.Phone) %></td></tr>
    <tr><td>Amount to Pay</td><td><%=Model.Amt.ToString("N") %></td></tr>
    <tr><td>CreditCard</td><td><%=Html.TextBox("pf.CreditCard") %></td></tr>
    <tr><td>CCV</td><td><%=Html.TextBox("pf.CCV") %></td></tr>
    <tr><td>Expires</td><td><%=Html.TextBox("pf.Expires") %></td></tr>
    </table>
    <p>
        <input id="Submit" type="submit" name="Submit" value="Pay with Credit Card" /><br />
        <%=Html.ValidationMessage("form") %>
        </p>
    </form>
    <form action="/OnlineReg/PayWithCoupon/<%=Model.DatumId %>" method="post">
    <p>
        If you have received a Cash Payment Code, please enter that number here and click
        the blue link next to it:</p>
    <input id="Coupon" type="password" name="Coupon" value='<%=ViewData["Coupon"] %>' />
    <a href="/OnlineReg/PayWithCoupon/<%=Model.DatumId %>" class="submitbutton">Pay with coupon</a>
    <span style="color: Red" id="validatecoupon"></span>
    </form>
</asp:Content>
