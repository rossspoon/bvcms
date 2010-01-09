<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site2.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.SalesModel>" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
	<title>Resource Purchase</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Payment Processing</h2>
    <p>
        You will now be redirected to ServiceU.com to process your credit card payment of <%=Model.amount.ToString("C") %>.
        After you are finished there, you will be redirected back here to get your confirmation.
    </p>
    <form action="https://public.serviceu.com/transaction/pay.asp" method="post">
    <%=Html.Hidden("OrgID", Model.ServiceUOrgID) %>
    <%=Html.Hidden("OrgAccountID", Model.ServiceUOrgAccountID) %>
    <%=Html.Hidden("Amount", Model.amount) %>
    <%=Html.Hidden("PostbackURL", Request.Url.Scheme + "://" + Request.Url.Authority + "/Sales/Confirm/" + Model.tranid) %>
    <%=Html.Hidden("NameOnAccount", Model.person.Name) %> 
    <%=Html.Hidden("Address", Model.person.PrimaryAddress) %>
    <%=Html.Hidden("City", Model.person.PrimaryCity)%>
    <%=Html.Hidden("State", Model.person.PrimaryState)%>
    <%=Html.Hidden("PostalCode", Model.person.PrimaryZip.Zip5())%>
    <%=Html.Hidden("Phone", Model.person.Family.HomePhone.FmtFone())%>
    <%=Html.Hidden("Email", Model.person.EmailAddress) %>
    <%=Html.Hidden("Misc1", Model.person.Name)%>
    <%=Html.Hidden("Misc2", Model.Description) %>
    <%=Html.SubmitButton("Submit", "Next") %>
    </form>

</asp:Content>
