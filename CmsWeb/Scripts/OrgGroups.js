$(function() {
    $.fmtTable = function() {
        $("table.grid td.tip").tooltip({ showBody: "|" });
        $('table.grid > tbody > tr:even').addClass('alt');
    }
    $.fmtTable();
    $(".filter").change(function(ev) {
        var q = $("form").serialize();
        $.post("/OrgGroups/Filter", q, function(ret) {
            $("table.grid > tbody").html(ret).ready($.fmtTable);
        });
    });

    $("#groupsform").delegate("#groupid", "change", function() {
        $.getTable($('#groupsform'));
        return false;
    });
    $.getTable = function(f) {
        var q = f.serialize();
        $.post("/OrgGroups/Filter", q, function(ret) {
            $('table.grid > tbody').html(ret).ready($.fmtTable);
        });
        return false;
    }
    $(".datepicker").datepicker({
        dateFormat: 'm/d/yy',
        changeMonth: true,
        changeYear: true
    });

    $("#SelectAll").click(function() {
        $("input[name='list']").attr('checked', $(this).attr('checked'));
    });
    $("a.display").live('click', function() {
        var f = $(this).closest('form');
        $.post(this.href, q, function(ret) {
            $(f).html(ret).ready(function() {
                return false;
            });
        });
        return false;
    });
    $("a.groupmanager").live('click', function() {
        var f = $(this).closest('form');
        var q = f.serialize();
        if (confirm("are you sure?"))
            $.post($(this).attr('href'), q, function(ret) {
                $("#ManageGroups").html(ret);
            });
        return false;
    });
    $("form").submit(function() {
        if ($('#groupid').val() <= 0) {
            alert("select active group first");
            return false;
        }
        var f = $(this).closest('form');
        var q = f.serialize();
        $.post("/OrgGroups/Update", q, function(ret) {
            $("table.grid > tbody").html(ret).ready($.fmtTable);
        });
        return false;
    });

});

