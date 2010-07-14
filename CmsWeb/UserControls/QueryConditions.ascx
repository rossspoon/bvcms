<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="QueryConditions.ascx.cs"
    Inherits="CmsWeb.QueryConditions" %>

<script type="text/javascript">


function toggle(t)
{
    //$("div.moreinfo").hide();
    var e = $(t).parent().next();
    if ($(e).is(':hidden'))
        $(e).show();
    else
        $(e).hide();
}
function show(t)
{
    $(t).parent().next().show();
}
</script>

<div id="conditions">
    <div>
        <asp:LinkButton ID="lbGroup" runat="server" OnClick="lbGroup_Click">Or start a new group of conditions</asp:LinkButton><br />
    </div>
    <div id="tabber" style="overflow: auto; height: 450px; margin:4px; border-top:solid 2px gray">
        <ul>
            <asp:Repeater ID="ConditionTabs" runat="server">
                <ItemTemplate>
                    <li><a href="<%# Eval("Name", "#{0}") %>"><span>
                        <%# Eval("Title") %></span></a></li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                <div id='<%# Eval("Name") %>'>
                    <asp:Repeater ID="Repeater2" runat="server" DataSource='<%# Eval("Fields") %>' OnItemCommand="ItemCommand_Click">
                        <ItemTemplate>
                            <div>
                                <asp:LinkButton CssClass="FieldLink" ID="LinkButton1" runat="server" Text='<%# Eval("Title") %>'
                                    CommandArgument='<%# Eval("Name") %>'></asp:LinkButton>
                                <span style="cursor: pointer" onclick="toggle(this)">
                                    <asp:Image ImageUrl="~/images/help_out.gif" runat="server" /></span>
                            </div>
                            <div class="moreinfo">
                                <%# Eval("Description") %>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</div>
