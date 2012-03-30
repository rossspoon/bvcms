$(function () {
    $('#pid').blur(function () {
        var tr, pid;
        if ($(this).val() == '')
            return false;
        if ($(this).val() == 'd') {
            tr = $('#bundle > tbody > tr:first');
            pid = $("a.pid", tr).text();
            $('#name').val($("td.name", tr).text());
            $('#checkno').val($("td.checkno", tr).text());
            $('#notes').val($("td.notes", tr).text());
            $('#amt').focus();
            $(this).val($.trim(pid));
            return true;
        }

        var q = $('#pbform').serialize();
        $.post("/PostBundle/GetNamePid/", q, function (ret) {
            if (ret.error == 'not found') {
                $.growlUI("PeopleId", "Not Found");
                $('#name').focus();
                $('#pid').val('');
            }
            else {
                $('#name').val(ret.name);
                $('#pid').val(ret.PeopleId);
                $('#amt').focus();
            }
        });
    });
    $(".bt").button();
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
    };
    $.Stripe();

    var keyallowed = true;

    $('#notes').keydown(function (event) {
        if (keyallowed && (event.keyCode == 9 || event.keyCode == 13) && !event.shiftKey) {
            event.preventDefault();
            keyallowed = false;
            $.PostRow({ scroll: false });
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
            $('#checkno').focus();
        }
    });
    $('#checkno').keydown(function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            $('#notes').focus();
        }
    });
    $('a.update').click(function (event) {
        event.preventDefault();
        $.PostRow({ scroll: true });
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
        $('#checkno').val($("td.checkno", tr).text());
        $('#notes').val($("td.notes", tr).text());
        tr.hide();
        if (a.val() == '0.00')
            a.val('');
        a.focus();
        $('a.edit').hide();
        $('a.split').hide();
        $('a.delete').hide();
        $.Stripe();
    });
    $('a.split').live("click", function (ev) {
        ev.preventDefault();
        var newamt = prompt("Amount to split out", "");
        newamt = parseFloat(newamt);
        if (isNaN(newamt))
            return false;
        var tr = $(this).closest("tr");

        var q = {
            pid: $("a.pid", tr).text(),
            name: $("td.name", tr).text(),
            fund: $("td.fund", tr).attr('val'),
            pledge: $("td.fund", tr).attr('pledge'),
            amt: newamt,
            splitfrom: tr.attr("cid"),
            checkno: $("td.checkno", tr).text(),
            notes: $("td.notes", tr).text(),
            id: $("#id").val()
        };
        $.PostRow({ scroll: true, q: q });
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
                $.Stripe();
            });
        }
    });
    $('a.pid').live("click", function (event) {
        event.preventDefault();
        var d = $('#searchDialog');
        $('iframe', d).attr("src", this.href);
        d.dialog("open");
    });
    $('#bundle').bind('mousedown', function (e) {
        if ($(e.target).hasClass("clickEdit")) {
            $(e.target).editable(function (value, settings) {
                $.post("/PostBundle/Edit/", { id: e.target.id, value: value }, function (ret) {
                    $('#totalitems').text(ret.totalitems);
                    $('#itemcount').text(ret.itemcount);
                    $('#a' + ret.cid).text(ret.amt);
                });
                return (value);
            }, {
                indicator: "<img src='/images/loading.gif'>",
                tooltip: "Click to edit...",
                style: 'display: inline',
                width: '4em',
                height: 25
            });
        }
        else if ($(e.target).hasClass("clickSelect")) {
            $(e.target).editable("/PostBundle/Edit/", {
                indicator: '<img src="/images/loading.gif">',
                loadtype: 'post',
                loadurl: "/PostBundle/Funds/",
                type: "select",
                submit: "OK",
                style: 'display: inline'
            });
        }
    });
    $.PostRow = function (options) {
        if (!options.q) {
            var n = parseFloat($('#amt').val());
            if (!n > 0) {
                $.growlUI("Contribution", "Cannot post, No Amount");
                return;
            }
            options.q = $('#pbform').serialize();
        }
        var action = "/PostBundle/PostRow/";
        var cid = $('#editid').val();
        if (cid)
            action = "/PostBundle/UpdateRow/";
        $.post(action, options.q, function (ret) {
            if (!ret)
                return;
            $('#totalitems').text(ret.totalitems);
            $('#itemcount').text(ret.itemcount);
            var pid = $('#pid').val();
            var tr;
            if (cid) {
                tr = $('tr[cid="' + cid + '"]');
                tr.replaceWith(ret.row);
                tr = $('tr[cid="' + cid + '"]');
            }
            else if (options.q.splitfrom) {
                tr = $('tr[cid="' + options.q.splitfrom + '"]');
                $('#a' + options.q.splitfrom).text(ret.othersplitamt);
                $(ret.row).insertAfter(tr);
                tr = $('tr[cid="' + ret.cid + '"]');
            }
            else {
                $('#bundle tbody').prepend(ret.row);
                tr = $('#bundle tbody tr:first');
            }
            $('td.name', tr).tooltip({ showBody: "|" });
            $('a.edit').show();
            $('a.split').show();
            $('a.delete').show();
            $(".bt", tr).button();
            $('#editid').val('');
            $('#entry input').val('');
            $('#fund').val($('#fundid').val());
            $.Stripe();
            $('#pid').focus();
            keyallowed = true;
            var top = tr.offset().top - 60;
            if (options.scroll == true) {
                $('html,body').animate({ scrollTop: top }, 1000);
            }
            tr.effect("highlight", {}, 3000);
        });
    };
    $('#searchDialog').dialog({
        title: 'Search/Add Contributor',
        bgiframe: true,
        autoOpen: false,
        width: 750,
        height: 700,
        modal: true,
        overlay: {
            opacity: 0.5,
            background: "black"
        }, close: function () {
            $('iframe', this).attr("src", "");
        }
    });
    $("#totalitems").text($("#titems").val());
    $("#totalcount").text($("#tcount").val());
});
function AddSelected(ret) {
    $('#searchDialog').dialog("close");
    var tr = $('tr[cid=' + ret.cid + ']');
    $('a.pid', tr).text(ret.pid);
    $('td.name', tr).text(ret.name);
    $('a.edit', tr).click();
}

