$(document).ready(function () {
    // override the getTable function in pager.js
    $.getTable = function () {
        var f = $("#form");
        var q = f.serialize();
        $.post(f.attr('action'), q, function (ret) {
            $("#Checkins > tbody").html(ret)
                        .ready(function () {
                            $.formatTable();
                            $("span.membercount").text($("#membercount").val());
                            $("span.guestcount").text($("#guestcount").val());
                        });
        });
        return false;
    };
    $('#filter').click(function (ev) {
        ev.preventDefault();
        $.getTable();
    });
    $("#clear").click(function (ev) {
        ev.preventDefault();
        $("input:text").val("");
        $('input:checkbox').removeAttr('checked');
        $("select").val(0);
        $.getTable();
    });
    $.formatTable = function () {
        $('table.grid > tbody > tr:even').addClass('alt');
    };
    $.formatTable();
    $(".bt").button();
});
