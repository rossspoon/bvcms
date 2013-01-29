$(function () {
    $(".bt").button();
    $(".datepicker").datepicker();
    $.fmtTable = function () {
        $('table.grid > tbody > tr:even').addClass('alt');
    };
    $.fmtTable();
    $("#search").click(function (ev) {
        ev.preventDefault();
        $.getTable();
        return false;
    });
    $.gotoPage = function (ev, pg) {
        $("#Page").val(pg);
        $.getTable();
        return false;
    };
    $.setPageSize = function (ev) {
        $('#Page').val(1);
        $("#PageSize").val($(ev).val());
        return $.getTable();
    };
    $.getTable = function () {
        var f = $('#results').closest('form');
        var q = f.serialize();
        $.blockUI();
        $.post('/Finance/Contributions/Results', q, function (ret) {
            $('#results').replaceWith(ret).ready($.fmtTable);
            $.unblockUI();
        });
    };
    $("#NewSearch").click(function () {
        form.reset();
    });
    $('.tip').tooltip({ showBody: "|" });
});