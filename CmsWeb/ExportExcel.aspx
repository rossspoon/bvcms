<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExportExcel.aspx.cs" Inherits="CmsWeb.ExportExcel1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns = "false" Font-Names = "Arial" >
            <Columns>
                <asp:TemplateField HeaderText="Picture" ItemStyle-Height = "200" ItemStyle-Width = "160">
                    <ItemTemplate>
                        <img src='<%# Eval("Image") %>' runat="server" width="160" height="200" />
                    </ItemTemplate> 
                </asp:TemplateField> 
                <asp:BoundField DataField = "PeopleId" HeaderText = "PeopleId"/>
                <asp:BoundField DataField = "Title" HeaderText = "Title"/>
                <asp:BoundField DataField = "FirstName" HeaderText = "First" />
                <asp:BoundField DataField = "LastName" HeaderText = "Last" />
                <asp:BoundField DataField = "Address" HeaderText = "Address1" />
                <asp:BoundField DataField = "Address2" HeaderText = "Address2" />
                <asp:BoundField DataField = "City" HeaderText = "City" />
                <asp:BoundField DataField = "State" HeaderText = "State" />
                <asp:BoundField DataField = "Zip" HeaderText = "Zip" />
                <asp:BoundField DataField = "Email" HeaderText = "Email" />
                <asp:BoundField DataField = "BirthDate" HeaderText = "BirthDate" />
                <asp:BoundField DataField = "BirthDay" HeaderText = "BirthDay" />
                <asp:BoundField DataField = "WeddingDate" HeaderText = "WeddingDate" />
                <asp:BoundField DataField = "JoinDate" HeaderText = "JoinDate" />
                <asp:BoundField DataField = "HomePhone" HeaderText = "HomePhone" />
                <asp:BoundField DataField = "CellPhone" HeaderText = "CellPhone" />
                <asp:BoundField DataField = "WorkPhone" HeaderText = "WorkPhone" />
                <asp:BoundField DataField = "MemberStatus" HeaderText = "Church" />
                <asp:BoundField DataField = "FellowshipLeader" HeaderText = "Teacher" />
                <asp:BoundField DataField = "Spouse" HeaderText = "Spouse" />
                <asp:BoundField DataField = "Age" HeaderText = "Age" />
                <asp:BoundField DataField = "School" HeaderText = "School" />
                <asp:BoundField DataField = "Grade" HeaderText = "Grade" />
                <asp:BoundField DataField = "AttendPctBF" HeaderText = "AttPct" />
                <asp:BoundField DataField = "Married" HeaderText = "Marital" />
                <asp:BoundField DataField = "FamilyId" HeaderText = "FamilyId" />
                <asp:BoundField DataField = "Image" HeaderText = "ImageUrl" />
            </Columns> 
        </asp:GridView>
    </div>
    </form>
</body>
</html>

