$(function () {
    $('body').on("click", '#delete', function (ev) {
        ev.preventDefault();
        var href = $(this).attr("href");
        if (confirm('Are you sure you want to delete?')) {
            $.block("deleting bundle");
            $.post(href, null, function (ret) {
                if (!ret.startsWith("/")) {
                    $.block(ret);
                    $('.blockOverlay').attr('title', 'Click to unblock').click($.unblock);
                }
                else {
                    $.block("Bundle Deleted, click screen to return to bundles");
                    $('.blockOverlay').attr('title', 'Click to unblock').click(function () {
                        $.unblock();
                        window.location = ret;
                    });
                }
            });
        }
        return false;
    });
    $('form table.grid > tbody > tr:even').addClass('alt');
    $("a.bt").button();
    $("input.datepicker").datepicker();

    $("body").on('click', 'a.displayedit', function (ev) {
        ev.preventDefault();
        var f = $(this).closest('form');
        var q = f.serialize();
        $.post($(this).attr('href'), q, function (ret) {
            if (ret.message) {
                $.block(ret.message);
                $('.blockOverlay').attr('title', 'Click to unblock').click(function () {
                    $.unblock();
                    window.location = ret.location;
                });
            }
            else
                $(f).html(ret).ready(function () {
                    $("input.datepicker").datepicker();
                    $(".bt", f).button();
                });
        });
        return false;
    });
});