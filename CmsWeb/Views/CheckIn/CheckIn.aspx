<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.CheckInRecModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="/Content/jquery.tooltip.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script src="/Scripts/CheckIn.js" type="text/javascript"></script>

    <%=Html.Hidden("OrgId") %>
    <%=Html.Hidden("PeopleId", Model.person.PeopleId) %>
    <h2>
        CheckIn for
        <%=Model.OrgName %></h2>
    <table>
        <tr>
            <td>
                <a href='/Person.aspx?id=<%=Model.person.PeopleId %>'>
                    <%=Model.person.Name%></a><br />
                <%=Model.WithBreak(Model.person.Addr1) %>
                <%=Model.WithBreak(Model.person.Addr2) %>
                <%=Model.WithBreak(Model.person.CityStateZip) %>
                <%=Model.WithBreak(Model.person.Phone) %>
                <%=Model.WithBreak(Model.person.Cell) %>
                <%=Model.WithBreak(Model.person.Email) %>
                <%=Model.WithBreak(Model.person.School + " " + Model.person.Year) %>
                <%=Model.WithBreak(Model.person.Birthday) %>
            </td>
            <td>
                <img src='<%=Model.person.ImageUrl %>' /><br />
                <a href='tkup:<%=Model.person.PeopleId %>' id="tkup">Take Picture</a>
            </td>
        </tr>
    </table>
    <div>
        KeyCard #:
        <input type="text" id="KeyCard" />
        New Card?
        <input type="checkbox" id="newCard" />
    </div>
    <div id="GOdialog" style="display: none; cursor: default">
        <h1>
            Click GO to refresh page.</h1>
        <input type="button" id="GOrefresh" value="GO" />
    </div>
</asp:Content>
