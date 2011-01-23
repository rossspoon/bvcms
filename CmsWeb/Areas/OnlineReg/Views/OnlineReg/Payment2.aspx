<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/onlinereg.Master" Inherits="System.Web.Mvc.ViewPage<CmsWeb.Models.PaymentModel>" %>
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
        $(function() {
            $(document).bind("idle.idleTimer", function() {
                window.location.href = '<%=Model._URL %>';
            });
            var tmout = parseInt('<%=Model._timeout %>');
            $.idleTimer(tmout);
        });
    </script>
    
    <h2>Payment Processing</h2>
    
    <form action="https://public.serviceu.com/transaction/pay.asp" method="post">
    <%=Html.Hidden("OrgID") %>
    <%=Html.Hidden("OrgAccountID") %>
    <%=Html.Hidden("PostbackURL") %>
    <%=Html.Hidden("NameOnAccount") %> 
    <%=Html.Hidden("Address") %>
    <%=Html.Hidden("City") %>
    <%=Html.Hidden("State")%>
    <%=Html.Hidden("PostalCode")%>
    <%=Html.Hidden("Phone") %>
    <%=Html.Hidden("Email") %>
    <%=Html.Hidden("Misc1")%>
    <%=Html.Hidden("Misc2") %>
    <%=Html.Hidden("Misc3") %>
    <%=Html.Hidden("Misc4") %>
    <p>
        When you click the 'Pay with Credit Card' button button will be redirected to ServiceU.com to process your credit card payment of <%=Model.Amount.ToString("C") %>.
        After you are finished there, you will be redirected back here to get your confirmation.
        Your information will not be committed until you complete the transaction on the next page.
    </p>
    <p>
    <%=Html.TextBox("Amount", Model.Amount.ToString("f2")) %>
    <input type="submit" name="Submit" value="Pay with Credit Card" /></p>
    </form>
    <form>
    <%=Html.Hidden("Amount") %>
    <p>If you have a coupon, please enter that number here and click the blue link next to it:</p>
    <%=Html.TextBox("Coupon") %>
    <a href="/OnlineReg/PayWithCouponOld/<%=Model._datumid %>" class="submitbutton">Pay with coupon</a>
    <span style="color:Red" id="validatecoupon"></span>
    </form>
</asp:Content>