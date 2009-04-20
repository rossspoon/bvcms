<%@ Page Language="C#" AutoEventWireup="True" MasterPageFile="~/site.master" 
Inherits="AdminGroups" Title="Roles Administration" Codebehind="AdminGroups.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="main" class="wide">
        <asp:HiddenField ID="HiddenField1" runat="server" />
        <br />
        Select Group to Edit
        <asp:DropDownList ID="DropDownList1" runat="server" EnableViewState="false" DataSourceID="ObjectDataSource3"
            AutoPostBack="True" DataTextField="Name" DataValueField="Id" OnDataBound="DropDownList1_DataBound"
            OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
        </asp:DropDownList><br />
        <asp:ObjectDataSource ID="ObjectDataSource3" runat="server" SelectMethod="FetchAdminGroups"
            TypeName="DiscData.GroupController"></asp:ObjectDataSource>
        <asp:UpdatePanel ID="UpdatePanel1" EnableViewState="false" runat="server">
            <ContentTemplate>
                <div>
                    <asp:HyperLink ID="HyperLink1" runat="server">Edit Welcome Text</asp:HyperLink></div>
                <br />
                <div>
                    <h4>
                        Invitations</h4>
                    <asp:GridView ID="Invitations" runat="server" EnableViewState="false" AutoGenerateColumns="False" EmptyDataText="There are no invitations."
                        DataSourceID="ObjectDataSource1" SkinID="subsonicSkin" DataKeyNames="password">
                        <Columns>
                            <asp:BoundField DataField="Password" HeaderText="Password" SortExpression="Password" />
                            <asp:BoundField DataField="Expires" HeaderText="Expires" SortExpression="Expires" />
                            <asp:CommandField ShowDeleteButton="True" />
                        </Columns>
                    </asp:GridView>
                    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" DeleteMethod="Delete"
                        SelectMethod="FetchInvitesForGroup" TypeName="DiscData.InvitationController" UpdateMethod="Update">
                        <DeleteParameters>
                            <asp:Parameter Name="Password" Type="Object" />
                        </DeleteParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="Password" Type="String" />
                            <asp:Parameter Name="Groupname" Type="String" />
                            <asp:Parameter Name="Expires" Type="DateTime" />
                        </UpdateParameters>
                        <SelectParameters>
                            <asp:ControlParameter ControlID="DropDownList1" Name="id" PropertyName="SelectedValue"
                                Type="Int32" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <p>
                        Password:
                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>&nbsp;<asp:Button ID="AddInvite"
                            runat="server" OnClick="AddInvite_Click" Text="Add Invitation" /><br />
                    </p>
                    <h4>
                        Users</h4>
                </div>
                <div>
                    <asp:GridView ID="UsersGrid" runat="server" AutoGenerateColumns="False" DataKeyNames="username"
                        DataSourceID="ObjectDataSource2" EmptyDataText="There are no matching users in the system."
                        Font-Italic="False" SkinID="subsonicSkin">
                        <EmptyDataRowStyle Font-Italic="True" />
                        <Columns>
                            <asp:TemplateField HeaderText="Active">
                                <ItemStyle HorizontalAlign="Center" Width="40px" />
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="true" Checked='<%#DataBinder.Eval(Container.DataItem, "IsApproved")%>'
                                        OnCheckedChanged="EnabledChanged" Value='<%#DataBinder.Eval(Container.DataItem, "UserName")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="User Name">
                                <ItemStyle Width="160px" />
                                <ItemTemplate>
                                    <a href='Admin/users_edit.aspx?username=<%#Eval("UserName")%>'>
                                        <%#DataBinder.Eval(Container.DataItem, "UserName")%>
                                    </a>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="LastLoginDate" HeaderText="Last Login" SortExpression="LastLoginDate">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                    <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" SelectMethod="GetUsersInGroup"
                        TypeName="DiscData.GroupController">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="DropDownList1" Name="id" PropertyName="SelectedValue"
                                Type="Int32" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="DropDownList1" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
