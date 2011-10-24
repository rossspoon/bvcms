$(function () {
    $("#clear").click(function () {
        $("input:text").val("");
    });
    $('#name').focus();
    $("#search").click(function (ev) {
        ev.preventDefault();
        $.getTable();
        return false;
    });
    $(".bt").button();
    $.getTable = function () {
        var f = $('#results').closest('form');
        var q = f.serialize();
        $.post($('#search').attr('href'), q, function (ret) {
            $('#results').replaceWith(ret).ready($.formatTable);
        });
        return false;
    }
    $("ul.sort").sortable();
    $("#UpdateSelected").click(function (ev) {
        ev.preventDefault();
        var list = $('input[type=checkbox]:checked').map(function () {
            return $(this).val();
        }).get().join(',');
        self.parent.UpdateSelectedOrgs(list);
        return false;
    });
    $("#close").click(function () {
        window.parent.$('#orgsDialog').dialog('close');
    });
    $.formatTable = function () {
        $("td.tooltip").tooltip({
            showURL: false,
            showBody: "|"
        });
        $('#results > tbody > tr:even').addClass('alt');
    };
    $.formatTable();
    $.SaveOrgIds = function (ev) {
        var list = $('input[type=checkbox]:checked').map(function () {
            return $(this).val();
        }).get().join(',');
        $.post("/SearchOrgs/SaveOrgIds/" + $("#id").val(), { oids: list });
    };
    $('input:checkbox').live('change', $.SaveOrgIds);
    $("form input").live("keypress", function (e) {
        if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
            $('#search').click();
            return false;
        }
        return true;
    });
});


