$(function() {
    $.fmtTable = function() {
        $("table.grid td.tip").tooltip({ showBody: "|" });
        $('table.grid > tbody > tr:even').addClass('alt');
    }
    $.fmtTable();
    $.loadTable = function() {
        $.getTable($('#groupsform'));
    }
    $('#Filter').live("click", function(ev) {
        ev.preventDefault();
        $.loadTable();
    });
    $("#groupsform").delegate("#memtype", "change", $.loadTable);

    $("#ingroup, #notgroup").keypress(function(ev) {
        if (ev.keyCode == '13') {
            ev.preventDefault();
            $.loadTable();
        }
    });

    $("#groupsform").delegate("#groupid", "change", $.loadTable);
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
    $("form").submit(function(ev) {
        ev.preventDefault();
        return false;
    });
    $.performAction = function(action) {
        if ($('#groupid').val() <= 0) {
            alert("select target group first");
            return false;
        }
        var q = $('form').serialize();
        $.post(action, q, function(ret) {
            $("table.grid > tbody").html(ret).ready($.fmtTable);
        });
        return false;
    };
    $('#AssignSelectedToTargetGroup').live('click', function(ev) {
        $.performAction("/OrgGroups/AssignSelectedToTargetGroup");
    });
    $('#RemoveSelectedFromTargetGroup').live('click', function(ev) {
        $.performAction("/OrgGroups/RemoveSelectedFromTargetGroup");
    });
});

