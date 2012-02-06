$(function () {
    $("#applydonation").click(function (ev) {
        ev.preventDefault();
        return false;
    });
    $("a.submitbutton, a.submitlink, input.submitbutton.ajax").click(function (ev) {
        ev.preventDefault();
        var f = $(this).closest('form');
        var q = f.serialize();
        $.post($(this).attr('href'), q, function (ret) {
            if (ret.error) {
                $('#validatecoupon').text(ret.error);
            }
            else if (ret.amt) {
                $('#validatecoupon').text('');
                $('#amt').text(ret.amt);
                $('#pf_ti_Amt').val(ret.tiamt);
                $('#Coupon').val('');
                $('td.coupon').empty();
                $('#tdcoup').html("<i>Applied</i>");
            }
            else {
                window.location = ret.confirm;
            }
        });
        return false;
    });
    $('#Coupon').showPassword();
    
    $('#findidclick').click(function () {
        $("#findid").dialog({ width: 400 });
    });

    $("#Terms").dialog({ autoOpen: false });
    $("#displayterms").click(function () {
        $("#Terms").dialog("open");
    });

    if ($('#IAgree').attr("id")) {
        $("#Submit").attr("disabled", "disabled");
        $("a.submitbutton").attr("disabled", "disabled");
    }
    $("#IAgree").click(function () {
        var checked_status = this.checked;
        if (checked_status == true) {
            $("#Submit").removeAttr("disabled");
            $("a.submitbutton").removeAttr("disabled");
        }
        else {
            $("#Submit").attr("disabled", "disabled");
            $("a.submitbutton").attr("disabled", "disabled");
        }
    });
    $.validator.setDefaults({
        highlight: function (input) {
            $(input).addClass("ui-state-highlight");
        },
        unhighlight: function (input) {
            $(input).removeClass("ui-state-highlight");
        }
    });
    // validate signup form on keyup and submit
    $("form").validate({
        rules: {
            "pf.ti.Name": { required: true, maxlength: 50 },
            "pf.ti.Address": { required: true, maxlength: 50 },
            "pf.ti.City": { required: true, maxlength: 50 },
            "pf.ti.State": { required: true, maxlength: 4 },
            "pf.ti.Zip": { required: true, maxlength: 15 },
            "pf.ti.Email": { required: true, maxlength: 80 },
            "pf.ti.Phone": { maxlength: 50 },
            "pf.CreditCard": { digits: true },
            "pf.CCV": { digits: true, maxlength: 4 },
            "pf.Expires": { digits: true, maxlength: 4 }
        }
    });

});

