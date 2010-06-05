<%@ Page Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CMSWeb.Default"
    Title="CMS2 Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table cellpadding="6" cellspacing="0" border="0" style="width: 100%; height: 100%">
            <tr valign="top">
                <td>
                    <table class="dashbox" cellpadding="2" cellspacing="0" width="100%">
                        <tr>
                            <td class="dashtitle">
                                Birthdays
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:HyperLink ID="BFClass" runat="server">BFClass</asp:HyperLink>
                                <div style="overflow: auto; height: 100px">
                                    <asp:GridView ID="Birthdays" runat="server" AutoGenerateColumns="False" ShowHeader="False"
        CellPadding="4" ForeColor="#333333" GridLines="None">
        <PagerSettings Position="TopAndBottom" />
        <FooterStyle BackColor="#3e8cb5" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#EFF3FB" />
        <PagerStyle CssClass="pagerstyle" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#3e8cb5" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#EFF3FB"/>
        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            <asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="~/Person/Index/{0}"
                                                DataTextField="Name" DataTextFormatString="{0}" />
                                            <asp:BoundField DataField="Birthday" DataFormatString="{0:MMM dd}" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table class="dashbox" cellpadding="2" cellspacing="0" width="100%">
                        <tr>
                            <td class="dashtitle">
                                Reports
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div style="overflow: auto; height: 125px;">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Report/ChurchAttendanceSummaryRpt.aspx"
                                                    Target="_blank">Church Attendance Summary</asp:HyperLink>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Report/ChurchAttendanceRpt.aspx"
                                                    Target="_blank">Church Attendance</asp:HyperLink>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/Report/BFCWeeklyAttendanceSummaryRpt.aspx"
                                                    Target="_blank">BFC Weekly Attendance Summary</asp:HyperLink>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="~/Report/BFCAvgWeeklyAttendanceRpt.aspx"
                                                    Target="_blank">BFC Average Weekly Attendance</asp:HyperLink>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:HyperLink ID="DecisionSummary" runat="server" NavigateUrl="~/Report/DecisionSummary.aspx"
                                                    Target="_blank">Decision Summary Report</asp:HyperLink>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table class="dashbox" cellpadding="2" cellspacing="0" width="100%">
                        <tr>
                            <td class="dashtitle">
                                My Involvement
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div style="overflow: auto; height: 105px;">
                                     <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" ShowHeader="False"
        CellPadding="4" ForeColor="#333333" GridLines="None">
        <PagerSettings Position="TopAndBottom" />
        <FooterStyle BackColor="#3e8cb5" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#EFF3FB" />
        <PagerStyle CssClass="pagerstyle" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#3e8cb5" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#EFF3FB"/>
        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            <asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="~/Person/Index/{0}"
                                                DataTextField="Name" DataTextFormatString="{0}" />
                                            <asp:BoundField DataField="Birthday" DataFormatString="{0:MMM dd}" />
                                        </Columns>
                                    </asp:GridView>

                                    <asp:GridView ID="grdMyInvolvement" runat="server" AutoGenerateColumns="False"
                                        EmptyDataText="No Current Enrollments Found."
                                        ShowHeader="false"
        CellPadding="4" ForeColor="#333333" GridLines="None">
        <PagerSettings Position="TopAndBottom" />
        <FooterStyle BackColor="#3e8cb5" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#EFF3FB" />
        <PagerStyle CssClass="pagerstyle" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#3e8cb5" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#EFF3FB"/>
        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            <asp:HyperLinkField DataNavigateUrlFields="Id" 
                                                DataTextField="Name" DataTextFormatString="{0}" />                                          
                                            <asp:BoundField DataField="MemberType" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <p><asp:LinkButton ID="UseOldNewDialog" runat="server" OnClick="UseDialog_Click"></asp:LinkButton></p>
                </td>
                <td>
                    <table class="dashbox" cellpadding="2" cellspacing="0" width="100%">
                        <tr>
                            <td class="dashtitle">
                                News
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <h4 style="font: verdana; color: #2b637d;font-size:.8em">
                                    <asp:HyperLink ID="BlogLink" runat="server" Target="_blank">CMS2 News</asp:HyperLink>
                                </h4>
                                <div style="overflow: auto; height: 600px;">
                                    <asp:GridView ID="NewsGrid" runat="server" AutoGenerateColumns="False" ShowHeader="False"
        CellPadding="4" ForeColor="#333333" GridLines="None">
        <PagerSettings Position="TopAndBottom" />
        <FooterStyle BackColor="#3e8cb5" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#EFF3FB" />
        <PagerStyle CssClass="pagerstyle" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#3e8cb5" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#EFF3FB"/>
        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("Url") %>' Target="_blank"
                                                        Text='<%# Eval("Title") %>'></asp:HyperLink>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("Published", "{0:d}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Author">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("Author") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table class="dashbox" cellpadding="2" cellspacing="0" width="100%">
                        <tr>
                            <td class="dashtitle">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <h4 style="font: verdana; color: #2b637d">
                                    <asp:HyperLink ID="HyperLink6" NavigateUrl="~/Task/List/" runat="server">My Tasks</asp:HyperLink>
                                </h4>
                                <div style="overflow: auto; height: 522px;">
                                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" ShowHeader="False"
                                        DataSourceID="ObjectDataSource1"
        CellPadding="4" ForeColor="#333333" GridLines="None">
        <PagerSettings Position="TopAndBottom" />
        <FooterStyle BackColor="#3e8cb5" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#EFF3FB" />
        <PagerStyle CssClass="pagerstyle" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#3e8cb5" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#EFF3FB"/>
        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            <asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="~/Task/List/{0}#detail"
                                                DataTextField="Description" HeaderText="Task" />
                                            <asp:HyperLinkField DataNavigateUrlFields="PeopleId" DataNavigateUrlFormatString="/Person/Index/{0}"
                                                DataTextField="Who" HeaderText="Who" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server"
        SelectMethod="FetchContactTasks" TypeName="CMSWeb.Models.TaskModel">
    </asp:ObjectDataSource>
</asp:Content>
