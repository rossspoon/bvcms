<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.VBSDetailModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Index</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% CMSWeb.Models.VBSDetailModel d = ViewData.Model; %>
    <div>
        <script src="/Content/js/jquery.pagination.js" type="text/javascript"></script>
        <script src="/Content/js/jquery.form.js" type="text/javascript"></script>
        <script src="/Content/js/jquery.form2.js" type="text/javascript"></script>
        <script src="/Content/js/ui.draggable.js" type="text/javascript"></script>
        <script src="/Scripts/SearchPeople.js" type="text/javascript"></script>
    </div>

    <script type="text/javascript">
    	$(function() {
    		$('#SearchPeopleDialog').SearchPeopleInit({ overlay: { background: "#000", opacity: 0.3} });
    		$('a.searchpeople').click(function(ev) {
    			$('#SearchPeopleDialog').SearchPeople(ev, function(id, peopleid) {
    				$.post('/VBS/Assign/' + id + "?PeopleId=" + peopleid, null, function(ret) {
    					$('#' + id).text(ret.pid);
    					$("#namelink").replaceWith("<a id='namelink' href='/Person.aspx?id=" + ret.pid + "'>" + ret.name + "</a>");
    				}, "json");
    			});
    			return false;
    		});
    		$("#delete").click(function() {
    			return confirm("Are you sure you want to delete?");
    		});
    	});
    </script>
<p><a href="/VBS/">Return to list</a></p>
<div id="VBS">
<form method="post" action="/VBS/Update/<%=d.Id%>">
<table><tr>
<td><label><a id='<%=d.Id%>' class="searchpeople" href="#">search(<%=d.PeopleId%>)</a></label></td>
<td><label>Name: <a id="namelink" href="/Person.aspx?id=<%=d.PeopleId%>"><%=d.Name%></a></label> </td>
<td>
	<label>Can Pub Photo:<%=Html.CheckBox("PubPhoto", d.PubPhoto)%></label> |
	<label>In Other Church:<%=Html.CheckBox("ActiveInAnotherChurch", d.ActiveInAnotherChurch)%></label> |
	<label>Medical/Allergy:<%=Html.CheckBox("MedAllergy", d.MedAllergy)%></label>
</td>
</tr>
<tr>
<td>&nbsp;</td>
<td><label>Grade: <%=Html.DropDownList("GradeCompleted", d.GradeCompleteds())%></label></td>
<td><label>Request:  <%=Html.TextBox("Request", d.Request)%></label>
<% if (User.IsInRole("Edit"))
   { %>
<input type="submit" name="Submit" value="Submit" /></td>
<% } %>
</td>
</tr>
</table>
</form>
</div>
<div>
    <% if (d.IsDocument)
       { %>
    <iframe width="750" height="440" src='/Image.ashx?id=<%=d.ImgId %>'></iframe>
    <% }
       else
       { %>
    <img alt='VBSAppImage' src='/Image.ashx?id=<%=d.ImgId %>' />
    <% } %>
</div>
<form action="/VBS/Delete" method="post">
<input name="vid" type="hidden" value='<%=d.Id %>' />
<% if (User.IsInRole("Edit"))
   { %>
<input id="delete" type="submit" value="Delete Record" />
<% } %>
</form>
    <div id="SearchPeopleDialog" style="width: 560px; overflow: scroll">
    </div>
</asp:Content>
