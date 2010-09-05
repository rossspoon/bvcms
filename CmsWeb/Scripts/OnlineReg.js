$(function () {
    $(".submitbutton").button();
    $('form.DisplayEdit input.dob').live("blur", function () {
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
        var bd = bday.getDate();
        var age = 0;
        while (bday <= tday) {
            bday = new Date(by + age, bm, bd);
            age++;
        }
        age -= 2;
        var f = $(this).closest('form');
        $("#age", f).text(age);
    });
    //    $("#first").live("change", function () {
    //        $('#regnew').hide();
    //    });
    $("form.DisplayEdit a.submitbutton").live('click', function (ev) {
        ev.preventDefault();
        var f = $(this).closest('form');
        var q = f.serialize();
        $.post($(this).attr('href'), q, function (ret) {
            $(f).html(ret);
            $(".submitbutton", f).button();
        });
        return false;
    });
    $("form.DisplayEdit").submit(function () {
        if (!$("#submitit").val())
            return false;
        return true;
    });
    $("form.DisplayEdit a.cancel").live('click', function (ev) {
        ev.preventDefault();
        var f = $(this).closest('form');
        var q = f.serialize();
        $.post($(this).attr('href'), q, function (ret) {
            if (ret == 'refresh')
                location.reload();
            $(f).html(ret);
            $(".submitbutton", f).button();
        });
        return false;
    });
    $("#zip").live("blur", function () {
        $.post('/Register/CityState/' + $(this).val(), null, function (ret) {
            if (ret) {
                $('#state').val(ret.state);
                $('#city').val(ret.city);
            }
        }, 'json');
    });
    $("#copy").live("click", function () {
        $("input:last[name$='.emcontact']").val($("input:hidden:last[name$='.emcontact']").val());
        $("input:last[name$='.emphone']").val($("input:hidden:last[name$='.emphone']").val());
        $("input:last[name$='.insurance']").val($("input:hidden:last[name$='.insurance']").val());
        $("input:last[name$='.policy']").val($("input:hidden:last[name$='.policy']").val());
        $("input:last[name$='.doctor']").val($("input:hidden:last[name$='.doctor']").val());
        $("input:last[name$='.docphone']").val($("input:hidden:last[name$='.docphone']").val());
        $("input:last[name$='.mname']").val($("input:hidden:last[name$='.mname']").val());
        $("input:last[name$='.fname']").val($("input:hidden:last[name$='.fname']").val());
        $("input:last[name$='.paydeposit']").val($("input:hidden:last[name$='.paydeposit']").val());
        return false;
    });
});

