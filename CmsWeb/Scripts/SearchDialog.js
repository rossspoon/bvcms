$(function() {
    $("form a.submitbutton").live('click', function() {
        var f = $(this).closest('form');
        $.showTable(f);
        return false;
    });
});

