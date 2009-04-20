<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="True" Inherits="Agreement"
    Title="Agreement" CodeBehind="Agreement.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="main">
        <h1>
            Membership Use Agreement
        </h1>
        <cms:Paragraph ID="agreement" ContentName="termsofuse" EnableViewState="false" runat="server" />
    </div>
</asp:Content>
