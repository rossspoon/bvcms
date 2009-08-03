<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.RecDetailModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Index</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% CMSWeb.Models.RecDetailModel d = ViewData.Model; %>
    <div>
        <script src="/Content/js/jquery.pagination.js" type="text/javascript"></script>
        <script src="/Content/js/jquery.form.js" type="text/javascript"></script>
        <script src="/Content/js/jquery.form2.js" type="text/javascript"></script>
        <script src="/Scripts/SearchPeople.js" type="text/javascript"></script>
        <script type="text/javascript">
        </script>
    </div>
    <p>
        <a href="/Recreation/">Return to list</a></p>
    <div id="Rec">
        <form method="post" action="/Recreation/Update/<%=d.Id%>">
        <table>
            <tr>
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
                <td>
                    Email: <a href="mailto:<%=d.Email%>"><%=d.Email%></a>
                </td>
                <td colspan="3">
                    <label>
                        ShirtSize:
                        <%=Html.DropDownList("ShirtSize", CMSWeb.Models.RecRegModel.ShirtSizes())%></label>
                    <label>
                        Teammate Request:
                        <%=Html.TextBox("Request", d.Request)%></label>
                    <% if (User.IsInRole("Edit"))
                       { %>
                    <input type="submit" name="Submit" value="Submit" />
                    <% } %>
                </td>
            </tr>
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
    <input name="vid" type="hidden" value='<%=d.Id %>' />
    <% if (User.IsInRole("Edit"))
       { %>
    <input id="delete" type="submit" value="Delete Record" />
    <% } %>
    </form>
</asp:Content>
