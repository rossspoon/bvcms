<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site2.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.PaymentModel>" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
	<title>Complete Registration Payment</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Payment Processing</h2>
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
    <p>
        When you click the Next button will be redirected to ServiceU.com to process your credit card payment of <%=Model.Amount.ToString("C") %>.
        After you are finished there, you will be redirected back here to get your confirmation.
        Your information will not be committed until you complete the transaction on the next page.
    </p>
    <p><%=Html.SubmitButton("Submit", "Next") %></p>
    </form>

</asp:Content>
