$(function () {
    $(".bt").button();
    $(".datepicker").datepicker();
    $.fmtTable = function () {
        $('table.grid > tbody > tr:even').addClass('alt');
    }
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
    }
    $.setPageSize = function (ev) {
        $('#Page').val(1);
        $("#PageSize").val($(ev).val());
        return $.getTable();
    }
    $.getTable = function () {
        var f = $('#results').closest('form');
        var q = f.serialize();
        $.post('/ContactSearch/Results', q, function (ret) {
            $('#results').replaceWith(ret).ready($.fmtTable);
        });
    }
    $("#ConvertToQuery").click(function (ev) {
        ev.preventDefault();
        var f = $('#results').closest('form');
        var q = f.serialize();
        $.post($(this).attr('href'), q, function (ret) {
            window.location = ret;
        });
        return false;
    });
    $("#ContactorSummary").click(function (ev) {
        ev.preventDefault();
        window.location = $(this).attr('href')
            + "?start=" + $("#StartDate").val() 
            + "&end=" + $("#EndDate").val()
            + "&ministry=" + $("#Ministry").val()
        return false;
    });
});