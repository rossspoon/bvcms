$(function () {
    $(".bt").button();

    $("#wandtarget").keypress(function (ev) {
        if (ev.which != 13)
            return true;
        $.mark();
        return false;
    });
    $.mark = function() {
        var tb = $("#wandtarget");
        var q = $("#markform").serialize();
        $.post("/Meeting/ScanTicket/", q, function(ret) {
            $("#mark").html(ret).ready(function() {
                if ($("#haserror").val())
                    PlaySound("zylo");
                else
                    PlaySound("ding");
                if ($("#SwitchMeeting").val() > 0) {
                    $.post("/Meeting/TicketMeeting/" + $("#SwitchMeeting").val(), null, function(ret) {
                        $("#meeting").html(ret).ready(function() {
                            $("#wandtarget").focus();
                        });
                    });
                }
            });
            tb.val("");
        });
    };
    $("#name").autocomplete({
        autoFocus: true,
        minLength: 3,
        source: function (request, response) {
            $.post("/Meeting/Names", request, function (ret) {
                if (!ret.length) {
                    $.growlUI("Name", "Not Found");
                    $('#name').val('');
                }
                response(ret.slice(0, 10));
            }, "json");
        },
        select: function (event, ui) {
            $("#wandtarget").val(ui.item.Pid);
            $.mark();
            $("#name").val('');
            return false;
        }
    }).data("uiAutocomplete")._renderItem = function (ul, item) {
        return $("<li>")
            .append("<a>" + item.Name + "<br>" + item.Addr + "</a>")
            .appendTo(ul);
    };
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
