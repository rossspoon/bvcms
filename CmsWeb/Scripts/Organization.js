$(function () {
    var maintabs = $("#main-tab").tabs();
    $('#deleteorg').click(function (ev) {
        ev.preventDefault();
        var href = $(this).attr("href");
        if (confirm('Are you sure you want to delete?')) {
            $.post(href, null, function (ret) {
                if (ret) {
                    $.blockUI({ message: ret });
                    $('.blockOverlay').attr('title', 'Click to unblock').click($.unblockUI);
                }
                else {
                    $.blockUI({ message: "org deleted" });
                    $('.blockOverlay').attr('title', 'Click to unblock').click(function () {
                        $.unblockUI();
                        window.location = "/";
                    });
                }
            });
        }
        return false;
    });
    $("#DivisionsList").delegate("#DivisionsList", "change", function () {
        $.getTable($('#Members-tab form'));
        return false;
    });

    $('table.grid > tbody > tr:even').addClass('alt');

    $(".CreateAndGo").click(function (ev) {
        ev.preventDefault();
        if (confirm($(this).attr("confirm")))
            $.post($(this).attr("href"), null, function (ret) {
                window.location = ret;
            });
        return false;
    });
    $('#memberDialog').dialog({
        title: 'Add Members Dialog',
        bgiframe: true,
        autoOpen: false,
        width: 712,
        height: 650,
        modal: true,
        overlay: {
            opacity: 0.5,
            background: "black"
        }, close: function () {
            $('iframe', this).attr("src", "");
        }
    });
    $('a.addmembers').live("click", function (e) {
        e.preventDefault();
        var d = $('#memberDialog');
        $('iframe', d).attr("src", this.href);
        d.dialog("option", "title", "Add Members");
        d.dialog("open");
    });
    $('a.memberdialog').live("click", function (e) {
        e.preventDefault();
        var title;
        var d = $('#memberDialog');
        $('iframe', d).attr("src", this.href);
        d.dialog("option", "title", this.title || 'Edit Member Dialog');
        d.dialog("open");
    });

    $("#inactive-link").click(function () {
        $.showTable($('#Inactive-tab form'));
    });
    $("#pending-link").click(function () {
        $.showTable($('#Pending-tab form'));
    });
    $("#priors-link").click(function () {
        $.showTable($('#Priors-tab form'));
    });
    $("#visitors-link").click(function () {
        $.showTable($('#Visitors-tab form'));
    });
    $("#meetings-link").click(function () {
        $.showTable($('#Meetings-tab form'));
    });
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
    }
    $.initDatePicker = function () {
        $(".datepicker").datepicker({
            dateFormat: 'm/d/yy',
            changeMonth: true,
            changeYear: true,
            beforeShow: function () { $('#ui-datepicker-div').maxZIndex(); }
        });
    }
    $.initDatePicker();
    $("a.displayedit,a.displayedit2").live('click', function (ev) {
        ev.preventDefault();
        var f = $(this).closest('form');
        $.post($(this).attr('href'), null, function (ret) {
            $(f).html(ret).ready(function () {
                var acopts = {
                    minChars: 3,
                    matchContains: 1
                };
                $.initDatePicker();
                $("#DivisionsList").multiSelect();
            });
        });
        return false;
    });
    $("form.DisplayEdit a.submitbutton").live('click', function (ev) {
        ev.preventDefault();
        var f = $(this).closest('form');
        if (!$(f).valid())
            return false;
        var q = f.serialize();
        $.post($(this).attr('href'), q, function (ret) {
            $(f).html(ret);
        });
        return false;
    });
    $("#future").live('click', function () {
        var f = $(this).closest('form');
        var q = f.serialize();
        $.post($(f).attr("action"), q, function (ret) {
            $(f).html(ret);
        });
    });
    $("a.groupmanager").live("click", function (ev) { // this is no longer needed?
        ev.preventDefault();
        var f = $(this).closest('form');
        var q = f.serialize();
        $.post($(this).attr("href"), q, function (ret) {
            if (ret) {
                $(f).html(ret);
                $.post('/Organization/SmallGroups/', null, function (ret) {
                    var op = $('#smallgroupid');
                    var id = $(op).val();
                    $(op).replaceWith(ret);
                    $('#smallgroupid').val(id);
                });
            }
        });
        return false;
    });
    $("form.DisplayEdit").submit(function () {
        if (!$("#submitit").val())
            return false;
    });
    $('a.taguntag').live("click", function (ev) {
        ev.preventDefault;
        $.post('/Organization/ToggleTag/' + $(this).attr('pid'), null, function (ret) {
            $(ev.target).text(ret);
        });
        return false;
    });
    $.validator.addMethod("time", function (value, element) {
        return this.optional(element) || /^\d{1,2}:\d{2}\s(?:AM|am|PM|pm)/.test(value);
    }, "time format h:mm AM/PM");
    $.validator.setDefaults({
        highlight: function (input) {
            $(input).addClass("ui-state-highlight");
        },
        unhighlight: function (input) {
            $(input).removeClass("ui-state-highlight");
        }
    });
    // validate signup form on keyup and submit
    $("#settingsForm").validate({
        rules: {
            "org.SchedTime": { time: true },
            "org.OnLineCatalogSort": { digits: true },
            "org.Limit": { digits: true },
            "org.NumCheckInLabels": { digits: true },
            "org.NumWorkerCheckInLabels": { digits: true },
            "org.FirstMeetingDate": { date: true },
            "org.LastMeetingDate": { date: true },
            "org.RollSheetVisitorWks": { digits: true },
            "org.GradeAgeStart": { digits: true },
            "org.GradeAgeEnd": { digits: true },
            "org.Fee": { number: true },
            "org.Deposit": { number: true },
            "org.ExtraFee": { number: true },
            "org.ShirtFee": { number: true }
        }
    });
    $("#Members-tab").delegate("#smallgroupid", "change", function () {
        $.getTable($('#Members-tab form'));
        return false;
    });
    $("#addfromtaglink").live("click", function (ev) {
        ev.preventDefault();
        var link = this;
        $("#AddFromTag").dialog({
            title: this.title,
            width: "auto",
            buttons: {
                Ok: function () {
                    $.post(link.href, {
                        tagid: $("#addfromtagid").val()
                    }, function () {
                        RebindMemberGrids();
                        $("#AddFromTag").dialog("destroy");
                    });
                }
            }
        });
        return false;
    });

    $("#NewMeetingDialog").dialog({ autoOpen: false });
    $('#RollsheetLink').live("click", function (ev) {
        ev.preventDefault();
        $('#grouplabel').text("By Group");
        var d = $("#NewMeetingDialog");
        d.dialog("option", "buttons", {
            "Ok": function () {
                var dt = $.GetMeetingDateTime();
                if (!dt.valid)
                    return false;
                var args = "?org=curr&dt=" + dt.date + " " + dt.time;
                if ($('#group').is(":checked"))
                    args += "&bygroup=1&sgprefix=";
                window.open("/Reports/Rollsheet/" + args);
                $(this).dialog("close");
            }
        });
        d.dialog('open');
    });
    $('#NewMeeting').live("click", function (ev) {
        ev.preventDefault();
        $('#grouplabel').text("Group Meeting");
        var d = $("#NewMeetingDialog");
        d.dialog("option", "buttons", {
            "Ok": function () {
                var dt = $.GetMeetingDateTime();
                if (!dt.valid)
                    return false;
                var url = "?d=" + dt.date + "&t=" + dt.time +
                "&group=" + ($('#group').is(":checked") ? "true" : "false");
                $.post("/Organization/NewMeeting" + url, null, function (ret) {
                    if (ret)
                        window.location = ret;
                });
                $(this).dialog("close");
            }
        });
        d.dialog('open');
        return false;
    });
    $.GetMeetingDateTime = function () {
        var reTime = /^ *(1[0-2]|[1-9]):[0-5][0-9] *(a|p|A|P)(m|M) *$/;
        var reDate = /^(0?[1-9]|1[012])[\/-](0?[1-9]|[12][0-9]|3[01])[\/-]((19|20)?[0-9]{2})$/i;
        var d = $('#NewMeetingDate').val();
        var t = $('#NewMeetingTime').val();
        var v = true;
        if (!reTime.test(t)) {
            alert('enter valid time');
            v = false;
        }
        if (!reDate.test(d)) {
            alert('enter valid date');
            v = false;
        }
        return { date: d, time: t, valid: v };
    };
    $('.delmeeting').live('click', function (ev) {
        ev.preventDefault();
        if (confirm("delete meeting for sure?")) {
            $.post("/Organization/DeleteMeeting/",
                { id: this.id, future: $('#future').is(":checked") },
                function (ret) {
                    if (ret)
                        $.updateTable($('#Meetings-tab form'));
                });
        }
        return false;
    });
    $('a.joinlink').live('click', function (ev) {
        ev.preventDefault();
        $.post("/Organization/Join/", { id: this.id },
            function (ret) {
                if (ret)
                    RebindMemberGrids();
            });
        return false;
    });
});
function RebindMemberGrids(from) {
    $.updateTable($('#Members-tab form'));
    $.updateTable($('#Inactive-tab form'));
    $.updateTable($('#Pending-tab form'));
    $.updateTable($('#Priors-tab form'));
    $.updateTable($('#Visitors-tab form'));
    $("#memberDialog").dialog("close");
}
