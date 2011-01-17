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
                window.location.href = '<%=Model.ti.Url %>';
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
    <form action="/OnlineReg/PayWithCoupon/<%=Model.ti.DatumId %>" method="post">
    <p>
        If you have received a Cash Payment Code, please enter that number here and click
        the blue link next to it:</p>
    <input id="Coupon" type="password" name="Coupon" value='<%=ViewData["Coupon"] %>' />
    <a href="/OnlineReg/PayWithCoupon/<%=Model.ti.DatumId %>" class="submitbutton">Apply Coupon</a>
    <div><%=Html.ValidationMessage("coupon") %></div>
    </form>
    <form action="/onlinereg/ProcessPayment/<%=Model.ti.DatumId %>" method="post">
    <%=Html.Hidden("pf.ti.DatumId", Model.ti.DatumId) %>
    <%=Html.Hidden("pf.ti.Amt", Model.ti.Amt) %>
    <%=Html.Hidden("pf.ti.Url", Model.ti.Url) %>
    <table>
    <tr><td>Name</td><td><%=Html.TextBox("pf.ti.Name", Model.ti.Name) %></td></tr>
    <tr><td>Address</td><td><%=Html.TextBox("pf.ti.Address", Model.ti.Address) %></td></tr>
    <tr><td>City</td><td><%=Html.TextBox("pf.ti.City", Model.ti.City) %></td></tr>
    <tr><td>State</td><td><%=Html.TextBox("pf.ti.State", Model.ti.State) %></td></tr>
    <tr><td>Zip</td><td><%=Html.TextBox("pf.ti.Zip", Model.ti.Zip) %></td></tr>
    <tr><td>Phone</td><td><%=Html.TextBox("pf.ti.Phone", Model.ti.Phone) %></td></tr>
    <tr><td>Amount to Pay</td><td><%=Model.ti.Amt.ToString2("N") %></td></tr>
    <tr><td>CreditCard</td><td><%=Html.TextBox("pf.CreditCard") %></td></tr>
    <tr><td>CCV</td><td><%=Html.TextBox("pf.CCV") %></td></tr>
    <tr><td>Expires</td><td><%=Html.TextBox("pf.Expires") %></td></tr>
    </table>
    <p>
        <input id="Submit" type="submit" name="Submit" value="Pay with Credit Card" /><br />
        <%=Html.ValidationMessage("form") %>
        </p>
    </form>
</asp:Content>
