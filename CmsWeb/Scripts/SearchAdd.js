$(function() {
    $("a.formlink").live('click', function() {
        var f = $(this).closest('form');
        var q = f.serialize();
        $.post($(this).attr('href'), q, function(ret) {
            if (ret == 'close')
                if (self.parent.RebindMemberGrids)
                    self.parent.RebindMemberGrids($("#from").val());
            $(f).html(ret).ready(function() {
                $('.addrcol').cluetip({ splitTitle: '|' });
                $('#people > tbody > tr:even').addClass('altrow');
            });
        });
        return false;
    });
    $("a.namecol").live('click', function() {
        var f = $(this).closest('form');
        var q = f.serialize();
        $.post($(this).attr('href'), q, function(ret) {
            $(f).html(ret).ready(function() {
                $('#people > tbody > tr:even').addClass('altrow');
            });
        });
        return false;
    });
    
    $("form.DisplayEdit").submit(function() {
        if (!$("#submitit").val())
            return false;
        return true;
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

