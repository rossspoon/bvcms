<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/bvorg.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.PaymentModel>" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
	<title>Complete Registration Payment</title>
	<style>
div.terms {
   width:600px;
   height:200px;
   border:1px solid #ccc;
   background:#f2f2f2;
   padding:6px;
   overflow:auto;
}
div.terms p,
div.terms li {font:normal 11px/15px arial;color:#333;}
div.terms h3 {font:bold 14px/19px arial;color:#000;}
div.terms h4 {font:bold 12px/17px arial;color:#000;}
div.terms strong {color:#000;}	</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script src="/Content/js/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="/Content/js/jquery.idle-timer.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function() {
            $(document).bind("idle.idleTimer", function() {
                window.location.href = '<%=ViewData["URL"] %>';
            });
            var tmout = parseInt('<%=ViewData["timeout"] %>');

            $(document).bind("keydown", function() {
                $(document).unbind("keydown");
                $.idleTimer(tmout);
            });
            $.idleTimer(tmout);
        });
    </script>
    <script type="text/javascript">
        $(function() {
            if ($('#IAgree').attr("id"))
                $("#Submit").attr("disabled", "disabled");
            $("#IAgree").click(function() {
                var checked_status = this.checked;
                if (checked_status == true)
                    $("#Submit").removeAttr("disabled");
                else
                    $("#Submit").attr("disabled", "disabled");
            });
        });
    </script>

    <h2>Payment Processing</h2>
<%--<div class="terms"></div>--%>
<% if(Model.Terms.HasValue())
   { %>
<%=Model.Terms %>
<p><%=Html.CheckBox("IAgree") %> I agree to the above terms and conditions.</p>
<% } %>
<%--    <p>If you have a coupon, please enter that number here:
    <%=Html.TextBox("Coupon") %></p>
--%>    
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
<% if (Model.Terms.HasValue())
   { %>
    <p>
        You must agree to the terms above for you or your minor child before you can continue.</p>
<% } %>
    <p>
        When you click the Next button will be redirected to ServiceU.com to process your credit card payment of <%=Model.Amount.ToString("C") %>.
        After you are finished there, you will be redirected back here to get your confirmation.
        Your information will not be committed until you complete the transaction on the next page.
    </p>
    <p><%=Html.SubmitButton("Submit", "Next") %></p>
    </form>

</asp:Content>
