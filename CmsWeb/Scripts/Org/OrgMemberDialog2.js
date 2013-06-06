$(function () {
    $(".ckbox").click(function (ev) {
        $.post("/OrgMemberDialog2/CheckBoxChanged/" + $(this).attr("id"), {
            ck: $(this).is(":checked")
        });
        return true;
    });
    $("body").on("click", 'a.delete', function (ev) {
        if (confirm("are you sure?"))
            $.post($(this).attr("href"), null, function (ret) {
                self.parent.RebindMemberGrids($("#from").val());
            });
        return false;
    });
    $("body").on('click', 'a.move', function (ev) {
        ev.preventDefault();
        var f = $(this).closest('form');
        if (confirm("are you sure?"))
            $.post($(this).attr('href'), null, function (ret) {
                self.parent.RebindMemberGrids($("#from").val());
            });
        return false;
    });
});

