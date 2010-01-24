$(function() {
    var addrtabs = $("#address-tab").tabs();
    $("#enrollment-tab").tabs();
    $("#member-tab").tabs();
    $('table.grid > tbody > tr:even').addClass('alt');
    var maintabs = $("#main-tab").tabs();
    addrtabs.tabs('select', $('#addrtab').val());
    $('#dialogbox').SearchPeopleInit({ overlay: { background: "#000", opacity: 0.3} });
    $('#clipaddr').click(function() {
        var inElement = $('#addrhidden')[0];
        if (inElement.createTextRange) {
            var range = inElement.createTextRange();
            if (range)
                range.execCommand('Copy');
        }
        return false;
    });
    $('#deleteperson').click(function() {
        if (confirm('Are you sure you want to delete?')) {
            $.post("/Person/Delete/" + $(this).attr("pid"), null, function(ret) {
                window.location.reload();
            });
        }
    });
    $('#moveperson').click(function(ev) {
        var pid = $(this).attr("pid");
        $('#dialogbox').SearchPeople(ev, function(id, peopleid) {
            $.post("/Person/Move/" + pid, { to: peopleid }, function(ret) {
                $('#dialogbox').dialog("close");
                if (ret) {
                    $.blockUI({ message: "Move Failed: " + ret });
                    $('.blockOverlay').attr('title', 'Click to unblock').click($.unblockUI);
                }
                else {
                    $.blockUI({ message: "Move succeeded" });
                    $('.blockOverlay').attr('title', 'Click to unblock').click(function() {
                        $.unblockUI();
                        window.location.reload();
                    });
                }
            });
        });
        return false;
    });
    $(".CreateAndGo").click(function() {
        if (confirm($(this).attr("confirm")))
            $.post($(this).attr("href"), null, function(ret) {
                window.location = ret;
            });
        return false;
    });
});
function GotoPage(e, pg) {
    return GetTable(e, "#Page", pg);
}
function SetPageSize(e, sz) {
    return GetTable(e, "#PageSize", sz);
}
function GetTable(e, arg0, arg1) {
    var f = $(e).closest('form');
    $(arg0, f).val(arg1);
    var q = f.serialize();
    $.post(f.attr('action'), q, function(ret) {
        $(f).replaceWith(ret).ready(function() {
            $('table.grid > tbody > tr:even', f).addClass('alt');
        });
    });
    return false;
}

