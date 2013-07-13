$(function () {
    $.gotoPage = function (e, pg) {
        var f = $(e).closest('form');
        $("#Page", f).val(pg);
        return $.getTable(f);
    };
    $.setPageSize = function (ev, size) {
        var f = $(ev).closest('form');
        $('#Page', f).val(1);
        $("#PageSize", f).val(size);
        return $.getTable(f);
    };

    $.getTable = function (f) {
        $.ajax({
            type: 'POST',
            url: f.attr("action"),
            data: f.serialize(),
            success: function (data, status) {
                f.html(data);
            }
        });
        return false;
    };
    $('table.grid > thead a.sortable').live("click", function () {
        var f = $(this).closest("form");
        var newsort = $(this).text();
        var sort = $("#Sort", f);
        var dir = $("#Direction", f);
        if ($(sort).val() == newsort && $(dir).val() == 'asc')
            $(dir).val('desc');
        else
            $(dir).val('asc');
        $(sort).val(newsort);
        $.getTable(f);
        return false;
    });
    $.showTable = function (f) {
        if ($('table.table', f).size() == 0)
            $.getTable(f);
        return false;
    };
    $.updateTable = function (f) {
        if ($('table.table', f).size() > 0)
            $.getTable(f);
        return false;
    };
    $("body").on("click", "input[name='toggletarget']", function (ev) {
        if ($('a.target[target="people"]').length == 0) {
            $("a.target").attr("target", "people");
            $("input[name='toggletarget']").attr("checked", true);
        } else {
            $("a.target").removeAttr("target");
            $("input[name='toggletarget']").removeAttr("checked");
        }
    });
});
