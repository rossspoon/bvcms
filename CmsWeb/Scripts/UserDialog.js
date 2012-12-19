$(function () {
    $(".submitbutton").button();
    $(".save").live('click', function (ev) {
        ev.preventDefault();
        var f = $(this).closest('form');
        var q = f.serialize();
        $.post($(this).attr('href'), q, function (ret) {
            if (ret != "ok")
                alert(ret);
            else
                self.parent.RebindUserInfoGrid();
        });
        return false;
    });
    $("#deluser").live('click', function (ev) {
        ev.preventDefault();
        if (!confirm("are you sure you want to delete?"))
            return false;
        var f = $(this).closest('form');
        var q = f.serialize();
        $.post($(this).attr('href'), q, function (ret) {
            self.parent.RebindUserInfoGrid();
        });
        return false;
    });
    $(".wrapper .item").transpose();
    $(".tip").tooltip({ showBody: "|", showURL: false });
});

