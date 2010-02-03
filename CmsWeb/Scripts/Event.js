$(function() {
    $('input.dob').blur(function() {
        var bday = new Date($(this).val());
        var by = bday.getFullYear();
        var bm = bday.getMonth();
        var bd = bday.getDate();
        var age = 0;
        var today = new Date();
        while (bday <= today) {
            bday = new Date(by + age, bm, bd);
            age++;
        }
        age -= 2;
        var f = $(this).closest('form');
        $("#age", f).text(age);
    });
    $("form.DisplayEdit a.submitbutton").live('click', function() {
        var f = $(this).closest('form');
        var q = f.serialize();
        $.post($(this).attr('href'), q, function(ret) {
            $(f).html(ret);
        });
        return false;
    });
    $("form.DisplayEdit").submit(function() {
        if (!$("#submitit").val())
            return false;
    });
    $("form.DisplayEdit a.cancel").live('click', function() {
        var f = $(this).closest('form');
        var q = f.serialize();
        $.post($(this).attr('href'), q, function(ret) {
            $(f).html(ret);
        });
        return false;
    });
});

