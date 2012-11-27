$(function () {
    $(".bt").button();
    $("#clear").click(function (ev) {
        ev.preventDefault();
        $("input:text").val("");
        $("#Role").val(-1);
        return false;
    });
    $('#Name').focus();
    $("#search").click(function (ev) {
        ev.preventDefault();
        $.getTable();
        return false;
    });
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
        var f = $('#usersearch');
        var q = f.serialize();
        $.blockUI();
        $.post($('#search').attr('href'), q, function(ret) {
            $('#results').replaceWith(ret).ready(function() {
                $('#results > tbody > tr:even').addClass('alt');
                $("#totalcount").text($("#totcnt").val());
                $(".tip").tooltip({ showBody: "|", showURL: false });
                $.unblockUI();
            });
        });
        return false;
    };
    $('#results > tbody > tr:even').addClass('alt');
    $('#Role').multiselect({ selectedList: 4 });
    $(".tip").tooltip({ showBody: "|", showURL: false });
});


