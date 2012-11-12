$(function () {
    $("#applydonation").click(function (ev) {
        ev.preventDefault();
        return false;
    });
    $("a.submitbutton, a.submitlink, input.submitbutton.ajax").click(function (ev) {
        ev.preventDefault();
        if (!agreeterms) {
            alert("must agree to terms");
            return false;
        }
        var f = $(this).closest('form');
        var q = f.serialize();
        $.post($(this).attr('href'), q, function (ret) {
            if (ret.error) {
                $('#validatecoupon').text(ret.error);
            } else if (ret.amt) {
                $('#validatecoupon').text('');
                $('#amt').text(ret.amt);
                $('#pf_AmtToPay').val(ret.tiamt);
                $('#pf_Amtdue').val(ret.amtdue);
                $('#pf_Coupon').val('');
                $('td.coupon').html(ret.msg);
            } else {
                window.location = ret.confirm;
            }
        });
        return false;
    });
    $('#pf_Coupon').showPassword();

    $('#findidclick').click(function () {
        $("#findid").dialog({ width: 400 });
    });
    var agreeterms = true;
    $("form").submit(function () {
        if (!agreeterms) {
            alert("must agree to terms");
            return false;
        }
        if (!$("#Submit").val())
            return false;
        if ($("form").valid())
            $("#Submit").attr("disabled", "true");
        return true;
    });

    $("#Terms").dialog({ autoOpen: false });
    $("#displayterms").click(function () {
        $("#Terms").dialog("open");
    });

    if ($('#IAgree').attr("id")) {
        $("#Submit").attr("disabled", "disabled");
        $("a.submitbutton").attr("disabled", "disabled");
        $("#ApplyCoupon").attr("disabled", "disabled");
        agreeterms = false;
    }
    $("#IAgree").click(function () {
        var checked_status = this.checked;
        if (checked_status == true) {
            agreeterms = true;
            $.EnableSubmit();
            $("#ApplyCoupon").removeAttr("disabled");
        } else {
            agreeterms = false;
            $("#Submit").attr("disabled", "disabled");
            $("a.submitbutton").attr("disabled", "disabled");
            $("#ApplyCoupon").attr("disabled", "disabled");
        }
    });
    $.ShowPaymentInfo = function (v) {
        $(".Card").hide();
        $(".Bank").hide();
        if (v === 'C')
            $(".Card").show();
        else if (v === 'B')
            $(".Bank").show();
        $("#Submit").attr("disabled", "true");
        $.EnableSubmit();
    };
    $.EnableSubmit = function () {
        if ($("#pf_Type").val() && agreeterms) {
            $("#Submit").removeAttr("disabled");
            $("a.submitbutton").removeAttr("disabled");
        }
    };
    $("input[name=Type]").live("change", function () {
        var v = $("input[name=Type]:checked").val();
        $("#pf_Type").val(v);
        $.ShowPaymentInfo(v);
    });
    if ($("#allowcc").val()) {
        $.ShowPaymentInfo($("#pf_Type").val()); // initial setting
    }
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
            "pf.Name": { required: true, maxlength: 50 },
            "pf.Address": { required: true, maxlength: 50 },
            "pf.City": { required: true, maxlength: 50 },
            "pf.State": { required: true, maxlength: 4 },
            "pf.Zip": { required: true, maxlength: 15 },
            "pf.Email": { required: true, maxlength: 80 },
            "pf.Phone": { maxlength: 50 }
        }
    });

});

