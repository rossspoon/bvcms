<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.RecDetailModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Index</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% CMSWeb.Models.RecDetailModel d = Model; %>
    <div>
        <script src="/Content/js/jquery.pagination.js" type="text/javascript"></script>
        <script src="/Content/js/jquery.form.js" type="text/javascript"></script>
        <script src="/Content/js/jquery.form2.js" type="text/javascript"></script>
        <script src="/Scripts/SearchPeople.js" type="text/javascript"></script>
        <script type="text/javascript">
            $(function() {
                $('#SearchPeopleDialog').SearchPeopleInit({ overlay: { background: "#000", opacity: 0.3} });
                $('a.searchpeople').click(function(ev) {
                    $('#SearchPeopleDialog').SearchPeople(ev, function(id, peopleid) {
                        $.post('/Recreation/Assign/' + id + "?PeopleId=" + peopleid, null, function(ret) {
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
    </div>
    <p>
        <a href="/Recreation/">Return to list</a></p>
    <div id="Rec">
        <form method="post" action="/Recreation/Update/<%=d.Id%>">
        <table>
            <tr>
                <td><label><a id='<%=d.Id%>' class="searchpeople" href="#">search(<%=d.PeopleId%>)</a></label></td>
                <td>
                    <label>
                        Name: <a id="namelink" href="/Person.aspx?id=<%=d.PeopleId%>">
                            <%=d.Name%></a></label>
                </td>
                <td>
                    <label>
                        FeePaid:
                        <%=Html.CheckBox("FeePaid", d.FeePaid)%></label>
                        (<%=d.TransactionID %>)
                </td>
                <td>
                    <label>
                        In Other Church:<%=Html.CheckBox("ActiveInAnotherChurch", d.ActiveInAnotherChurch)%></label>
                </td>
                <td>
                    <label>
                        Medical/Allergy:<%=Html.CheckBox("MedAllergy", d.MedAllergy)%></label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    Email: <a href="mailto:<%=d.Email%>"><%=d.Email%></a>
                </td>
                <td colspan="3">
                    <label>
                        ShirtSize:
                        <%=Html.DropDownList("ShirtSize", CMSWeb.Models.RecRegModel.ShirtSizes())%></label>
                    <label>
                        Teammate Request:
                        <%=Html.TextBox("Request", d.Request)%></label>
                    <label>League: <%=Html.DropDownList("League", Model.Leagues()) %></label>
                </td>
            </tr>
            <tr><td colspan="4"></td><td>
                    <% if (User.IsInRole("Edit"))
                       { %>
                    <input type="submit" name="Submit" value="Submit" />
                    <% } %>
            </td></tr>
        </table>
        </form>
    </div>
    <div>
        <% if (d.IsDocument)
           { %>
           <%=ImageData.Image.Content(d.ImgId) %>
        <% }
           else
           { %>
        <img alt='RecImage' src='/Image.ashx?id=<%=d.ImgId %>' />
        <% } %>
    </div>
    <form action="/Recreation/Delete" method="post">
    <input name="rid" type="hidden" value='<%=d.Id %>' />
    <% if (User.IsInRole("Edit"))
       { %>
    <input id="delete" type="submit" value="Delete Record" />
    <% } %>
    </form>
    <div id="SearchPeopleDialog" style="width: 560px; overflow: scroll">
    </div>
</asp:Content>
