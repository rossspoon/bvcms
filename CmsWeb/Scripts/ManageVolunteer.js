$(function () {
    $.tigerstripe = function () {
        $('table.grid > tbody > tr').removeClass('alt');
        $('table.grid > tbody > tr:even').addClass('alt');
    };
    $.tigerstripe();
    $('#sortweek').click(function (ev) {
        ev.preventDefault();
        $('table.grid > tbody > tr').sortElements(function (a, b) {
            return $(a).find("td.week").text() > $(b).find("td.week").text() ? 1 : -1;
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
});