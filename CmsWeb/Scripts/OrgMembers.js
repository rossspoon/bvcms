$(function() {
    $.RefreshPage = function() {
        var f = $('form');
        var q = f.serialize();
        $.post("/OrgMembers/List", q, function(ret) {
            $(f).html(ret).ready(function() {
                $('table.grid > tbody > tr:even', f).addClass('alt');
            });
        });
    }
    $("form").delegate('#ProgId', "change", $.RefreshPage);
    $("form").delegate('#DivId', "change", $.RefreshPage);
    $("form").delegate('#SourceId', "change", $.RefreshPage);
    $("form").delegate('#TargetId', "change", $.RefreshPage);
    $("form").delegate('#MembersOnly', "change", $.RefreshPage);
    $("form").delegate('#move', "click", function(e) {
        e.preventDefault();
        var f = $('#form');
        var q = f.serialize();
        $.post("/OrgMembers/Move", q, function(ret) {
            $(f).html(ret).ready(function() {
                $('table.grid > tbody > tr:even', f).addClass('alt');
            });
        });
    });
    //    $('input.check').click(UpdateTotals);
    //    $('form table.grid a.sortable').click(function(ev) {
    //        var newsort = $(this).text();
    //        var oldsort = $("#Sort").val();
    //        $("#Sort").val(newsort);
    //        var dir = $("#Dir").val();
    //        if (oldsort == newsort && dir == 'asc')
    //            $("#Dir").val('desc');
    //        else
    //            $("#Dir").val('asc');
    //        RefreshList();
    //    });
    //    //$('#total').text($('.check').length);
    //    UpdateTotals = function() {
    //        $('#ttotal').text($('.check:checked').length);
    //    }
});
