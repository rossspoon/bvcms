$(function () {
    $(".submitbutton").button();
    $.dpoptions = {
        changeMonth: true,
        changeYear: true,
        yearRange: 'c-99:c+0',
        onSelect: function (dateText, inst) {
            var f = $(this).closest('form');
            $("#age", f).text($.dodate(dateText));
        }
    };
    $("#dob").datepicker($.dpoptions);
    $.dodate = function (bd) {
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
        return age - 2;
    };
    $('form.DisplayEdit input.dob').live("blur", function () {
        var f = $(this).closest('form');
        $("#age", f).text($.dodate($(this).val()));
    });
    $("a.submitbutton, a.submitlink, input.submitbutton.ajax", "form.DisplayEdit").live('click', function (ev) {
        ev.preventDefault();
        var f = $(this).closest('form');
        var q = f.serialize();
        $.post($(this).attr('href'), q, function (ret) {
            if (ret.substr(0, 13) === '/Person/Index') {
                window.location = ret;
                return;
            } else if (ret.substr(0, 30) === '/OnlineReg/ManageSubscriptions') {
                window.location = ret;
                return;
            }
            $(f).html(ret).ready(function () {
                $.InstructionsShow();
            });
            $(".submitbutton", f).button();
            $("#dob").datepicker($.dpoptions);
        });
        return false;
    });
    $.InstructionsShow = function () {
        $("div.instructions").hide();
        if ($("#personedit").attr("id")) {
            $("#fillout").hide();
            $("div.instructions.find").show();
        }
        else if ($("#otheredit").attr("id"))
            $("div.instructions.options").show();
        else if ($("#username").attr("id"))
            $("div.instructions.login").show();
        else if ($("#submitit").attr("id"))
            $("div.instructions.submit").show();
    }
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
        $.post('/OnlineReg/CityState/' + $(this).val(), null, function (ret) {
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
    $.InstructionsShow();
});

