$(function () {
    $(".clickSelect").editable("/Meeting/EditGroup/", {
        indicator: '<img src="/images/loading.gif">',
        data: " {'true':'Group (headcount)','false':'Regular'}",
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
    $(".numpresent").editable("enable");
    $(".clickEdit").editable("/Meeting/Edit/", {
        indicator: "<img src='/images/loading.gif'>",
        tooltip: "Click to edit...",
        style: 'display: inline',
        width: '300px',
        height: 25,
        submit: 'OK'
    });

    $(".bt").button();
    $('#visitorDialog').dialog({
        title: 'Add Visitors Dialog',
        bgiframe: true,
        autoOpen: false,
        width: 700,
        height: 600,
        modal: true,
        overlay: {
            opacity: 0.5,
            background: "black"
        }, close: function () {
            $('iframe', this).attr("src", "");
        }
    });
    $('#addvisitor').click(function (e) {
        e.preventDefault();
        var d = $('#visitorDialog');
        $('iframe', d).attr("src", this.href);
        d.dialog("open");
    });
    if($("#showbuttons input[@name=show]:checked").val() == "attends")
        $(".atck:not(:checked)").parent().parent().hide();
    if($('#editing').is(':checked'))
        $(".atck").removeAttr("disabled");

    $("table.grid > tbody > tr:visible:even").addClass("alt");

    $('#showbuttons input:radio').change(function () {
        $("table.grid > tbody > tr").show().removeClass("alt");
        switch ($(this).val()) {
            case "attends":
                $(".atck:not(:checked)").parent().parent().hide();
                break;
            case "absents":
                $(".atck:checked").parent().parent().hide();
                break;
            case "all":
                break;
        }
        $("table.grid > tbody > tr:visible:even").addClass("alt");
    });
    $('#editing').change(function () {
        if ($(this).is(':checked')) {
            $('#showbuttons input:radio[value=all]').click();
            $(".atck").removeAttr("disabled");
        }
        else
            $(".atck").attr("disabled", "disabled");
    });
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
        var cb = $('input[pid=' + text + ']');
        if (cb[0]) {
            cb[0].scrollIntoView();
            cb.click();
        }
        return false;
    });
    $("#wandtarget").focus();
});
function AddSelected(ret) {
    $('#visitorDialog').dialog("close");
    if (ret.error)
        alert(ret.error);
    window.location.reload(true);
}