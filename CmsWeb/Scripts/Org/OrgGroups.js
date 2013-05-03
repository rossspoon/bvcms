$(function () {
    $.fmtTable = function () {
        $("table.grid td.tip").tooltip({ showBody: "|" });
        $('table.grid > tbody > tr:even').addClass('alt');
        $(".bt").button();
    }
    $.fmtTable();
    $(".helptip").tooltip({ showBody: "|" });
    $.loadTable = function() {
        $.block();
        $.getTable($('#groupsform'));
        $.unblock();
    };
    $('body').on("click", '#filter', function (ev) {
        ev.preventDefault();
        $.loadTable();
    });
    $('body').on("click", 'a.sortable', function (ev) {
        ev.preventDefault();
        $('#sort').val($(this).text());
        $.loadTable();
    });
    $("#groupsform").delegate("#memtype", "change", $.loadTable);

    $("#SelectAll").click(function () {
        if ($(this).attr("checked"))
            $("table.grid input[name='list']").attr('checked', true);
        else
            $("table.grid input[name='list']").removeAttr('checked');
    });

    $("#ingroup, #notgroup").keypress(function (ev) {
        if (ev.keyCode == '13') {
            ev.preventDefault();
            $.loadTable();
        }
    });
    //$("#groupidDD").multiselect();
    $("#groupsform").delegate("#groupid", "change", $.loadTable);
    $.getTable = function (f) {
        var q = f.serialize();
        $.post("/OrgGroups/Filter", q, function (ret) {
            $('table.grid > tbody').html(ret).ready($.fmtTable);
        });
        return false;
    }
    $(".datepicker").datepicker();

    $("body").on("click", '#SelectAll', function () {
        $("input[name='list']").attr('checked', $(this).attr('checked'));
    });
    $("body").on('click', 'a.display', function (ev) {
        ev.preventDefault();
        var f = $(this).closest('form');
        $.post(this.href, q, function (ret) {
            $(f).html(ret).ready(function () {
                $.fmtTable();
                return false;
            });
        });
        return false;
    });
    $("body").on('click', 'a.groupmanager', function (ev) {
        ev.preventDefault();
        if (confirm("are you sure?")) {
            var f = $(this).closest('form');
            var q = f.serialize();
            $.block();
            $.post($(this).attr('href'), q, function (ret) {
                if (ret.substring(0, 5) != "error") {
                    f.html(ret).ready(function () {
                        if ($('#newgid').val())
                            $('#groupid').val($('#newgid').val());
                        $('#GroupName').val('');
                        $.fmtTable();
                    });
                }
                $.unblock();
            });
        }
    });
    $("form").submit(function (ev) {
        ev.preventDefault();
        return false;
    });
    $.performAction = function (action) {
        if ($('#groupid').val() <= 0) {
            $.growlUI("error", 'select target group first');
            return false;
        }
        $.block();
        var q = $('form').serialize();
        $.post(action, q, function (ret) {
            $("table.grid > tbody").html(ret).ready($.fmtTable);
            $.unblock();
        });
        return false;
    };
    $('body').on('click', '#AssignSelectedToTargetGroup', function (ev) {
        $.performAction("/OrgGroups/AssignSelectedToTargetGroup");
    });
    $('body').on('click', '#RemoveSelectedFromTargetGroup', function (ev) {
        $.performAction("/OrgGroups/RemoveSelectedFromTargetGroup");
    });
    var lastChecked = null;
    $("body").on("click", "input[name = 'list']", function (e) {
        if (!lastChecked) {
            lastChecked = this;
            return;
        }
        if (e.shiftKey) {
            var start = $("input[name = 'list']").index(this);
            var end = $("input[name = 'list']").index(lastChecked);
            $("input[name = 'list']").slice(Math.min(start, end), Math.max(start, end) + 1).attr('checked', lastChecked.checked);
        }
        lastChecked = this;
    });

});

