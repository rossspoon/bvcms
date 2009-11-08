<%@ Page StylesheetTheme="Default" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="True"
    Inherits="Verse_Default" Title="Verse Memorization Database" Codebehind="Default.aspx.cs" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="Server">

    <script type="text/javascript">
function center(width, height)
{
    width = Math.min(screen.availWidth, width);
    height = Math.min(screen.availHeight, height);
    topMargin = parseInt((screen.availHeight - height)/2);
    leftMargin = parseInt((screen.availWidth - width)/2);
    return 'width='+width+',height='+height+',top='+topMargin+',left='+leftMargin;
}
function OpenWin(w, wURL)
{
    opts='resizable=yes,scrollbars=no,toolbar=no,location=no,directories=no,status=yes,menubar=no,';
    opts += center(775, 550);
    window.open(wURL,w,opts);
}
    </script>

    <div id="main" class="wide">
            <h1>
                Verses for Selected Category</h1>
            Category:
            <cc1:DropDownCC ID="Category" runat="server" DataMember="VerseCategories" DataTextField="DisplayName"
                DataValueField="Id" OnSelectedIndexChanged="Category_SelectedIndexChanged" AutoPostBack="True"
                EnableViewState="False">
            </cc1:DropDownCC>
            <asp:CheckBox ID="IncludeAdmin" runat="server" Text="Include Master" AutoPostBack="True" />
            <asp:HyperLink ID="EditCategories" runat="server" NavigateUrl="~/Verse/Category.aspx"
                EnableViewState="False" CssClass="CommonImageTextButton CommonEditButton">Edit Categories</asp:HyperLink>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" RenderMode="Inline">
                <ContentTemplate>
                    <asp:HyperLink ID="AddVerse" runat="server" EnableViewState="False" CssClass="CommonImageTextButton CommonAddButton">Add Verse</asp:HyperLink>
                    <asp:HyperLink ID="SelectVerses" runat="server" EnableViewState="False" CssClass="CommonImageTextButton CommonCheckListButton">Select Verses</asp:HyperLink>
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" EmptyDataText="There are no data records to display."
                        CellPadding="4" DataKeyNames="id" ForeColor="#333333" GridLines="None" DataSourceID="VerseDataSource"
                        EnableViewState="False" SkinID="subsonicSkin">
                        <Columns>
                            <asp:BoundField DataField="RefAndVersion" HeaderText="Reference" />
                            <asp:BoundField DataField="VerseText" HeaderText="Text" SortExpression="Text" />
                            <asp:TemplateField HeaderText="Practice">
                                <ItemTemplate>
                                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# "javascript:OpenWin(\u0027practice\u0027,\u0027Practice/" + Eval("Id") + ".aspx" + "\u0027)" %>'>
                                        <asp:Image ID="Image1" ImageUrl="~/images/icons/checkdoc.png" runat="server" /></asp:HyperLink>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Remove">
                                <ItemTemplate>
                                    <asp:ImageButton ID="Delete" runat="server" ImageUrl="~/images/icons/minusdoc.png"
                                        CausesValidation="false" CommandName="Delete" Visible='<%# IsOwner %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <EditRowStyle BackColor="#999999" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="Category" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
            <asp:ObjectDataSource ID="VerseDataSource" runat="server" SelectMethod="GetSortedVerseCollectionFromCategory"
                TypeName="DiscData.VerseController" EnableViewState="False" DeleteMethod="RemoveVerseFromCategory">
                <DeleteParameters>
                    <asp:ControlParameter ControlID="Category" Name="CatId" PropertyName="SelectedValue"
                        Type="Int32" />
                    <asp:Parameter Name="Id" Type="Int32" />
                </DeleteParameters>
                <SelectParameters>
                    <asp:ControlParameter ControlID="Category" Name="CatId" PropertyName="SelectedValue"
                        Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
    </div>
</asp:Content>
