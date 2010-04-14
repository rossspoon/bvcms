<%@ Page Title="" StylesheetTheme="Standard" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="BadEts.aspx.cs" Inherits="CMSWeb.BadEts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
        var tid = 0;
        var tdt = '';
        function EditDate(id,dt) {
            tid = id;
            $('#tdate').val(dt);
            $.blockUI({ message: $('#changeTDform') });
        }
        function Cancel() {
            $.unblockUI();
        }
        function ChangeIt() {
            tdt = $('#tdate').val();
            $("#<%=NewDate.ClientID%>").val(tdt);
            $("#<%=TranId.ClientID%>").val(tid);
            $("#<%=ChangeDate.ClientID%>").click();
        }
    </script>

    <asp:HiddenField ID="NewDate" runat="server" />    
    <asp:HiddenField ID="TranId" runat="server" />
    <asp:Button ID="ChangeDate" runat="server" Text="Button" style="display:none" onclick="ChangeDate_Click" />
    <asp:LinkButton ID="RunAnalysis" OnClientClick="$.blockUI()" style="font-size:larger" runat="server" 
        onclick="RunAnalysis_Click">Run Analysis</asp:LinkButton>
    &nbsp;&nbsp;
    <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" AppendDataBoundItems="true"
        DataSourceID="ObjectDataSource1" DataTextField="Value" DataValueField="Id">
        <asp:ListItem Value="0">(not specified)</asp:ListItem>
    </asp:DropDownList>
    <asp:Label ID="NumberPeople" runat="server" Text="Number of People/Orgs"></asp:Label>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" OldValuesParameterFormatString="original_{0}"
        SelectMethod="BadETCodes" TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
    <br />
    <asp:GridView ID="GridView1" runat="server" AllowPaging="True"
        AutoGenerateColumns="False" DataSourceID="ObjectDataSource2" 
        PageSize="200" SkinID="GridViewSkin" onrowcommand="GridView1_RowCommand" 
        onrowdatabound="GridView1_RowDataBound" EnableViewState="false">
        <Columns>
            <asp:BoundField DataField="Flag" HeaderText="Flag" />
            <asp:BoundField DataField="TranId" HeaderText="TranId" />
            <asp:TemplateField HeaderText="TranDt">
                <ItemTemplate>
                    <asp:Label ID="Label1" tid='<%# Eval("TranId") %>' Font-Strikeout='<%# Eval("Status") %>' runat="server" Text='<%# Eval("TranDt") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="TranType" HeaderText="TranType" />
            <asp:HyperLinkField DataNavigateUrlFields="PeopleId,OrgId" 
                DataNavigateUrlFormatString="/AttendStrDetail.aspx?id={0}&amp;oid={1}"
                Target="AttendStrDetail"
                DataTextField="Name" HeaderText="Name" />
            <asp:BoundField DataField="OrgId" HeaderText="OrgId" />
            <asp:HyperLinkField DataNavigateUrlFields="OrgId" DataNavigateUrlFormatString="/Organization/Index/{0}"
                DataTextField="OrgName" HeaderText="OrgName" />
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Time++"
                        Text="Time++" CommandArgument='<%#Eval("TranId")%>'></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Disable"
                        Text="Disable" CommandArgument='<%#Eval("TranId")%>'></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False" CommandName="Add5"
                        Text="Add 5" CommandArgument='<%#Eval("TranId")%>'></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:LinkButton ID="LinkButton4" runat="server" CausesValidation="False" CommandName="Ins1"
                        Text="Ins 1" CommandArgument='<%#Eval("TranId")%>'></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:HyperLink ID="HyperLink1" runat="server">EditDate</asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" EnablePaging="True" MaximumRowsParameterName="max"
        SelectCountMethod="Count" SelectMethod="FetchBadEts"
        StartRowIndexParameterName="start" TypeName="CMSWeb.BadEtsController" EnableViewState="false">
        <SelectParameters>
            <asp:ControlParameter ControlID="DropDownList1" Name="flag" PropertyName="SelectedValue"
                Type="Int32" />
            <asp:Parameter Name="start" Type="Int32" />
            <asp:Parameter Name="max" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <div id="changeTDform" style="display: none">
        <p>
            <label>
                Date:</label><input type="text" id="tdate" /></p>
        <p>
            <input type="button" value="change" onclick="ChangeIt()" />
            <input type="button" value="cancel" onclick="Cancel()" />
            </p>
    </div>
</asp:Content>
