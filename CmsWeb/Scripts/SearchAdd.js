$(function() {
    $("a.formlink").live('click', function(ev) {
        ev.preventDefault();
        var f = $(this).closest('form');
        var q = f.serialize();
        $.post($(this).attr('href'), q, function(ret) {
            if (ret.close) {
                switch (ret.how) {
                    case 'rebindgrids':
                        if (self.parent.RebindMemberGrids)
                            self.parent.RebindMemberGrids($("#from").val());
                        break;
                    case 'addselected':
                        if (self.parent.AddSelected)
                            self.parent.AddSelected(ret);
                        break;
                    case 'addselected2':
                        if (self.parent.AddSelected2)
                            self.parent.AddSelected2(ret);
                        break;
                    case 'CloseAddDialog':
                        if (self.parent.CloseAddDialog)
                            self.parent.CloseAddDialog();
                        break;
                }
            }
            else
                $(f).html(ret).ready(function() {
                    $("a.bt").button();
                    $(".addrcol").tooltip({
                        showURL: false,
                        showBody: "|"
                    });
                    $('#people > tbody > tr:even').addClass('altrow');
                });
        });
        return false;
    });
    $("a.bt").button();
    $('a.clear').live('click', function(ev) {
        ev.preventDefault();
        $("#name").val('');
        $("#phone").val('');
        $("#address").val('');
        $("#dob").val('');
        return false;
    });
    $("#verifyaddress").live("click", function() {
        var f = $(this).closest('form');
        var q = f.serialize();
        $.post($(this).attr('href'), q, function(ret) {
            if (confirm(ret.address + "\nUse this Address?")) {
                $('#address', f).val(ret.Line1);
                $('#address2', f).val(ret.Line2);
                $('#city', f).val(ret.City);
                $('#state', f).val(ret.State);
                $('#zip', f).val(ret.Zip);
            }
        }, "json");
        return false;
    });
    $("#zip").live("blur", function() {
        $.post('/OnlineReg/CityState/' + $(this).val(), null, function(ret) {
            if (ret) {
                $('#state').val(ret.state);
                $('#city').val(ret.city);
            }
        }, 'json');
    });
    $("form input").live("keypress", function(e) {
        if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
            $('a.default').click();
            return false;
        }
        return true;
    });
});

