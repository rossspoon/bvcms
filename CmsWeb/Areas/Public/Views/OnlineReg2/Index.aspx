<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/onlinereg.Master" Inherits="System.Web.Mvc.ViewPage<CmsWeb.Models.OnlineRegModel2>" %>
<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
<style>
.instruct
{
    position:relative;
    left: -1em;
    color:Blue;
    font-size: 120%;
    padding-left: 2em;
    height: 27px;
    line-height: 27px;
    background: url(/content/bluebullet.gif) left center no-repeat;
}
.box
{
    margin-left:1em; 
    border-width: 1px;
    border-color: #D3D3D3; 
    border-style: solid;
    padding-left:1em;
}
a.close
{
    float:right;
}
div.personheader
{
    font-size:120%;
    padding:8px;
}
</style>
    <%= SquishIt.Framework.Bundle.JavaScript()
        .Add("/Content/js/jquery-1.4.2.js")
        .Add("/Content/js/jquery-ui-1.8.2.custom.js")
        .Add("/Content/js/jquery.idle-timer.js")
        .Add("/Scripts/OnlineReg.js")
        .Render("/Content/OnLineReg_#.js")
            %>  
    <script type="text/javascript">
        $(function () {
            $(document).bind("idle.idleTimer", function () {
                window.location.href = '<%=Model.URL %>';
            });
            var tmout = parseInt('<%=ViewData["timeout"] %>');

            if ($('input:text[value!=""]').length == 0)
                $(document).bind("keydown", function () {
                    $(document).unbind("keydown");
                    $.idleTimer(tmout);
                });
            else
                $.idleTimer(tmout);
            $(".personheader a").live("click", function (e) {
                e.preventDefault();
                $(this).closest('div').nextAll('table').slideToggle();
                return false;
            });
        });
    </script>
    <h2><%=Model.Header%></h2>
<% if (Model.IsEnded())
   { %>
<h4 style="color:Red">Sorry, registration has ended</h4>
<% }
   else
   { %>
<form class="DisplayEdit" action="/OnlineReg2/CompleteRegistration/<%=Model.qtesting %>" method="post">
<% Html.RenderPartial("List", Model); %>
</form>
<% } %>
</asp:Content>