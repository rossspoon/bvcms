$(function() {
    $('#pid').blur(function() {
        if ($(this).val() == '')
            return;
        var q = $('#pbform').serialize();
        $.post("/PostBundle/GetName/", q, function(ret) {
            if (ret == 'not found') {
                $.growlUI("PeopleId", "Not Found");
                $('#name').focus();
                $('#pid').val('');
            }
            else {
                $('#name').val(ret);
                $('#amt').focus();
            }
        });
    });
    $("#name").autocomplete("/PostBundle/Names", {
        minChars: 3,
        matchContains: false,
        mustMatch: true,
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
        if (data) {
            $('#pid').val(data[1]);
        }
        if (this.value === '') {
            $.growlUI("Name", "Not Found");
            $('#pid').val('');
        }
    });

    $.Stripe = function() {
        $('#bundle tbody tr').removeClass('alt');
        $('#bundle tbody tr:even').addClass('alt');
    }
    $.Stripe();
    $('#notes').keydown(function(event) {
        if (event.keyCode == 9 && !event.shiftKey) {
            event.preventDefault();
            $.PostRow();
        }
    });
    $('a.update').click(function(event) {
        event.preventDefault();
        $.PostRow();
    });
    $('a.edit').live("click", function() {
        $('#editid').val($(this).attr("cid"));
        var tr = $(this).closest("tr");
        $('#pid').val($("td.pid", tr).text());
        $('#name').val($("td.name", tr).text());
        $('#amt').val($("td.amt", tr).attr("val"));
        $('#fund').val($("td.fund", tr).attr("val"));
        $('#notes').val($("td.notes", tr).text());
        $(tr).remove();
        $('#amt').focus();
        $('a.update').show();
        $('a.edit').hide();
    });
    $.PostRow = function() {
        if (!$('#pid').val()) {
            $.growlUI("Contribution", "Cannot post, No PeopleId");
            return;
        }
        var n = parseFloat($('#amt').val());
        if (!n > 0) {
            $.growlUI("Contribution", "Cannot post, No Amount");
            return;
        }
        var q = $('#pbform').serialize();
        var action = "/PostBundle/PostRow/";
        if ($('#editid').val())
            action = "/PostBundle/UpdateRow/";
        $.post(action, q, function(ret) {
            $('#totalitems').text(ret.totalitems);
            $('#itemcount').text(ret.itemcount);
            $('#bundle tbody').prepend(
                    '<tr><td class="pid">' + $('#pid').val()
                    + '</td><td class="name">' + $('#name').val()
                    + '</td><td class="amt" val="' + $('#amt').val()
                    + '" align="right">' + ret.amt
                    + '</td><td val="' + $('#fund').val()
                    + '" class="fund">' + ret.fund
                    + '</td><td class="notes">' + $('#notes').val()
                    + '</td><td><a class="edit" cid="' + ret.cid
                    + '" href="#">edit</a></td></tr>');
            $('#editid').val('');
            $('#entry input').val('');
            $('#fund').val($('#fundid').val());
            $('#pid').focus();
            $('a.edit').show();
            $('a.update').hide();
            $.Stripe();
        });
    }
});
