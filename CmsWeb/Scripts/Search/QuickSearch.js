$(function () {
    $(".bt").button();
    $(".grid td.tip[title]").tooltip({
        showBody: "|"
    });
    $("#q").focus().select();
    $("body").on("keypress", '#quicksearch', function (e) {
        if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
            $('#qsearch').click();
            return false;
        }
        return true;
    });
});


