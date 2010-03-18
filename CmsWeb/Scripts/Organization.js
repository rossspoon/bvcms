$(function() {
    var maintabs = $("#main-tab").tabs();
    $('#deleteorg').click(function() {
        var href = $(this).attr("href");
        if (confirm('Are you sure you want to delete?')) {
            $.post(href, null, function(ret) {
                if (ret) {
                    $.blockUI({ message: "delete Failed: " + ret });
                    $('.blockOverlay').attr('title', 'Click to unblock').click($.unblockUI);
                }
                else {
                    $.blockUI({ message: "person deleted" });
                    $('.blockOverlay').attr('title', 'Click to unblock').click(function() {
                        $.unblockUI();
                        window.location = "/";
                    });
                }
            });
        }
        return false;
    });

    $(".CreateAndGo").click(function() {
        if (confirm($(this).attr("confirm")))
            $.post($(this).attr("href"), null, function(ret) {
                window.location = ret;
            });
        return false;
    });

    $("#inactive-link").click(function() {
        $.showTable($('#inactive-tab form'));
    });
    $("#pending-link").click(function() {
        $.showTable($('#pending-tab form'));
    });
    $("#priors-link").click(function() {
        $.showTable($('#priors-tab form'));
    });
    $("#visitors-link").click(function() {
        $.showTable($('#visitors-tab form'));
    });
    $("#meetings-link").click(function() {
        $.showTable($('#meetings-tab form'));
    });

    $("a.displayedit").live('click', function() {
        var f = $(this).closest('form');
        $.post($(this).attr('href'), null, function(ret) {
            $(f).html(ret).ready(function() {
                var acopts = {
                    minChars: 3,
                    matchContains: 1
                };
                $(".datepicker").datepicker({
                    dateFormat: 'm/d/yy',
                    changeMonth: true,
                    changeYear: true
                });
                return false;
            });
        });
        return false;
    });
    $("form.DisplayEdit a.submitbutton").live('click', function() {
        var f = $(this).closest('form');
        if (!$(f).valid())
            return false;
        var q = f.serialize();
        $.post($(this).attr('href'), q, function(ret) {
            $(f).html(ret);
        });
        return false;
    });
    $("#future").live('click', function() {
        var f = $(this).closest('form');
        var q = f.serialize();
        $.post($(f).attr("action"), q, function(ret) {
            $(f).html(ret);
        });
    });
    $("form.DisplayEdit").submit(function() {
        if (!$("#submitit").val())
            return false;
    });
    $.validator.addMethod("time", function(value, element) {
        return this.optional(element) || /^\d{1,2}:\d{2}\s(?:AM|PM)/.test(value);
    }, "time format h:mm AM/PM");
    $.validator.setDefaults({
        highlight: function(input) {
            $(input).addClass("ui-state-highlight");
        },
        unhighlight: function(input) {
            $(input).removeClass("ui-state-highlight");
        }
    });
    // validate signup form on keyup and submit
    $("#settingsForm").validate({
        rules: {
            SchedTime: { time: true },
            OnLineCatalogSort: { digits: true },
            Limit: { digits: true },
            NumCheckInLabels: { digits: true },
            NumWorkerCheckInLabels: { digits: true },
            FirstMeetingDate: { date: true },
            LastMeetingDate: { date: true },
            RollSheetVisitorWks: { digits: true },
            GradeAgeStart: { digits: true },
            GradeAgeEnd: { digits: true },
            Fee: { number: true },
            Deposit: { number: true },
            ExtraFee: { number: true },
            ShirtFee: { number: true }
        }
    });
});
function RebindMemberGrids(from) {
    $("#ui-widget-iframe-outer").dialog('close');
}
