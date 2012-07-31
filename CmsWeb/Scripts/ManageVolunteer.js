$(function () {
    $.tigerstripe = function () {
        $('table.grid > tbody > tr').removeClass('alt');
        $('table.grid > tbody > tr:even').addClass('alt');
    };
    $.tigerstripe();
    $('#sortweek').click(function (ev) {
        ev.preventDefault();
        $('table.grid > tbody > tr').sortElements(function (a, b) {
            return $(a).find("td.week").attr("jweek") > $(b).find("td.week").attr("jweek") ? 1 : -1;
        });
        $.tigerstripe();
    });
    $('#sortday').click(function (ev) {
        ev.preventDefault();
        $('table.grid > tbody > tr').sortElements(function (a, b) {
            return $(a).find("td.day").attr("jday") > $(b).find("td.day").attr("jday") ? 1 : -1;
        });
        $.tigerstripe();
    });
    if (jQuery().transpose) {
        $(".wrapper .item").transpose();
    }
    $("#SelectAll").click(function () {
        if ($(this).attr("checked"))
            $("input[name='pids']").attr('checked', true);
        else
            $("input[name='pids']").removeAttr('checked');
    });
    $("form").submit(function () {
        $("input[name='Commit']").removeAttr("disabled");
    });
});