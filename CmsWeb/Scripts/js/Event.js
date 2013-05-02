$(function() {
    $('body').on("blur", 'form.DisplayEdit input.dob', function () {
        var bd = $(this).val();
        var re0 = /^(0[1-9]|1[012])(0[1-9]|[12][0-9]|3[01])((19|20)?[0-9]{2})$/i;
        var re = /^(0?[1-9]|1[012])[\/-](0?[1-9]|[12][0-9]|3[01])[\/-]((19|20)?[0-9]{2})$/i;
        var m = re0.exec(bd);
        if (m == null)
            m = re.exec(bd);
        if (m == null)
            return;

        var y = parseInt(m[3]);
        if (y < 1000)
            if (y < 50) y = y + 2000; else y = y + 1900;
        var bday = new Date(y, m[1] - 1, m[2]);
        var tday = new Date();
        if (bday > tday)
            bday = new Date(y - 100, m[1] - 1, m[2]);

        var by = bday.getFullYear();
        var bm = bday.getMonth();
        bd = bday.getDate();
        var age = 0;
        while (bday <= tday) {
            bday = new Date(by + age, bm, bd);
            age++;
        }
        age -= 2;
        var f = $(this).closest('form');
        $("#age", f).text(age);
    });
    $("body").on('click', 'form.DisplayEdit a.submitbutton', function (ev) {
        ev.preventDefault();
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
        return true;
    });
    $("body").on('click', 'form.DisplayEdit a.cancel', function (ev) {
        ev.preventDefault();
        var f = $(this).closest('form');
        var q = f.serialize();
        $.post($(this).attr('href'), q, function(ret) {
            $(f).html(ret);
        });
        return false;
    });
    $("body").on("blur", "#zip", function () {
        $.post('/OnlineReg/CityState/' + $(this).val(), null, function(ret) {
            if (ret) {
                $('#state').val(ret.state);
                $('#city').val(ret.city);
            }
        }, 'json');
    });
});

