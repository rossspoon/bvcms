<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="True" Inherits="BellevueTeachers.PageView"
    Title="Resource Page" validateRequest="false" CodeBehind="PageView.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
<asp:Literal ID="EditorScript" runat="server">
</asp:Literal>
      <div id="main" class="wide">
  <asp:Panel ID="pnlPublic" runat="server" EnableViewState="false">
        <asp:Panel ID="Admin" runat="server" EnableViewState="false" CssClass="adminpanel">
            <center>
                <b>Admin</b></center>
            <table cellpadding="5" width="80">
                <tr>
                    <td width="10">
                        <asp:Image ID="Image2" runat="server" ImageUrl="~/images/icons/edit.gif" />
                    </td>
                    <td>
                        <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" OnClick="lnkEdit_Click" CausesValidation="false"></asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td width="10">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/icons/add.gif" />
                    </td>
                    <td>
                        <asp:HyperLink ID="lnkNewPage" runat="server" NavigateUrl="~/view/newpage.aspx">New</asp:HyperLink>
                    </td>
                </tr>
                <tr>
                    <td width="10">
                        <asp:Image ID="Image3" runat="server" ImageUrl="~/images/icons/addressbook.gif" />
                    </td>
                    <td>
                        <asp:HyperLink ID="HyperLink1" NavigateUrl="~/admin/cmspagelist.aspx" runat="server">Page List</asp:HyperLink>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <div>
            <h1>
                <%=thisPage.Title%>
            </h1>
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
            <div class="lastupdated">
                <i>Last updated by
                    <%=thisPage.ModifiedBy%>
                    on
                    <%=thisPage.ModifiedOn%>
                </i>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlEdit" EnableViewState="false" runat="server">
        <h1>
            <asp:Label ID="editorTitle" runat="server"></asp:Label></h1>
        <cms:ResultMessage ID="ResultMessage1" runat="server" />
        <div>
            <p>
                <b>Title</b><br />
                <asp:TextBox ID="txtTitle" runat="server" Width="461px"></asp:TextBox><br />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtTitle"
                    ErrorMessage="Required" Display="Dynamic"></asp:RequiredFieldValidator><br />
                <i>This shows at the top of each page.</i>
            </p>
            <p>
                <b>Body</b><br />
                <asp:TextBox ID="Body2" runat="server" TextMode="MultiLine" Height="700" Width="700" ></asp:TextBox>
            </p>
            <p>
                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" />
            </p>
        </div>

        <script type="text/javascript">
function CheckDelete()
{
    return confirm("Delete this page? This action cannot be undone...");
}
        </script>

    </asp:Panel>
</div>
</asp:Content>
