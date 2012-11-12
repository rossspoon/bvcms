$(function () {
    $("a.bt").button();
    $("a.record").click(function (ev) {
        var t = $(this);
        ev.preventDefault();
        $.post($(this).attr("href"), {}, function (ret) {
            alert(ret);
            t.parent().parent().remove();
        });
        return false;
    });
});
