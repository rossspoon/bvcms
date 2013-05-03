$(function () {
    $('#init').click(function (ev) {
        ev.preventDefault();
        var f = $(this).closest("form");
        var q = f.serialize();
        $.post(f.attr("action"), q, function (ret) {
            $('#imessageOK').text(ret);
            $('#imessageOK').show();
            $("button.ibutton[name='test']").removeAttr('disabled');
        });
        return false;
    });
    $("button.ibutton[name='test']").attr('disabled', 'disabled');
    $("button.ibutton[name='test']").click(function (ev) {
        ev.preventDefault();
        var f = $(this).closest("form");
        var q = f.serialize();
        $.post(f.attr("action"), q, function (ret) {
            $("div.xml pre", f).html(ret);
        });
        return false;
    });
    $("button.ibutton[name='clear']").click(function (ev) {
        ev.preventDefault();
        var f = $(this).closest("form");
        $("div.xml pre", f).html("");
        return false;
    });
    $("div.section").click(function (ev) {
        $(this).next().toggle();
    });
    $("div.section").tooltip();
});
