$(function() {
    $('form.DisplayEdit input.dob').live("blur", function() {
        var bd = $(this).val();
        if (bd.length == 6)
            bd = bd.substr(0, 1) + '/' + bd.substr(2, 3) + '/' + bd.substr(4, 5);
        else
            bd = bd.replace("-", "/");
        var d = bd.split("/");
        var y = parseInt(d[2]);
        if (y < 30)
            y += 2000;
        if (y < 100)
            y += 1900;
        var bday = new Date(y, d[0] - 1, d[1]);
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
    $("#zip").live("blur", function() {
        $.post('/Register/CityState/' + $(this).val(), null, function(ret) {
            if (ret) {
                $('#state').val(ret.state);
                $('#city').val(ret.city);
            }
        }, 'json');
    });
});

