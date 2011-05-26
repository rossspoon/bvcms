$(function () {
    $("#clear").click(function () {
        $("input:text").val("");
    });
    $('#name').focus();
    $("#search").click(function (ev) {
        ev.preventDefault();
        $.getTable();
        return false;
    });
    $(".bt").button();
    $.getTable = function () {
        var f = $('#results').closest('form');
        var q = f.serialize();
        $.post($('#search').attr('href'), q, function (ret) {
            $('#results').replaceWith(ret).ready(function () {
                $('#results > tbody > tr:even').addClass('alt');
            });
        });
        return false;
    }
    $('#results > tbody > tr:even').addClass('alt');
    $('input:checkbox').live('change', function (ev) {
        var sp = $(this).parents('tr:eq(0)').find("span.move");
        var ck = $(this).is(":checked")
        var pid = $(this).attr("value");
        $.post("/SearchUsers/TagUntag/" + pid,
            { ischecked: !ck
            }, function (ret) {
                if (ck)
                    sp.html("<a href='#' class='move' value='" + pid + "'>move to top</a>");
                else
                    sp.empty();
            });
    });
    $("a.move").live("click", function (ev) {
        ev.preventDefault();
        var f = $('#results').closest('form');
        $("#topid").val($(this).attr("value"));
        var q = f.serialize();
        $.post("/SearchUsers/MoveToTop", q, function (ret) {
            $('#results').replaceWith(ret).ready(function () {
                $('#results > tbody > tr:even').addClass('alt');
            });
        });
    });
    $("#UpdateSelected").click(function (ev) {
        ev.preventDefault();
        self.parent.UpdateSelectedUsers($("#topid").val());
        return false;
    });
    $("a.select").live("click", function (ev) {
        ev.preventDefault();
        self.parent.UpdateSelectedUsers($(this).attr("value"));
        return false;
    });
    $("form input").live("keypress", function (e) {
        if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
            $('#search').click();
            return false;
        }
        return true;
    });
});


