<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site2.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.PrayerModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <title>Prayer Signup</title>
    <link href="/Content/jquery.tooltip.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<script src="/Content/js/jquery.form2.js" type="text/javascript"></script>
<script src="/Content/js/jquery.blockUI.js" type="text/javascript"></script>
<script src="/Content/js/jquery.dimensions.min.js" type="text/javascript"></script>
<script src="/Content/js/jquery.tooltip.min.js" type="text/javascript"></script>
<script type="text/javascript">

    $(function() {
        $('td.slot').tooltip({
            track: true,
            delay: 0,
            showURL: false,
            showBody: " - ",
            fade: 250
        });
    });
    $.blockUI.defaults.growlCSS = {
        width: '350px',
        top: '40%',
        left: '35%',
        right: '10px',
        border: 'none',
        padding: '5px',
        opacity: '0.7',
        cursor: null,
        color: '#fff',
        backgroundColor: '#000',
        '-webkit-border-radius': '10px',
        '-moz-border-radius': '10px'
    };
    $.growlUI2 = function(title, message, timeout) {
        var $m = $('<div class="growlUI2"></div>');
        if (title) $m.append('<h1>' + title + '</h1>');
        if (message) $m.append('<h2>' + message + '</h2>');
        if (timeout == undefined) timeout = 3000;
        $.blockUI({
            message: $m, fadeIn: 400, fadeOut: 700, centerY: false,
            timeout: timeout, showOverlay: false,
            css: $.blockUI.defaults.growlCSS
        });
    };
    $.growlUI1 = function(title, message, timeout) {
        var $m = $('<div class="growlUI"></div>');
        if (title) $m.append('<h1>' + title + '</h1>');
        if (message) $m.append('<h2>' + message + '</h2>');
        if (timeout == undefined) timeout = 3000;
        $.blockUI({
            message: $m, fadeIn: 400, fadeOut: 700, centerY: false,
            timeout: timeout, showOverlay: false,
            css: $.blockUI.defaults.growlCSS
        });
    };
    function ToggleSlot(sender) {
        $.post('/Prayer/ToggleSlot/' + $('#uid').val(), { slot: sender.id, ck: sender.checked }, function(ret) {
            var sid = sender.id;
            $(sender).parent().replaceWith(ret.Html);
            $('#'+sid).parent().tooltip({
                track: true,
                delay: 0,
                showURL: false,
                showBody: " - ",
                fade: 250
            });

            switch (ret.Status) {
                case 'Yours':
                    $.growlUI1('Notification', 'Time slot is now yours');
                    break;
                case 'Open':
                    $.growlUI1('Notification', 'Time slot is now open');
                    break;
                case 'Taken':
                    $.growlUI2('Notification', 'Sorry, time slot has recently been taken by someone else');
                    break;
                case 'Limit':
                    $.growlUI2('Notification', 'Your limit has been reached');
                    break;
                case 'NoChange':
                    $.growlUI2('Notification', 'No change');
                    break;
            }
            return false;
        }, "json");
}
</script>
    <h2>
        Prayer Times <span style="font-size:10pt">(<%=ViewData.Model.TotalCount()%> commitments)</span></h2>
        <%= Html.Hidden("uid", Model.person.PeopleId) %>
   <table align="center" border="0" cellspacing="0" cellpadding="0"><tr><td>
   <table border="0" cellpadding="0" cellspacing="0" id="slots">
        <thead>
        <% if (Model.Group.IsAdmin)
           { %>
            <tr><td colspan="8">
        <a href="/Prayer/Report">Report</a>
            </td></tr>
        <% } %>
            <tr>
                <th></th>
    <% for (var d = 0; d < 7; d++ )
       { %>
                <th><%=Model.DayName(d)%></th>
    <% } %>
            </tr>
        </thead>
        <tbody>
<% foreach(var ts in ViewData.Model.FetchTimeSlots())
   { %>
            <tr>
                <th><%=string.Format("{0:h:mm tt}", ts)%></th>
    <% foreach (var si in ViewData.Model.FetchWeekForTime(ts))
           Response.Write(si.SlotCell() + "\n"); %>
            </tr>
<% } %>
        </tbody>
    </table>
   </td><td valign="top">
   <table id="leg"  cellspacing="0">
  <col></col>
  <col align="left"></col>
   <tr><td colspan="3">Check a box to claim a spot</td></tr>
   <tr><td>&nbsp;</td><td>&nbsp;</td><td>empty</td></tr>
   <tr><td class="m1"></td><td class="o1"></td><td>few</td></tr>
   <tr><td class="m2"></td><td class="o2"></td><td>some</td></tr>
   <tr><td class="m3"></td><td class="o3"></td><td>many</td></tr>
   <tr><td colspan="3">green = yours</td></tr>
   <tr><td colspan="3">unchecked = available</td></tr>
   </table>
   </td></tr></table>
    <div class="growlUI" style="display: none;">
        <h1>Growl Notification</h1>
        <h2>Have a nice day!</h2>
    </div>
</asp:Content>
