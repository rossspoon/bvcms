<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.DivisionInfo>" %>
<tr>
    <td class="tip" title="<%=Model.ToolTip %>"><img src="/content/images/group.png" /></td>
    <td><a href="/OrgSearch/Index/?div=<%=Model.Id%>&progid=<%=Model.ProgId%>"><%=Model.Id%></a></td>
    <td>
        <span id='n<%=Model.Id %>' 
            class='clickEdit'><%=Model.Name%></span>
    </td>
    <td><%=Model.ProgId == null ? "(not set)" : Model.Program%></td>
    <td>
        <span id='r<%=Model.Id %>' 
            class='clickEdit'><%=Model.ReportLine%></span>
    </td>
    <td><a href="/Setup/Division/ToggleProg/<%=Model.Id %>" class="taguntag" title="Add to/Remove from Target Program"><%=Model.Tag %></a></td>
    <td><a href="/Setup/Division/MainProg/<%=Model.Id %>" class="mainprog" title="Make Target Main Program"><%=Model.ChangeMain == true ? "Make Main" : "" %></a></td>
    <td>
    <% if (Model.CanDelete)
       { %>
        <a id='x<%=Model.Id %>' href="#" class="delete"><img border="0" src="/images/delete.gif" /></a>
    <% } %>
    </td>
</tr>
