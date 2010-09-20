$(function() {
    $(".submitbutton").button();
    $("form.DisplayEdit a.submitbutton").live('click', function(ev) {
        ev.preventDefault();
        var f = $(this).closest('form');
        var q = f.serialize();
        $.post($(this).attr('href'), q, function(ret) {
            if (ret)
                $(f).html(ret);
            else
                self.parent.RebindUserInfoGrid();
        });
        return false;
    });
});

