$(function () {
    $("a.bt").button();
    $('#sendreminders').click(function (ev) {
        ev.preventDefault();
        var href = $(this).attr("href");
        if (confirm('Are you sure you want to send reminders?')) {
            $.blockUI({ message: "sending reminders" });
            $.post(href, null, function (ret) {
                if (ret != "ok") {
                    $.unblockUI();
                    $.growlUI("error", ret);
                }
                else {
                    $.unblockUI();
                    $.growlUI("Email", "Reminders Sent");
                }
            });
        }
    });
    $(".smallgroup").change(function () {
        window.location = $.URL();
    });
    $("#SortIt").click(function (ev) {
        ev.preventDefault();
        if ($("#SortByWeek").val() === "False")
            $("#SortByWeek").val("True");
        else
            $("#SortByWeek").val("False");
        window.location = $.URL();
    });
    $.URL = function () {
        var u = "/Volunteers/Calendar/" + $("#OrgId").val()
                + "?sg1=" + $("#sg1").val() + "&sg2=" + $("#sg2").val();
        if ($("#SortByWeek").val() === "True")
            return u + "&sortbyweek=true";
        return u;
    }
    $("a[pid]").draggable({
        appendTo: "body",
        helper: "clone"
    });
    $("div.drop").droppable({
        activeClass: "ui-state-default",
        hoverClass: "ui-state-hover",
        accept: ":not(.ui-sortable-helper)",
        drop: function (event, ui) {
            var $this = $(this);
            $.post("/Volunteers/DragDrop/", {
                "id": $("#OrgId").val(),
                "week": $this.attr("week"),
                "time": $this.attr("time"),
                "pid": ui.draggable.attr("pid")
            }, function (ret) {
                window.location = "/Volunteers/Calendar/" + $("#OrgId").val()
                        + "?sg1=" + $("#sg1").val() + "&sg2=" + $("#sg2").val();
            });
        }
    });
});
