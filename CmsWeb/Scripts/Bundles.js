$(function () {
    $(".bt").button();
    $.gotoPage = function(e, pg) {
        $("#Page").val(pg);
        $.getTable();
        return false;
    };
    $.setPageSize = function(e) {
        $('#Page').val(1);
        $("#PageSize").val($(e).val());
        return $.getTable();
    };
    $.getTable = function() {
        var f = $('#results');
        var q = f.serialize();
        $.blockUI();
        $.post("/finance/bundles/results", q, function(ret) {
            $('#results').html(ret).ready(function() {
                $('#results > table > tbody > tr:even').addClass('alt');
                $.unblockUI();
            });
        });
        return false;
    };
    $('#results > table > tbody > tr:even').addClass('alt');
});


