$(function () {
    $('#pid').blur(function () {
        if ($(this).val() == '')
            return;
        var q = $('#pbform').serialize();
        $.post("/PostBundle/GetName/", q, function (ret) {
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
    $('td.name').tooltip({ showBody: "|" });
    $("#name").autocomplete("/PostBundle/Names", {
        minChars: 3,
        matchContains: false,
        mustMatch: true,
        width: 200,
        selectFirst: false,
        autoFill: false,
        formatItem: function (row, pos, len) {
            return row[0] + " (" + row[2] + ")<br />" + row[3];
        },
        formatResult: function (row) {
            return row[0];
        }
    });
    $("#name").result(function (ev, data, formatted) {
        if (data) {
            $('#pid').val(data[1]);
        }
        if (this.value === '') {
            $.growlUI("Name", "Not Found");
            $('#pid').val('');
        }
    });

    $.Stripe = function () {
        $('#bundle tbody tr').removeClass('alt');
        $('#bundle tbody tr:even').addClass('alt');
    }
    $.Stripe();
    $('#notes').keydown(function (event) {
        if ((event.keyCode == 9 || event.keyCode == 13) && !event.shiftKey) {
            event.preventDefault();
            $.PostRow();
        }
    });
    $('#pid').keydown(function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            if (!$.browser.msie)
                $('#pid').blur();
            $('#name').focus();
        }
    });
    $('#name').keydown(function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            $('#amt').focus();
        }
    });
    $('#amt').keydown(function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            $('#fund').focus();
        }
    });
    $('#fund').keydown(function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            $('#notes').focus();
        }
    });
    $('a.update').click(function (event) {
        event.preventDefault();
        $.PostRow();
    });
    $('a.edit').live("click", function (ev) {
        ev.preventDefault();
        var tr = $(this).closest("tr");
        $('#editid').val(tr.attr("cid"));
        $('#pid').val($("a.pid", tr).text());
        $('#name').val($("td.name", tr).text());
        $('#fund').val($("td.fund", tr).attr('val'));
        $('#pledge').attr('checked', $("td.fund", tr).attr('pledge') == 'true');
        var a = $('#amt');
        a.val($("td.amt", tr).attr("val"));
        $('#fund').val($("td.fund", tr).attr("val"));
        $('#notes').val($("td.notes", tr).text());
        tr.remove();
        if (a.val() == '0.00')
            a.val('');
        a.focus();
        $('a.edit').hide();
        $.Stripe();
    });
    $('a.delete').live("click", function (ev) {
        ev.preventDefault();
        if (confirm("are you sure?")) {
            var tr = $(this).closest("tr");
            $('#editid').val(tr.attr("cid"));
            var q = $('#pbform').serialize();
            $.post("/PostBundle/DeleteRow/", q, function (ret) {
                tr.remove();
                $('#totalitems').text(ret.totalitems);
                $('#itemcount').text(ret.itemcount);
                $('#editid').val('');
                q = $('#pbform').serialize();
                $.post("/PostBundle/FundTotals/", q, function (ret) {
                    $("fundtotals").html(ret);
                });
            });
        }
    });
    $('a.pid').live("click", function (event) {
        event.preventDefault();
        var d = $('#searchDialog');
        $('iframe', d).attr("src", this.href);
        d.dialog("open");
    });
    $.MakeEditable = function () {
        $(".clickEdit").editable("/PostBundle/Edit/", {
            indicator: "<img src='/images/loading.gif'>",
            tooltip: "Click to edit...",
            style: 'display: inline',
            width: '200px'
        });
        $(".clickSelect").editable("/PostBundle/Edit/", {
            indicator: '<img src="/images/loading.gif">',
            loadtype: 'post',
            loadurl: "/PostBundle/Funds/",
            type: "select",
            submit: "OK",
            style: 'display: inline'
        });
    }
    $.MakeEditable();
    $.PostRow = function () {
        var n = parseFloat($('#amt').val());
        if (!n > 0) {
            $.growlUI("Contribution", "Cannot post, No Amount");
            return;
        }
        var q = $('#pbform').serialize();
        var action = "/PostBundle/PostRow/";
        if ($('#editid').val())
            action = "/PostBundle/UpdateRow/";
        $.post(action, q, function (ret) {
            if (!ret)
                return;
            $('#totalitems').text(ret.totalitems);
            $('#itemcount').text(ret.itemcount);
            var pid = $('#pid').val();
            $('#bundle tbody').prepend(
                    '<tr cid="' + ret.cid
                    + '"><td><a href="/SearchAdd/Index/' + ret.cid
                    + '?type=contributor" class="pid">' + (pid || 'select')
                    + '</a></td><td class="name" title="' + ret.tip
                    + '">' + $('#name').val()
                    + '</td><td class="amt" val="' + $('#amt').val()
                    + '" align="right">' + ret.amt
                    + '</td><td val="' + $('#fund').val() + '"class="fund" pledge="' + ret.pledge
                    + '"><span id="f' + ret.cid + '" class="clickSelect">' + ret.fund 
                    + '</span></td><td class="notes">' + $('#notes').val()
                    + '</td><td><a class="edit" href="#">edit</a>'
                    + '</td><td><a class="delete" href="#">delete</a></td>'
                    + '</tr>');
            $('#bundle tbody tr:first td.name').tooltip({ showBody: "|" });
            $('#editid').val('');
            $('#entry input').val('');
            $('#fund').val($('#fundid').val());
            $.MakeEditable();
            $('#pid').focus();
            $('a.edit').show();
            $('a.update').show();
            $.Stripe();
            q = $('#pbform').serialize();
            $.post("/PostBundle/FundTotals/", q, function (ret) {
                $("fundtotals").html(ret);
            });
        });
    }
    $('#searchDialog').dialog({
        title: 'Search/Add Contributor',
        bgiframe: true,
        autoOpen: false,
        width: 712,
        height: 630,
        modal: true,
        overlay: {
            opacity: 0.5,
            background: "black"
        }, close: function () {
            $('iframe', this).attr("src", "");
        }
    });
});
function AddSelected(ret) {
    $('#searchDialog').dialog("close");
    var tr = $('tr[cid=' + ret.cid + ']');
    $('a.pid', tr).text(ret.pid);
    $('td.name', tr).text(ret.name);
    $('a.edit', tr).click();
}

