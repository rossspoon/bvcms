$(function () {
    $(".submitbutton").button();
    $("a.submitbutton").click(function (ev) {
        ev.preventDefault();
        var f = $(this).closest('form');
        var q = f.serialize();
        $.post(this.href, q, function (ret) {
            if (ret.error) {
                $('#validatecoupon').text(ret.error);
            }
            else {
                window.location = ret.confirm;
            }
        }, "json");
        return false;
    });
    $('form').showPassword();

    $("#Terms").dialog({ autoOpen: false });
    $("#displayterms").click(function () {
        $("#Terms").dialog("open");
    });

    if ($('#IAgree').attr("id")) {
        $("#Submit").attr("disabled", "disabled");
        $("a.submitbutton").attr("disabled", "disabled");
    }
    $("#IAgree").click(function () {
        var checked_status = this.checked;
        if (checked_status == true) {
            $("#Submit").removeAttr("disabled");
            $("a.submitbutton").removeAttr("disabled");
        }
        else {
            $("#Submit").attr("disabled", "disabled");
            $("a.submitbutton").attr("disabled", "disabled");
        }
    });
});

