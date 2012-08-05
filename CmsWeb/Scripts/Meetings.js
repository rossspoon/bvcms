$(function () {
    $(".datepicker").datepicker();
    $("#Dt1").change(function () {
        $("#Dt2").val("");
        $.reload();
    });
    $.reload = function () {
        var q = $("form").serialize();
        window.location = '/Reports/Meetings?' + q;
    }
    $('#Inactive').change($.reload);
    $('#NoZero').change($.reload);
    $('table.grid tbody tr:even').addClass('alt');
    $("a.sortable").click(function (ev) {
        ev.preventDefault();
        var newsort = $(this).text();
        var oldsort = $("#Sort").val();
        $("#Sort").val(newsort);
        var dir = $("#Dir").val();
        if (oldsort == newsort && dir == 'asc')
            $("#Dir").val('desc');
        else
            $("#Dir").val('asc');
        $.reload();
    });
});
