<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/onlinereg.Master"
    Inherits="System.Web.Mvc.ViewPage<CmsWeb.Models.PaymentForm>" %>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <%= SquishIt.Framework.Bundle.JavaScript()
        .Add("/Content/js/jquery-1.4.2.js")
        .Add("/Content/js/jquery-ui-1.8.2.custom.js")
        .Add("/Content/js/jquery.idle-timer.js")
        .Add("/Content/js/jquery.showpassword-1.0.js")
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
            $('#Coupon').showPassword('#showpassword');
        });
    </script>
<div class="regform" style="width:400px">
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
    <form action="/onlinereg/ProcessPayment/<%=Model.ti.DatumId %>" method="post">
    <%=Html.Hidden("pf.ti.DatumId", Model.ti.DatumId) %>
    <%=Html.Hidden("pf.ti.Url", Model.ti.Url) %>
    <%=Html.Hidden("pf.ti.Amt", Model.ti.Amt) %>
    <table width="100%">
    <col align="right" width="150" />
    <col align="left" />
    <tr><td>Name:</td>
        <td><%=Html.TextBox("pf.ti.Name", Model.ti.Name, new { @class = "wide" })%></td></tr>
    <tr><td>Address:</td>
        <td><%=Html.TextBox("pf.ti.Address", Model.ti.Address, new { @class = "wide" })%></td></tr>
    <tr><td>City:</td>
        <td><%=Html.TextBox("pf.ti.City", Model.ti.City, new { @class = "wide" }) %></td></tr>
    <tr><td>State:</td>
        <td> <%=Html.TextBox("pf.ti.State", Model.ti.State, new { @class = "wide" }) %></td></tr>
    <tr><td>Zip:</td>
        <td> <%=Html.TextBox("pf.ti.Zip", Model.ti.Zip, new { @class = "wide" }) %></td></tr>
    <tr><td>Phone:</td>
        <td><%=Html.TextBox("pf.ti.Phone", Model.ti.Phone, new { @class = "wide" }) %></td></tr>
    <tr><td>Email:</td>
        <td><%=Html.TextBox("pf.ti.Emails", Model.ti.Emails, new { @class = "wide" }) %></td></tr>
    <tr><td>Amount to Pay:</td>
        <td><span class="right"><%=Model.ti.Amt.ToString2("N2")%></span></td></tr>
    <tr><td>Credit Card #:</td>
        <td><%=Html.TextBox("pf.CreditCard", Model.CreditCard, new { @class = "wide", autocomplete = "off" }) %></td></tr>
    <tr><td nowrap="nowrap">CC Security Code #:</td>
        <td><%=Html.TextBox("pf.CCV", Model.CCV, new { @class = "short", autocomplete = "off" }) %></td></tr>
    <tr><td>Expires (MMYY):</td>
        <td><%=Html.TextBox("pf.Expires", Model.Expires, new { @class = "wide", autocomplete = "off" }) %></td></tr>
    <tr><td></td>
        <td height="40"><input id="Submit" type="submit" name="Submit" class="submitbutton" value="Pay with Credit Card" />
            <div class="column"><%=Html.ValidationMessage("form") %></div></td></tr>
    <tr><td colspan="2">&nbsp;</td></tr>
    <tr><td>Payment Code:</td>
        <td><%=Html.Password("Coupon", ViewData["Coupon"], new { @class = "wide", autocomplete = "off" }) %><br />
            <input id="showpassword" type="checkbox" /> Show Code</td></tr>
    <tr><td></td>
        <td height="40"><a href="/OnlineReg/PayWithCoupon/<%=Model.ti.DatumId %>" class="submitbutton">Apply Code</a>
        <div class="right red" id="validatecoupon"></div></td></tr>
    </table>
    </form>
</div>
</asp:Content>
