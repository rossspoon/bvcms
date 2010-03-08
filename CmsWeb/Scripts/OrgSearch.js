$(function() {
    $('#Name').focus();
    $("#search").click(function() {
        $.getTable();
        return false;
    });
    $.gotoPage = function(e, pg) {
        $("#Page").val(pg);
        return $.getTable();
    }
    $.setPageSize = function(e) {
        $('#Page').val(1);
        $("#PageSize").val($(e).val());
        return $.getTable();
    }
    $.getTable = function() {
        var f = $('#results').closest('form');
        var q = f.serialize();
        $.post($('#search').attr('href'), q, function(ret) {
            $('#results').html(ret).ready(function() {
                $('.addrcol').cluetip({
                    splitTitle: '|',
                    hoverIntent: {
                        sensitivity: 3,
                        interval: 50,
                        timeout: 0
                    }
                });
                $('#results > tbody > tr:even').addClass('alt');
            });
        });
        return false;
    }
    $('#results > thead a.sortable').live('click', function() {
        var newsort = $(this).text();
        var sort = $("#Sort");
        var dir = $("#Direction");
        if ($(sort).val() == newsort && $(dir).val() == 'asc')
            $(dir).val('desc');
        else
            $(dir).val('asc');
        $(sort).val(newsort);
        $.getTable();
        return false;
    });
    $.maxZIndex = $.fn.maxZIndex = function(opt) {
        var def = { inc: 10, group: "*" };
        $.extend(def, opt);
        var zmax = 0;
        $(def.group).each(function() {
            var cur = parseInt($(this).css('z-index'));
            zmax = cur > zmax ? cur : zmax;
        });
        if (!this.jquery)
            return zmax;
        return this.each(function() {
            zmax += def.inc;
            $(this).css("z-index", zmax);
        });
    }
    $(".datepicker").datepicker({
        showOn: 'button',
        buttonImageOnly: true,
        buttonImage: '/Content/images/calendar.gif',
        dateFormat: 'm/d/yy',
        beforeShow: function() { $('#ui-datepicker-div').maxZIndex(); },
        changeMonth: true,
        changeYear: true
    });
    $('#ProgramId').change(function() {
        $.post('/OrgSearch/DivisionIds/' + $('#ProgramId').val(), null, function(ret) {
            $('#DivisionId').replaceWith(ret);
        });
    });
    $("form input").live("keypress", function(e) {
        if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
            $('a.default').click();
            return false;
        }
        return true;
    });
    $('div.dialog').dialog({ autoOpen: false });
    $('#Rollsheet').click(function() {
        $.post('/OrgSearch/DefaultMeetingDate/' + $('#ScheduleId').val(), null, function(ret) {
            $('#MeetingDate').val(ret.date);
            $('#MeetingTime').val(ret.time);
            $('#PanelRollsheet').dialog('open');
        }, "json");
        return false;
    });
    $('#AttDetail').click(function() {
        $('#PanelAttDetail').dialog('open');
        return false;
    });
    $('#Rollsheet').click(function() {
        $('div.dialog').dialog('close');
        var did = $('#DivisionId').val();
        if (did == '0') {
            alert('must choose division');
            return false;
        }
        var sid = $('#ScheduleId').val();
        var nam = $('#Name').val();
        var d = $('#MeetingDate').val();
        var t = $('#MeetingTime').val();
        var args = "?div=" + did +
               "&schedule=" + sid +
               "&name=" + nam +
               "&dt=" + d + " " + t;
        window.open("/Reports/Rollsheet/" + args);
    });
    $('#Meetings').click(function() {
        $('div.dialog').dialog('close');
        var did = $('#DivisionId').val();
        var pid = $('#ProgramId').val();
        var sid = $('#ScheduleId').val();
        var nam = $('#Name').val();
        var cid = $('#CampusId').val();
        var args = "?progid=" + pid +
               "&divid=" + did +
               "&schedid=" + sid +
               "&campusid=" + cid +
               "&name=" + nam;
        window.open("/Meetings.aspx" + args);
    });
    $('#AttDetail').click(function() {
        $('div.dialog').dialog('close');
        var did = $('#DivisionId').val();
        if (did == '0') {
            alert('must choose division');
            return false;
        }
        var sid = $('#ScheduleId').val();
        var nam = $('#Name').val();
        var d1 = $('#MeetingDate1').val();
        var d2 = $('#MeetingDate2').val();
        var args = "?divid=" + did +
               "&schedid=" + sid +
               "&name=" + nam +
               "&dt1=" + d1 +
               "&dt2=" + d2;
        window.open("/Report/AttendanceDetail.aspx" + args);
    });
    $('#Roster').click(function() {
        var did = $('#DivisionId').val();
        if (did == '0') {
            alert('must choose division');
            return false;
        }
        var sid = $('#ScheduleId').val();
        var args = "?div=" + did + "&schedule=" + sid;
        window.open("/Reports/Roster/" + args);
    });
});

function ToggleTagCallback(e) {
    if (e == "")
        return;
    var result = eval('(' + e + ')');
    $('#' + result.ControlId).text(result.HasTag ? "Remove" : "Add");
}

function ViewReport(url) {
    if ('<%=OrgDivisions.SelectedValue %>' == '0')
        alert('must choose division');
    else {
        var args = "?div=<%=OrgDivisions.SelectedValue %>&schedule=<%=Schedule.SelectedValue %>&name=<%=NameSearch.Text %>";
        window.open(url + args);
    }
}
