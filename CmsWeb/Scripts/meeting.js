$(function () {
    $('#AddFromTag').dialog({
        title: 'Add From Tag',
        bgiframe: true,
        autoOpen: false,
        width: 750,
        height: 650,
        modal: true,
        overlay: {
            opacity: 0.5,
            background: "black"
        }, close: function () {
            window.location.reload();
        }
    });
    $('a.addfromtag').live("click", function (e) {
        e.preventDefault();
        var d = $('#AddFromTag');
        $('iframe', d).attr("src", this.href);
        d.dialog("option", "title", "Add Attendees From Tag");
        d.dialog("open");
    });
    $(".clickSelectG").editable("/Meeting/EditGroup/", {
        indicator: '<img src="/images/loading.gif">',
        loadurl: "/Meeting/MeetingTypes/",
        loadtype: "POST",
        type: "select",
        submit: "OK",
        style: 'display: inline',
        tooltip: 'Click to edit...',
        callback: function (value, settings) {
            if (value == 'Group (headcount)')
                $(".numpresent").editable("enable");
            else
                $(".numpresent").editable("disable");
        }
    });
    $(".clickSelectC").editable("/Meeting/EditAttendCredit/", {
        indicator: '<img src="/images/loading.gif">',
        loadurl: "/Meeting/AttendCredits/",
        loadtype: "POST",
        type: "select",
        submit: "OK",
        tooltip: 'Click to edit...',
        style: 'display: inline'
    });
    $(".numpresent").editable("enable");
    $(".clickEdit").editable("/Meeting/Edit/", {
        indicator: "<img src='/images/loading.gif'>",
        tooltip: "Click to edit...",
        style: 'display: inline',
        width: '300px',
        height: 25,
        submit: 'OK',
        data: function (value, settings) {
            if (value === '0')
                return '';
          return value;
        },
        callback: function (value) {
            if (value.startsWith("error:"))
                alert(value);
            value = "";
        }
    });

    $(".bt").button();
    $('#visitorDialog').dialog({
        title: 'Add Visitors Dialog',
        bgiframe: true,
        autoOpen: false,
        width: 750,
        height: 700,
        modal: true,
        overlay: {
            opacity: 0.5,
            background: "black"
        }, close: function () {
            $('iframe', this).attr("src", "");
        }
    });
    $('#addvisitor,#addregistered').click(function (e) {
        e.preventDefault();
        if (e.shiftKey) {
            if (confirm("Are you sure you want to join all visitors to org?")) {
                $.post("/Meeting/JoinAllVisitors/" + $("#meetingid").val(), {}, function (ret) {
                    alert(ret);
                });
            }
        }
        else {
            var d = $('#visitorDialog');
            $('iframe', d).attr("src", this.href);
            d.dialog("open");
        }
    });
    $('#JoinAllVisitors').click(function (e) {
        e.preventDefault();
    });
    if ($("#showbuttons input[name=show]:checked").val() == "attends") {
        $(".atck:not(:checked)").parent().parent().hide();
    }
    if ($('#editing').is(':checked')) {
        $(".atck").removeAttr("disabled");
        $(".rgck").removeAttr("disabled");
    }

    $("#attends > tbody > tr:visible:even").addClass("alt");

    $('#showbuttons input:radio').change(function () {
        $("#attends > tbody > tr").hide().removeClass("alt");
        switch ($(this).val()) {
            case "attends":
                $(".atck:checked").parent().parent().show();
                break;
            case "absents":
                $(".atck:not(:checked)").parent().parent().show();
                break;
            case "reg":
                $(".commitment:not(:contains('Uncommitted'))").parent().parent().show();
                $(".atck:checked").parent().parent().show();
                break;
            case "all":
                $("#attends > tbody > tr").show();
                break;
        }
        $("#attends > tbody > tr:visible:even").addClass("alt");
    });
    $('#currmembers').change(function () {
        if ($(this).is(':checked'))
            window.location = "/Meeting/Index/" + $("#meetingid").val() + "?CurrentMembers=true";
        else
            window.location = "/Meeting/Index/" + $("#meetingid").val();
    });
    $('#editing').change(function () {
        if ($(this).is(':checked')) {
            if (!$("#showregistered").val())
                $('#showbuttons input:radio[value=all]').click();
            $(".atck").removeAttr("disabled");
            $(".rgck0").addClass("rgck").removeClass("rgck0");
        }
        else {
            $(".atck").attr("disabled", "disabled");
            $(".rgck").addClass("rgck0").removeClass("rgck");
        }
    });
    $('#sortbyname').click(function () {
        if ($("#sort").val() == "false") {
            $("#sort").val("true");
            $('#attends > tbody > tr').sortElements(function (a, b) {
                return $(a).find("td.name a").text() > $(b).find("td.name a").text() ? 1 : -1;
            });
        }
        else {
            $("#sort").val("false");
            $('#attends > tbody > tr').sortElements(function (a, b) {
                var art = $(a).attr("rowtype");
                var brt = $(b).attr("rowtype");
                if (art > brt)
                    return -1;
                else if (art < brt)
                    return 1;
                return $(a).find("td.name a").text() > $(b).find("td.name a").text() ? 1 : -1;
            });
        }
    });
    $('#registering').change(function () {
        if ($(this).is(':checked')) {
            $(".showreg").show();
            $("#addregistered").removeClass("hidden");
        }
        else {
            $(".showreg").hide();
            $("#addregistered").addClass("hidden");
        }
    });
    $(".showreg").hide();
    if ($("#showregistered").val()) {
        $('#showbuttons input:radio[value=reg]').click();
        $('#registering').click();
    }

    $(".atck").change(function (ev) {
        var ck = $(this);
        var tr = ck.parent().parent();
        $.post("/Meeting/MarkAttendance/", {
            MeetingId: $("#meetingid").val(),
            PeopleId: ck.attr("pid"),
            Present: ck.is(':checked')
        }, function (ret) {
            if (ret.error) {
                ck.attr("checked", !ck.is(':checked'));
                alert(ret.error);
            }
            else {
                tr.effect("highlight", {}, 3000);
                for (var i in ret) {
                    $("#" + i + " span").text(ret[i]);
                }
            }
        });
    });
    $("#wandtarget").keypress(function (ev) {
        if (ev.which != 13)
            return true;
        if (!$("#editing").is(':checked'))
            $("#editing").click();

        var tb = $("#wandtarget");
        var text = tb.val();
        tb.val("");
        if (text.substring(2, 0) == "M.") {
            $.post("/Meeting/CreateMeeting/", { id: text }, function (ret) {
                if (ret.substring(5, 0) == "error")
                    alert(ret);
                else
                    window.location = ret;
            });
            return false;
        }
        var cb = $('input[pid=' + text + '].atck');
        if (cb[0]) {
            cb[0].scrollIntoView();
            cb.click();
        }
        return false;
    });
    $("#wandtarget").focus();

    $.extraEditable = function () {
        $('.editarea').editable('/Meeting/EditExtra/', {
            type: 'textarea',
            submit: 'OK',
            rows: 5,
            width: 200,
            indicator: '<img src="/images/loading.gif">',
            tooltip: 'Click to edit...'
        });
        $(".editline").editable("/Meeting/EditExtra/", {
            indicator: "<img src='/images/loading.gif'>",
            tooltip: "Click to edit...",
            style: 'display: inline',
            width: 200,
            height: 25,
            submit: 'OK'
        });
    }
    $("#newvalueform").dialog({
        autoOpen: false,
        buttons: {
            "Ok": function () {
                var ck = $("#multiline").is(':checked');
                var fn = $("#fieldname").val();
                var v = $("#fieldvalue").val();
                if (fn)
                    $.post("/Meeting/NewExtraValue/" + $("#meetingid").val(), { field: fn, value: v, multiline: ck }, function (ret) {
                        if (ret.startsWith("error"))
                            alert(ret);
                        else {
                            $("#extras > tbody").html(ret);
                            $.extraEditable();
                        }
                        $("#fieldname").val("");
                    });
                $(this).dialog("close");
            }
        }
    });
    $("#newextravalue").live("click", function (ev) {
        ev.preventDefault();
        var d = $('#newvalueform');
        d.dialog("open");
    });
    $("a.deleteextra").live("click", function (ev) {
        ev.preventDefault();
        if (confirm("are you sure?"))
            $.post("/Meeting/DeleteExtra/" + $("#meetingid").val(), { field: $(this).attr("field") }, function (ret) {
                if (ret.startsWith("error"))
                    alert(ret);
                else {
                    $("#extras > tbody").html(ret);
                    $.extraEditable();
                }
            });
        return false;
    });
    $('#attends').bind('mousedown', function (e) {
        if ($(e.target).hasClass("rgck")) {
            $(e.target).editable("/Meeting/EditCommitment/", {
                indicator: '<img src="/images/loading.gif">',
                loadtype: 'post',
                loadurl: "/Meeting/AttendCommitments/",
                type: "select",
                submit: "OK",
                style: 'display: inline'
            });
        }
    });
    $("#rollsheetoptions").dialog({
        autoOpen: false,
        buttons: {
            "Ok": function () {
                var ck = $("#bygroup").is(':checked');
                var sg = $("#sgprefix").val();
                var q = "?MeetingId=" + $("#meetingid").val();
                if (ck)
                    q = q.appendQuery("bygroup=1");
                if (sg.length > 0)
                    q = q.appendQuery("sgprefix=" + sg);
                $(this).dialog("close");
                window.open("/Reports/Rollsheet" + q);
            }
        }
    });
    $("#rollsheet").live("click", function (ev) {
        ev.preventDefault();
        var d = $('#rollsheetoptions');
        d.dialog("open");
    });
});
function AddSelected(ret) {
    $('#visitorDialog').dialog("close");
    if (ret.error)
        alert(ret.error);
    window.location.reload(true);
}