$(function () {
    $(".bt").button();
    $(".datepicker").datepicker();
    $.fmtTable = function () {
        $('table.grid > tbody > tr:even').addClass('alt');
    }
    $.fmtTable();
    $("#Search").click(function (ev) {
        ev.preventDefault();
        $.getTable();
        return false;
    });
    $.gotoPage = function (ev, pg) {
        $("#Page").val(pg);
        $.getTable();
        return false;
    }
    $.setPageSize = function (ev) {
        $('#Page').val(1);
        $("#PageSize").val($(ev).val());
        return $.getTable();
    }
    $.getTable = function () {
        var f = $('#form');
        var q = f.serialize();
        $.post('/ContactSearch/Results', q, function (ret) {
            $('table.grid').replaceWith(ret).ready($.fmtTable);
        });
    }
});