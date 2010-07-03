$(function() {
    $('#Name').focus();
    $("#search").click(function() {
        var name = $('#Name').val();
        if (name.match("^" + "M\.") == "M.") {
            var f = $('#results').closest('form');
            f.attr("action", "/OrgSearch/CreateMeeting/");
            f.submit();
        }
        $.getTable();
        return false;
    });
    $.gotoPage = function(e, pg) {
        $("#Page").val(pg);
        $.getTable();
        return false;
    }
    $.setPageSize = function(e) {
        $('#Page').val(1);
        $("#PageSize").val($(e).val());
        return $.getTable();
    }
    $.getTable = function() {
        var f = $('#results').closest('form');
        var q = f.serialize();
        $.blockUI();
        $.post($('#search').attr('href'), q, function(ret) {
            $('#results').replaceWith(ret).ready(function() {
                $.fmtTable();
                $.unblockUI();
            });
        });
        return false;
    }
    $.editable.addInputType('datepicker', {
        element: function(settings, original) {
            var input = $('<input>');
            if (settings.width != 'none') { input.width(settings.width); }
            if (settings.height != 'none') { input.height(settings.height); }
            input.attr('autocomplete', 'off');
            $(this).append(input);
            return (input);
        },
        plugin: function(settings, original) {
            var form = this;
            settings.onblur = 'ignore';
            $(this).find('input').datepicker().bind('click', function() {
                $(this).datepicker('show');
                return false;
            }).bind('dateSelected', function(e, selectedDate, $td) {
                $(form).submit();
            });
        }
    });
    $.editable.addInputType("checkbox", {
        element: function(settings, original) {
            var input = $('<input type="checkbox">');
            $(this).append(input);
            $(input).click(function() {
                var value = $(input).attr("checked") ? 'yes' : 'no';
                $(input).val(value);
            });
            return (input);
        },
        content: function(string, settings, original) {
            var checked = string == "yes" ? 1 : 0;
            var input = $(':input:first', this);
            $(input).attr("checked", checked);
            var value = $(input).attr("checked") ? 'yes' : 'no';
            $(input).val(value);
        }
    });
    $.fmtTable = function() {
        $("#results td.tip").tooltip({
            showBody: "|"
        });
        $('#results > tbody > tr:even').addClass('alt');
        $("#results.edit span.bday").editable('/OrgSearch/Edit/', {
            type: 'datepicker',
            tooltip: 'click to edit...',
            event: 'click',
            submit: 'OK',
            cancel: 'Cancel',
            width: '100px'
        });
        $('#results.edit span.yesno').editable('/OrgSearch/Edit', {
            type: 'checkbox',
            submit: 'OK'
        });
    }
    $.fmtTable();
    $('#results > thead a.sortable').live('click', function(ev) {
        ev.preventDefault();
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
    $('#clear').click(function(ev) {
        ev.preventDefault();
        var f = $(this).closest('form');
        $(f).find(':input').each(function() {
            $(this).val('');
        });
        $(f).find('select').each(function() {
            $(this).val("0");
        });
        $('#DivisionId').html('<option value="0">(select a program)</option>');
        $('#StatusId').val("30");
        return $.getTable();
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
    $('#ProgramId').change(function() {
        $.post('/OrgSearch/DivisionIds/' + $('#ProgramId').val(), null, function(ret) {
            $('#DivisionId').replaceWith(ret);
        });
    });
    $('#TagProgramId').change(function() {
        $.post('/OrgSearch/TagDivIds/' + $('#TagProgramId').val(), null, function(ret) {
            $('#TagDiv').replaceWith(ret);
            $('#TagDiv').change(function() {
                $.getTable();
            });
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
    $('#rollsheet1').click(function(ev) {
        ev.preventDefault();
        $.post('/OrgSearch/DefaultMeetingDate/' + $('#ScheduleId').val(), null, function(ret) {
            $('#MeetingDate').val(ret.date);
            $('#MeetingTime').val(ret.time);
            var d = $('#PanelRollsheet')
            d.dialog('open');
            d.parent().center();
        }, "json");
        return false;
    });
    $('#attdetail1').click(function(ev) {
        ev.preventDefault();
        var d = $('#PanelAttDetail');
        d.dialog('open');
        d.parent().center();
        return false;
    });
    $('#rollsheet2').click(function(ev) {
        ev.preventDefault();
        $('div.dialog').dialog('close');
        var did = $('#DivisionId').val();
        if (did == '0') {
            alert('must choose division');
            return false;
        }
        var args = "?div=" + did +
               "&schedule=" + $('#ScheduleId').val() +
               "&name=" + $('#Name').val() +
               "&dt=" + $('#MeetingDate').val() + " " + $('#MeetingTime').val();
        window.open("/Reports/Rollsheet/" + args);
        return false;
    });
    $('#ExportExcel').click(function(ev) {
        ev.preventDefault();
        $('div.dialog').dialog('close');
        var args = "?prog=" + $('#ProgramId').val() +
               "&div=" + $('#DivisionId').val() +
               "&schedule=" + $('#ScheduleId').val() +
               "&status=" + $('#StatusId').val() +
               "&campus=" + $('#CampusId').val() +
               "&name=" + $('#Name').val();
        window.open("/OrgSearch/ExportExcel/" + args);
        return false;
    });
    $('#Meetings').click(function(ev) {
        ev.preventDefault();
        $('div.dialog').dialog('close');
        var args = "?progid=" + $('#ProgramId').val() +
               "&divid=" + $('#DivisionId').val() +
               "&schedid=" + $('#ScheduleId').val() +
               "&campusid=" + $('#CampusId').val() +
               "&name=" + $('#Name').val();
        window.open("/Meetings.aspx" + args);
        return false;
    });
    $('#attdetail2').click(function(ev) {
        ev.preventDefault();
        $('div.dialog').dialog('close');
        var did = $('#DivisionId').val();
        if (did == '0') {
            alert('must choose division');
            return false;
        }
        var args = "?divid=" + did +
               "&schedid=" + $('#ScheduleId').val() +
               "&name=" + $('#Name').val() +
               "&dt1=" + $('#MeetingDate1').val() +
               "&dt2=" + $('#MeetingDate2').val();
        window.open("/Report/AttendanceDetail.aspx" + args);
        return false;
    });
    $('#Roster').click(function(ev) {
        ev.preventDefault();
        var did = $('#DivisionId').val();
        if (did == '0') {
            alert('must choose division');
            return false;
        }
        var args = "?div=" + did + "&schedule=" + $('#ScheduleId').val();
        window.open("/Reports/Roster/" + args);
        return false;
    });
    $('#PasteSettings').click(function(ev) {
        ev.preventDefault();
        if (!confirm("Are you sure you want to replace all these settings?"))
            return false;
        var f = $('form');
        var q = f.serialize();
        $.post(this.href, q, function(ret) {
            $.growlUI("Completed", "Settings Replaced");
        });
        return false;
    });
    $('a.ViewReport').click(function(ev) {
        ev.preventDefault();
        var did = $('#DivisionId').val();
        if (did == '0') {
            alert('must choose division');
            return false;
        }
        var args = "?div=" + did +
            "&schedule=" + $('#ScheduleId').val() +
            "&name=" + $('#Name').val();
        window.open($(this).attr("href") + args);
        return false;
    });
    $('a.taguntag').live('click', function(ev) {
        ev.preventDefault();
        var a = $(this);
        $.post(a.attr('href'), { tagdiv: $('#TagDiv').val() }, function(ret) {
            a.text(ret.value);
        }, "json");
        return false;
    });
    $('a.maindiv').live('click', function(ev) {
        ev.preventDefault();
        var a = $(this);
        $.post(a.attr('href'), { tagdiv: $('#TagDiv').val() }, function(ret) {
            a.text("");
        }, "json");
        return false;
    });
});


