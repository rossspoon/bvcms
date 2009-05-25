<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<SelectListItem>>" %>
<% foreach(var i in Model) 
   { %>
    <option value='<%=i.Value%>'<%=i.Selected? " selected='true'" : "" %>><%=i.Text %></option>
<% } %>