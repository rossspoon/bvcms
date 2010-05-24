<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/onlinereg.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.PaymentModel>" %>
<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <script src="/Content/js/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="/Content/js/jquery.idle-timer.js" type="text/javascript"></script>
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
    <%=Html.Hidden("Amount") %>
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
    <%=Html.Hidden("_datumid") %>
    <%=Html.Hidden("_timeout") %>
    <%=Html.Hidden("_URL") %>
    <%=Html.Hidden("_confirm") %>
    <p>
        When you click the 'Pay with Credit Card' button button will be redirected to ServiceU.com to process your credit card payment of <%=Model.Amount.ToString("C") %>.
        After you are finished there, you will be redirected back here to get your confirmation.
        Your information will not be committed until you complete the transaction on the next page.
    </p>
    <p><%=Html.SubmitButton("Submit", "Pay with Credit Card") %></p>
    <p>If you have a coupon, please enter that number here and click the blue link next to it:</p>
    <%=Html.TextBox("_Coupon") %>
    <a href="/OnlineReg/PayWithCoupon/" class="submitbutton">Pay with coupon</a>
    <span style="color:Red" id="validatecoupon"></span>
    </form>
</asp:Content>