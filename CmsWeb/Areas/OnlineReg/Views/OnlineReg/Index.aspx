<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/onlinereg.Master"
    Inherits="System.Web.Mvc.ViewPage<CmsWeb.Models.OnlineRegModel>" %>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .instruct
        {
            color: #808080;
            font-size: 120%;
            height: 27px;
            line-height: 27px; /*  padding-left: 2em; 
    position:relative;
    left: -1em;
    background: url(/content/bluebullet.gif) left center no-repeat;
*/
        }
        h4
        {
            color: #908080;
            font-size: 90%;
            padding-left: 11px; /*  padding-left: 2em; 
    position:relative;
    left: -1em;
    background: url(/content/bluebullet.gif) left center no-repeat;
*/
        }
        .box
        {
            border-width: 1px;
            border-color: #808080;
            border-style: solid;
            padding-left: 10px;
            padding-top: 8px;
            padding-bottom: 8px;
        }
        a.close
        {
            float: right;
        }
        div.personheader
        {
            font-size: 110%;
        }
        div.instructions.find, div.instructions.options
        {
            display: none;
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
    <h2>
        <%=Model.Header%></h2>
    <% if (Model.IsEnded())
       { %>
    <h4 style="color: Red">
        Sorry, registration has ended</h4>
    <% }
       else
       { %>
    <%=Util.PickFirst(Model.Instructions, @"
<div class=""instructions login"">
</div>
<div class=""instructions find"">
</div>
<div class=""instructions select"">
</div>
<div class=""instructions other"">
</div>
<div class=""instructions submit"">
</div>
") %>
    <form class="DisplayEdit" action="/OnlineReg/CompleteRegistration/<%=Model.qtesting %>"
    method="post">
    <% Html.RenderPartial("List", Model); %>
    </form>
    <% } %>
</asp:Content>
