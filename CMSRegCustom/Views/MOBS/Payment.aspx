<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/bvorg.Master" Inherits="System.Web.Mvc.ViewPage<CMSRegCustom.Models.MOBSModel>" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
	<title>MOBS Event Registration</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Payment Processing</h2>
    <p>
        You will now be redirected to ServiceU.com to process your credit card payment of <%=Model.Amount.ToString("C") %>.
        After you are finished there, you will be redirected back here to get your confirmation.
    </p>
    <form action="https://public.serviceu.com/transaction/pay.asp" method="post">
    <%=Html.Hidden("OrgID", DbUtil.Settings("ServiceUOrgID", "0")) %>
    <%=Html.Hidden("OrgAccountID", DbUtil.Settings("ServiceUOrgAccountID", "0")) %>
    <%=Html.Hidden("Amount", Model.Amount) %>
    <%=Html.Hidden("PostbackURL", Request.Url.Scheme + "://" + Request.Url.Authority + "/MOBS/Confirm/" + Model.regid) %>
    <%=Html.Hidden("NameOnAccount", Model.person.Name) %> 
    <%=Html.Hidden("Address", Model.person.PrimaryAddress) %>
    <%=Html.Hidden("City", Model.person.PrimaryCity)%>
    <%=Html.Hidden("State", Model.person.PrimaryState)%>
    <%=Html.Hidden("PostalCode", Model.person.PrimaryZip.Zip5())%>
    <%=Html.Hidden("Phone", Model.person.Family.HomePhone.FmtFone())%>
    <%=Html.Hidden("Email", Model.person.EmailAddress) %>
    <%=Html.Hidden("Misc1", Model.person.Name)%>
    <%=Html.Hidden("Misc2", Model.meeting.Organization.OrganizationName) %>
    <%=Html.SubmitButton("Submit", "Next") %>
    </form>

</asp:Content>
