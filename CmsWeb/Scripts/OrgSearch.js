$(function () {
    $('#Name').focus();
    $(".bt").button();
    $("#clear").click(function (ev) {
        ev.preventDefault();
        $("input:text").val("");
        $("#ProgramId,#CampusId,#ScheduleId,#TypeId").val(0);
        $("#OnlineReg").val(-1);
        $.post('/OrgSearch/DivisionIds/0', null, function (ret) {
            $('#DivisionId').html(ret);
        });
        return false;
    });
    $("#search").click(function (ev) {
        ev.preventDefault();
        var name = $('#Name').val();
        if (name.match("^" + "M\.") == "M.") {
            $('#Name').val("");
            var f = $('#results').closest('form');
            f.attr("action", "/OrgSearch/CreateMeeting/" + name);
            f.submit();
        }
        $.getTable();
        return false;
    });
    $("#hideshow").click(function () {
        $(".managedivisions").toggle();
    });
    $.getTable = function () {
        var f = $('#results').closest('form');
        var q = f.serialize();
        $.blockUI();
        $.post($('#search').attr('href'), q, function (ret) {
            $('#results').replaceWith(ret).ready(function () {
                $.fmtTable();
                $.unblockUI();
            });
        });
        return false;
    };
    $.editable.addInputType('datepicker', {
        element: function (settings, original) {
            var input = $('<input>');
            if (settings.width != 'none') { input.width(settings.width); }
            if (settings.height != 'none') { input.height(settings.height); }
            input.attr('autocomplete', 'off');
            $(this).append(input);
            return (input);
        },
        plugin: function (settings, original) {
            var form = this;
            settings.onblur = 'ignore';
            $(this).find('input').datepicker().bind('click', function () {
                $(this).datepicker('show');
                return false;
            }).bind('dateSelected', function (e, selectedDate, $td) {
                $(form).submit();
            });
        }
    });
    $.editable.addInputType("checkbox", {
        element: function (settings, original) {
            var input = $('<input type="checkbox">');
            $(this).append(input);
            $(input).click(function () {
                var value = $(input).attr("checked") ? 'yes' : 'no';
                $(input).val(value);
            });
            return (input);
        },
        content: function (string, settings, original) {
            var checked = string == "yes" ? 1 : 0;
            var input = $(':input:first', this);
            $(input).attr("checked", checked);
            var value = $(input).attr("checked") ? 'yes' : 'no';
            $(input).val(value);
        }
    });
    $.fmtTable = function () {
        $("#results td.tip[title]").tooltip({
            showBody: "|"
        });
        $('#results > tbody > tr:even').addClass('alt');

        $('#results').bind('mousedown', function (e) {
            if ($(e.target).hasClass("bday")) {
                $(e.target).editable('/OrgSearch/Edit/', {
                    type: 'datepicker',
                    tooltip: 'click to edit...',
                    event: 'click',
                    submit: 'OK',
                    cancel: 'Cancel',
                    width: '100px',
                    height: 25
                });
            } else if ($(e.target).hasClass("yesno")) {
                $(e.target).editable('/OrgSearch/Edit', {
                    type: 'checkbox',
                    onblur: 'ignore',
                    submit: 'OK'
                });
            }
        });
    };
    $.fmtTable();
    $.maxZIndex = $.fn.maxZIndex = function (opt) {
        var def = { inc: 10, group: "*" };
        $.extend(def, opt);
        var zmax = 0;
        $(def.group).each(function () {
            var cur = parseInt($(this).css('z-index'));
            zmax = cur > zmax ? cur : zmax;
        });
        if (!this.jquery)
            return zmax;
        return this.each(function () {
            zmax += def.inc;
            $(this).css("z-index", zmax);
        });
    };
    $("#searchvalues select").css("width", "100%");
    $('#ProgramId').change(function () {
        $.post('/OrgSearch/DivisionIds/' + $(this).val(), null, function (ret) {
            $('#DivisionId').html(ret);
        });
    });

    $("#ApplyType").click(function (ev) {
        ev.preventDefault();
        var f = $('#results').closest('form');
        var q = f.serialize();
        $.blockUI();
        $.post('/OrgSearch/ApplyType/' + $("#TargetType").val(), q, function (ret) {
            $.getTable();
        });
        return false;
    });
    $("#MakeNewDiv").click(function (ev) {
        ev.preventDefault();
        $.post('/OrgSearch/MakeNewDiv/', { id: $("#TagProgramId").val(), name: $("#NewDiv").val() }, function (ret) {
            $('#TagDiv').html(ret);
            $("#NewDiv").val("");
        });
        return false;
    });
    $("#RenameDiv").click(function (ev) {
        ev.preventDefault();
        $.post('/OrgSearch/RenameDiv/', { id: $("#TagProgramId").val(), divid: $("#TagDiv").val(), name: $("#NewDiv").val() }, function (ret) {
            if (ret == "error")
                alert("expected error");
            {
                $('#TagDiv').html(ret);
                $("#NewDiv").val("");
            }
        });
        return false;
    });
    $("#TagProgramId").change(function () {
        $.post('/OrgSearch/TagDivIds/' + $(this).val(), null, function (ret) {
            $('#TagDiv').html(ret);
            $("#search").click();
        });
    });
    $("#TagDiv").change(function () {
        $("#search").click();
    });

    $("form input").live("keypress", function (e) {
        if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
            $('a.default').click();
            return false;
        }
        return true;
    });
    $('div.dialog').dialog({ autoOpen: false });
    $('#rollsheet1').click(function (ev) {
        ev.preventDefault();
        $.post('/OrgSearch/DefaultMeetingDate/' + $('#ScheduleId').val(), null, function (ret) {
            $('#MeetingDate').val(ret.date);
            $('#MeetingTime').val(ret.time);
            var d = $('#PanelRollsheet')
            d.dialog('open');
        });
        return false;
    });
    $('#attdetail1').click(function (ev) {
        ev.preventDefault();
        var d = $('#PanelAttDetail');
        d.dialog('open');
        return false;
    });
    $('#rollsheet2').click(function (ev) {
        ev.preventDefault();
        $('div.dialog').dialog('close');
        var pid = $('#ProgramId').val();
        var did = $('#DivisionId').val();
        if (pid == '0') {
            $.growlUI("error", 'must choose program');
            return false;
        }
        var args = "?div=" + did + "&pid=" + pid +
               "&schedule=" + $('#ScheduleId').val() +
               "&name=" + $('#Name').val() +
               "&dt=" + $('#MeetingDate').val() + " " + $('#MeetingTime').val();
        if ($('#altnames').is(":checked"))
            args += "&altnames=true";
        if ($('#rallymode').is(":checked"))
            window.open("/Reports/RallyRollsheet/" + args);
        else
            window.open("/Reports/Rollsheet/" + args);
        return false;
    });
    $('#ExportExcel').click(function (ev) {
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
    $('#Meetings').click(function (ev) {
        ev.preventDefault();
        $('div.dialog').dialog('close');
        var args = "?progid=" + $('#ProgramId').val() +
               "&divid=" + $('#DivisionId').val() +
               "&schedid=" + $('#ScheduleId').val() +
               "&campusid=" + $('#CampusId').val() +
               "&statusid=" + $('#StatusId').val() +
               "&name=" + $('#Name').val();
        window.open("/Reports/Meetings" + args);
        return false;
    });
    $('#attdetail2').click(function (ev) {
        ev.preventDefault();
        $('div.dialog').dialog('close');
        var did = $('#DivisionId').val();
        if (did == '0') {
            $.growlUI("error", 'must choose division');
            return false;
        }
        var args = "?divid=" + did +
               "&schedid=" + $('#ScheduleId').val() +
               "&name=" + $('#Name').val() +
               "&dt1=" + $('#MeetingDate1').val() +
               "&dt2=" + $('#MeetingDate2').val();
        window.open("/Reports/AttendanceDetail" + args);
        return false;
    });
    $('#Roster').click(function (ev) {
        ev.preventDefault();
        var did = $('#DivisionId').val();
        if (did == '0') {
            $.growlUI("error", 'must choose division');
            return false;
        }
        var args = "?div=" + did + "&schedule=" + $('#ScheduleId').val();
        window.open("/Reports/Roster/" + args);
        return false;
    });
    $('#PasteSettings').click(function (ev) {
        ev.preventDefault();
        if (!confirm("Are you sure you want to replace all these settings?"))
            return false;
        var f = $('form');
        var q = f.serialize();
        $.post(this.href, q, function (ret) {
            $.growlUI("Completed", "Settings Replaced");
        });
        return false;
    });
    $('#RepairTransactions').click(function (ev) {
        ev.preventDefault();
        if (!confirm("Are you sure you want to run repair transactions?"))
            return false;
        var f = $('form');
        var q = f.serialize();
        $.post(this.href, q, function (ret) {
            $.growlUI("Completed", "Repair Completed");
        });
        return false;
    });
    $('a.ViewReport').click(function (ev) {
        ev.preventDefault();
        var did = $('#DivisionId').val();
        if (did == '0') {
            $.growlUI("error", 'must choose division');
            return false;
        }
        var args = "?div=" + did +
            "&schedule=" + $('#ScheduleId').val() +
            "&name=" + $('#Name').val();
        window.open($(this).attr("href") + args);
        return false;
    });
    $('a.taguntag').live('click', function (ev) {
        ev.preventDefault();
        var a = $(this);
        var td = $('#TagDiv').val()
        if (td > 0)
            $.post(a.attr('href'), { tagdiv: td }, function (ret) {
                if (ret == "error")
                    alert("unexpected error, refresh page");
                else {
                    $(a).parent().parent().replaceWith(ret);
                    $.fmtTable();
                }
            });
        return false;
    });
    $('a.maindiv').live('click', function (ev) {
        ev.preventDefault();
        var a = $(this);
        $.post(a.attr('href'), { tagdiv: $('#TagDiv').val() }, function (ret) {
            if (ret == "error")
                alert("unexpected error, refresh page");
            else {
                $(a).parent().parent().replaceWith(ret);
                $.fmtTable();
            }
        });
        return false;
    });
});


