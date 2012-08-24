$(function () {
    $(".bt").button();

    $("#wandtarget").keypress(function (ev) {
        if (ev.which != 13)
            return true;

        var tb = $("#wandtarget");
        var f = $(this).closest("form");
        var q = f.serialize();
        $.post("/Meeting/ScanTicket/", q, function (ret) {
            $("#mark").html(ret).ready(function () {
                if ($("#haserror").val())
                    PlaySound("zylo");
                else
                    PlaySound("ding");
                if ($("#SwitchMeeting").val() > 0) {
                    $.post("/Meeting/TicketMeeting/" + $("#SwitchMeeting").val(), null, function (ret) {
                        $("#meeting").html(ret).ready(function () {
                            $("#wandtarget").focus();
                        });
                    });
                }
            });
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
function PlaySound(soundObj) {
  var sound = document.getElementById(soundObj);
  sound.Play();
}
