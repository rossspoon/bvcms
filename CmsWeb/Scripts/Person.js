$(function() {
    var $addrtabs = $("#address-tab").tabs();
    var $maintabs = $("#main-tab").tabs();
    $addrtabs.tabs('select', $('#addrtab').val());
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
});
