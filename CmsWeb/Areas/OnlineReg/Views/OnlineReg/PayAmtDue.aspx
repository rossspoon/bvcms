<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/onlinereg.Master" Inherits="System.Web.Mvc.ViewPage<CmsData.Transaction>" %>
<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <%= SquishIt.Framework.Bundle.JavaScript()
        .Add("/Content/js/jquery-1.4.4.js")
        .Add("/Content/js/jquery-ui-1.8.9.custom.js")
        .Add("/Content/js/jquery.idle-timer.js")
        .Add("/Content/js/jquery.showpassword.js")
        .Add("/Content/js/jquery.validate.js")
        .Add("/Scripts/OnlineRegPayment.js")
        .Render("/Content/OnLineRegPayment_#.js")
    %>
    <script type="text/javascript">
        $(function() {
            $(document).bind("idle.idleTimer", function() {
                window.location.href = '<%=Model.Url %>';
            });
            var tmout = parseInt('<%=ViewData["timeout"] %>');
            $.idleTimer(tmout);
        });
    </script>
    
    <h2>Payment Processing</h2>
    
    <form action="https://public.serviceu.com/transaction/pay.asp" method="post">
    <%=Html.Hidden("OrgID", Model.ServiceUOrgID) %>
    <%=Html.Hidden("OrgAccountID", Model.ServiceUOrgAccountID) %>
    <%=Html.Hidden("PostbackURL", Util.ServerLink("/OnlineReg/ConfirmDuePaid/" + Model.Id))%>
    <%=Html.Hidden("NameOnAccount", Model.Name) %> 
    <%=Html.Hidden("Address", Model.Address) %>
    <%=Html.Hidden("City", Model.City) %>
    <%=Html.Hidden("State", Model.State)%>
    <%=Html.Hidden("PostalCode", Model.Zip)%>
    <%=Html.Hidden("Phone", Model.Phone) %>
    <%=Html.Hidden("Email", Util.FirstAddress(Model.Emails)) %>
    <%=Html.Hidden("Misc1", Model.Name)%>
    <%=Html.Hidden("Misc2", Model.Description) %>
    <%=Html.Hidden("Misc3") %>
    <%=Html.Hidden("Misc4") %>
    <p>
        When you click the 'Pay with Credit Card' button you will be redirected to ServiceU.com to process your credit card payment of $<%=Model.Amt.Value.ToString("N") %>.
        After you are finished there, you will be redirected back here to get your confirmation.
        Your information will not be committed until you complete the transaction on the next page.
    </p>
    <p>
    <%=Html.TextBox("Amount", Model.Amt.Value.ToString("f2")) %>
    <input type="submit" name="Submit" value="Pay with Credit Card" /></p>
    <p>If you have a coupon, please enter that number here and click the button next to it:</p>
    <%=Html.TextBox("Coupon") %>
    <input type="button" href="/OnlineReg/PayWithCoupon2/<%=Model.Id%>" class="submitbutton ajax"
        value="Pay with coupon" />
    <span style="color:Red" id="validatecoupon"></span>
    </form>
</asp:Content>