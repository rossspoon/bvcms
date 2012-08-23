$(function () {
    $(".bt").button();

    $("#wandtarget").keypress(function (ev) {
        if (ev.which != 13)
            return true;

        var tb = $("#wandtarget");
        var f = $(this).closest("form");
        var q = f.serialize();
        $.post("/Meeting/ScanTicket/", q, function (ret) {
            if (ret.startsWith("http")) {
                window.location = ret;
            }
            $("#mark").html(ret);
            tb.val("");
        });
        return false;
    });
    $(".atck").change(function (ev) {
        var ck = $(this);
        var tr = ck.parent().parent();
    });
    $("#wandtarget").focus();
});
