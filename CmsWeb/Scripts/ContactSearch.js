$(function () {
    $(".bt").button();
    $(".datepicker").datepicker();
    $.fmtTable = function () {
        $('table.grid > tbody > tr:even').addClass('alt');
    }
    $.fmtTable();
    $("#Search").click(function (ev) {
        ev.preventDefault();
        var f = $(this).closest('form');
        var q = f.serialize();
        $.post("/ContactSearch/Results", q, function (ret) {
            $('table.grid').replaceWith(ret).ready($.fmtTable);
        });
        return false;
    });
});