$(function () {
    $.fmtTable = function () {
        $("table.grid td.tip").tooltip({ showBody: "|" });
        $('table.grid > tbody > tr:even').addClass('alt');
    }
    $.fmtTable();
    $(".bt").button();
    $(".filter").change(function (ev) {
        var q = $("form").serialize();
        $.post("/OrgMembersDialog/Filter", q, function (ret) {
            $("table.grid > tbody").html(ret).ready($.fmtTable);
        });
    });

    $(".datepicker").datepicker({
        dateFormat: 'm/d/yy',
        changeMonth: true,
        changeYear: true
    });

    $("#SelectAll").click(function () {
        $("input[name='list']").attr('checked', $(this).attr('checked'));
    });
    $("a.display").live('click', function (ev) {
        ev.preventDefault();
        var f = $(this).closest('form');
        $.post(this.href, null, function (ret) {
            $(f).html(ret).ready(function () {
                var acopts = {
                    minChars: 3,
                    matchContains: 1
                };
                return false;
            });
        });
        return false;
    });
    $("a.delete").live("click", function (ev) {
        if (confirm("are you sure?"))
            $.post($(this).attr("href"), null, function (ret) {
                self.parent.RebindMemberGrids($("#from").val());
            });
        return false;
    });
    $("a.move").live('click', function (ev) {
        ev.preventDefault();
        var f = $(this).closest('form');
        if (confirm("are you sure?"))
            $.post($(this).attr('href'), null, function (ret) {
                self.parent.RebindMemberGrids($("#from").val());
            });
        return false;
    });
    $("form.DisplayEdit a.submitbutton").live('click', function (ev) {
        ev.preventDefault(); p
        var f = $(this).closest('form');
        var q = f.serialize();
        $.post($(this).attr('href'), q, function (ret) {
            self.parent.RebindMemberGrids($("#from").val());
        });
        return false;
    });
});

