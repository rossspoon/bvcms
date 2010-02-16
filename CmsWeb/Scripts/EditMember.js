$(function() {
    $("a.delete").click(function(ev) {
        if (confirm("are you sure?"))
            $.post("/EditMember/Delete/" + $(this).attr("id"), null, function(ret) {
                self.parent.CloseIFrameDialog();
            });
        return false;
    });
    $(".ckbox").click(function(ev) {
        $.post("/OrgMemberDialog/CheckBoxChanged/" + $(this).attr("id"), {
            ck: $(this).is(":checked")
        });
        return true;
    });
    $("a.display").live('click', function() {
        var f = $(this).closest('form');
        $.post($(this).attr('href'), null, function(ret) {
            $(f).html(ret).ready(function() {
                var acopts = {
                    minChars: 3,
                    matchContains: 1
                };
                $(".datepicker").datepicker({
                    dateFormat: 'm/d/yy',
                    changeMonth: true,
                    changeYear: true
                });
                return false;
            });
        });
        return false;
    });
    $("a.move").live('click', function() {
        var f = $(this).closest('form');
        if (confirm("are you sure?"))
            $.post($(this).attr('href'), null, function(ret) {
                    self.parent.CloseIFrameDialog();
            });
        return false;
    });
    $("form.DisplayEdit a.submitbutton").live('click', function() {
        var f = $(this).closest('form');
        var q = f.serialize();
        $.post($(this).attr('href'), q, function(ret) {
            $(f).html(ret);
        });
        return false;
    });
});

