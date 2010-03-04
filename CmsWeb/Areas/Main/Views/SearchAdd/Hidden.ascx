<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.SearchPersonModel>" %>

<input type="hidden" name="m.List.Index" value="<%=Model.index%>" />
<input type="hidden" name="m.List[<%=Model.index%>].Found" value="<%=Model.Found%>" />
<input type="hidden" name="m.List[<%=Model.index%>].IsNew" value="<%=Model.IsNew%>" />
<input type="hidden" name="m.List[<%=Model.index%>].first" value="<%=Model.first%>" />
<input type="hidden" name="m.List[<%=Model.index%>].last" value="<%=Model.last%>" />
<input type="hidden" name="m.List[<%=Model.index%>].dob" value="<%=Model.birthday.ToShortDateString()%>" />
<input type="hidden" name="m.List[<%=Model.index%>].phone" value="<%=Model.phone%>" />
<input type="hidden" name="m.List[<%=Model.index%>].homecell" value="<%=Model.homecell%>" />
<input type="hidden" name="m.List[<%=Model.index%>].address" value="<%=Model.address%>" />
<input type="hidden" name="m.List[<%=Model.index%>].zip" value="<%=Model.zip%>" />
<input type="hidden" name="m.List[<%=Model.index%>].city" value="<%=Model.city%>" />
<input type="hidden" name="m.List[<%=Model.index%>].state" value="<%=Model.state%>" />
<input type="hidden" name="m.List[<%=Model.index%>].email" value="<%=Model.email%>" />
<input type="hidden" name="m.List[<%=Model.index%>].age" value="<%=Model.age%>" />
