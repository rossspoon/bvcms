<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="True"
    Inherits="Forum_Thread" Title="Bellevue Teacher Forum Topic Thread" Codebehind="Thread.aspx.cs" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="Server">
    <table border="0" cellpadding="10" cellspacing="0">
        <tr valign="top">
            <td>
                <asp:UpdatePanel ID="UpdateTreePanel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TreeView ID="tree" runat="server" OnSelectedNodeChanged="tree_SelectedNodeChanged"
                            ShowLines="True" Font-Size="Smaller">
                        </asp:TreeView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" EnableViewState="false" runat="server">
                    <ContentTemplate>
                        <div class="ForumPostButtons" id="PostButtons" style="visibility: visible">
                            <div>
                                <asp:HyperLink ID="Reply" runat="server" CssClass="CommonImageTextButton CommonReplyButton"
                                    EnableViewState="False">Reply</asp:HyperLink>
                                <asp:HyperLink ID="Edit" runat="server" CssClass="CommonImageTextButton CommonEditButton"
                                    EnableViewState="False">Edit</asp:HyperLink><br />
                            </div>
                        </div>
                        <cc1:ForumEntryDisplay ID="ForumEntryDisplay1" runat="server"></cc1:ForumEntryDisplay>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="tree" EventName="SelectedNodeChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
