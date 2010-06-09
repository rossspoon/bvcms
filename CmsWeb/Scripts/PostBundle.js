$(function() {
    $('#pid').blur(function() {
        var q = $('#pbform').serialize();
        $.post("/PostBundle/GetName/", q, function(ret) {
            $('#name').val(ret);
        });
    });
    $("#name").autocomplete("/PostBundle/Names", {
        minChars: 3,
        matchContains: false,
        selectFirst: false,
        autoFill: false,
        formatItem: function(row, pos, len) {
            return row[0] + " (" + row[1] + ")";
        },
        formatResult: function(row) {
            return row[0];
        }
    });
    $("#name").result(function(ev, data, formatted) {
        $('#pid').val(data[1]);
    });

    $.Stripe = function() {
        $('#bundle tbody tr:even').addClass('alt');
    }
    $.Stripe();
    $('#notes').keydown(function(event) {
        if (event.keyCode == 9) {
            event.preventDefault();
            var q = $('#pbform').serialize();
//            $.post("/PostBundle/PostRow/", q, function(ret) {
//            });
            $('#bundle tbody').prepend(
                '<tr><td>' + $('#pid').val()
                + '</td><td>' + $('#name').val()
                + '</td><td>' + $('#amt').val()
                + '</td><td>' + $('#fund').val()
                + '</td><td>' + $('#notes').val()
                + '</td></tr>');
            $('#entry input').val('');
            $('#pid').focus();
            $.Stripe();
        }
    });
});
