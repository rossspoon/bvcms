$(function () {
    $("form a.submitbutton").live('click', function () {
        var f = $(this).closest('form');
        $.getTable(f);
        return false;
    });
    $.gotoPage = function (e, pg) {
        var f = $(e).closest('form');
        $("#Page", f).val(pg);
        return $.getTable(f);
    };
    $.setPageSize = function (e) {
        var f = $(e).closest('form');
        $('#Page', f).val(1);
        $("#PageSize", f).val($(e).val());
        return $.getTable(f);
    };
    $('#people a.sortable').live('click', function () {
        var newsort = $(this).text();
        var sort = $("#Sort");
        var dir = $("#Direction");
        if ($(sort).val() == newsort && $(dir).val() == 'asc')
            $(dir).val('desc');
        else
            $(dir).val('asc');
        $(sort).val(newsort);
        var f = $(this).closest('form');
        $.getTable(f);
        return false;
    });
    $.getTable = function (f) {
        var q = f.serialize();
        $.post(f.attr('action'), q, function (ret) {
            $('#peoplediv').html(ret).ready(function () {
                $('#people > tbody > tr:even').addClass('altrow');
                $.DisplaySelect();
                $('.namecol').cluetip({ splitTitle: '|' });
            });
        });
        return false;
    };

    $.ClearForm = function (ev) {
        $('#searchForm .clearable').clearFields();
        return false;
    };
    $("#ClearForm").click($.ClearForm);
    $.DisplaySelect = function () {
        if ($("#AddToExisting").attr("checked")) {
            $("#people .select").hide();
            $("#people .add").show();
        }
        else {
            $("#people .add").hide();
            $("#people .select").show();
        }
    };
    $("#AddToExisting").click($.DisplaySelect);
    $.BindSelect = function () {
        $("#people .select").click(function (ui) {
            o.Select(o.Target.id, ui.target.id.substring(1));
            o.$this.dialog("close");
        });
        $("#people .add").click(o.AddNew);
        o.DisplaySelect();
    };

});

