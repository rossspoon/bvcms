$(function () {
    var addrtabs = $("#address-tab").tabs();
    $("#enrollment-tab").tabs();
    $("#member-tab").tabs();
    $("#growth-tab").tabs();
    $("#system-tab").tabs();
    var maintabs = $("#main-tab").tabs();
    $(".submitbutton,.bt").button();
    addrtabs.tabs('select', $('#addrtab').val());
    $('#dialogbox').SearchPeopleInit({ overlay: { background: "#000", opacity: 0.3} });
    $('#clipaddr').live('click', function () {
        var inElement = $('#addrhidden')[0];
        if (inElement.createTextRange) {
            var range = inElement.createTextRange();
            if (range)
                range.execCommand('Copy');
        }
        return false;
    });
    $('#deleteperson').click(function () {
        var href = $(this).attr("href");
        if (confirm('Are you sure you want to delete?')) {
            $.post(href, null, function (ret) {
                if (ret != "ok") {
                    $.blockUI({ message: "delete Failed: " + ret });
                    $('.blockOverlay').attr('title', 'Click to unblock').click($.unblockUI);
                }
                else {
                    $.blockUI({ message: "person deleted" });
                    $('.blockOverlay').attr('title', 'Click to unblock').click(function () {
                        $.unblockUI();
                        window.location = "/";
                    });
                }
            });
        }
        return false;
    });
    $('#moveperson').click(function (ev) {
        var href = $(this).attr("href");
        $('#dialogbox').SearchPeople(ev, function (id, peopleid) {
            $.post(href, { to: peopleid }, function (ret) {
                $('#dialogbox').dialog("close");
                if (ret != "ok") {
                    $.blockUI({ message: "Move Failed: " + ret });
                    $('.blockOverlay').attr('title', 'Click to unblock').click($.unblockUI);
                }
                else {
                    $.blockUI({ message: "Move succeeded" });
                    $('.blockOverlay').attr('title', 'Click to unblock').click(function () {
                        $.unblockUI();
                        window.location.reload();
                    });
                }
            });
        });
        return false;
    });

    $.extraEditable = function () {
        $('.editarea').editable('/Person/EditExtra/', {
            type: 'textarea',
            cancel: 'Cancel',
            submit: 'OK',
            indicator: '<img src="/images/loading.gif">',
            tooltip: 'Click to edit...'
        });
        $(".clickEdit").editable("/Person/EditExtra/", {
            indicator: "<img src='/images/loading.gif'>",
            tooltip: "Click to edit...",
            style: 'display: inline',
            width: '300px',
            height: 25,
            submit: 'OK'
        });
    }
    $.getTable = function (f, q) {
        q = q || f.serialize();
        $.post(f.attr('action'), q, function (ret) {
            $(f).html(ret).ready(function () {
                $('table.grid > tbody > tr:even', f).addClass('alt');
                //$('.dropdown', f).hoverIntent(dropdownshow, dropdownhide);
                $('.bt').button();
                $(".datepicker").datepicker();
                $.extraEditable();
            });
        });
        return false;
    }
    $('#memberDialog').dialog({
        title: 'Member Dialog',
        bgiframe: true,
        autoOpen: false,
        width: 600,
        height: 550,
        modal: true,
        overlay: {
            opacity: 0.5,
            background: "black"
        }, close: function () {
            $('iframe', this).attr("src", "");
        }
    });
    $('form a.membertype').live("click", function (e) {
        e.preventDefault();
        var d = $('#memberDialog');
        $('iframe', d).attr("src", this.href);
        d.dialog("open");
    });
    $('#previous-tab form a.membertype').live("click", function (e) {
        e.preventDefault();
        var d = $('#memberDialog');
        $('iframe', d).attr("src", this.href);
        d.dialog("open");
    });

    $(".CreateAndGo").click(function () {
        if (confirm($(this).attr("confirm")))
            $.post($(this).attr("href"), null, function (ret) {
                window.location = ret;
            });
        return false;
    });

    $("#enrollment-link").click(function () {
        $.showTable($('#current-tab form'));
    });
    $("#previous-link").click(function () {
        $.showTable($('#previous-tab form'));
    });
    $("#pending-link").click(function () {
        $.showTable($('#pending-tab form'));
    });
    $("#attendance-link").click(function () {
        $.showTable($('#attendance-tab form'));
    });
    $("#growth-link").click(function () {
        $("#contacts-tab form").each(function () {
            $.showTable($(this));
        });
    });
    $("#system-link").click(function () {
        $.showTable($("#extras-tab form"));
        $.extraEditable();
    });
    $("#changes-link").click(function () {
        $.showTable($("#changes-tab form"));
    });
    $("#duplicates-link").click(function () {
        $.showTable($("#duplicates-tab form"));
    });
    $("#user-link").click(function () {
        $.showTable($("#user-tab form"));
    });
    $('#family table.grid > tbody > tr:even').addClass('alt');
    $("#recreg-link").click(function () {
        var f = $('#recreg-tab form')
        if ($('table', f).size() > 0)
            return false;
        var q = f.serialize();
        $.post(f.attr('action'), q, function (ret) {
            $(f).html(ret);
        });
    });

    $("a.displayedit").live('click', function (ev) {
        ev.preventDefault();
        var f = $(this).closest('form');
        $.post($(this).attr('href'), null, function (ret) {
            $(f).html(ret).ready(function () {
                var acopts = {
                    minChars: 3,
                    matchContains: 1
                };
                $('#Employer', f).autocomplete("/Person/Employers", acopts);
                $('#School', f).autocomplete("/Person/Schools", acopts);
                $('#Occupation', f).autocomplete("/Person/Occupations", acopts);
                $('#NewChurch,#PrevChurch', f).autocomplete("/Person/Churches", acopts);
                $(".datepicker").datepicker({
                    dateFormat: 'm/d/yy',
                    changeMonth: true,
                    changeYear: true
                });
                $(".submitbutton,.bt").button();
                $('.dropdown', f).hoverIntent(dropdownshow, dropdownhide);
                $("#verifyaddress").click(function () {
                    var f = $(this).closest('form');
                    var q = f.serialize();
                    $.post($(this).attr('href'), q, function (ret) {
                        if (confirm(ret.address + "\nUse this Address?")) {
                            $('#Address1', f).val(ret.Line1);
                            $('#Address2', f).val(ret.Line2);
                            $('#City', f).val(ret.City);
                            $('#State', f).val(ret.State);
                            $('#Zip', f).val(ret.Zip);
                        }
                    });
                    return false;
                });
                return false;
            });
        });
        return false;
    });
    $("form.DisplayEdit a.submitbutton").live('click', function (ev) {
        ev.preventDefault();
        var f = $(this).closest('form');
        if (!$(f).valid())
            return;
        var q = f.serialize();
        $.post($(this).attr('href'), q, function (ret) {
            $(f).html(ret).ready(function () {
                var bc = $('#businesscard');
                $.post($(bc).attr("href"), null, function (ret) {
                    $(bc).html(ret);
                });
                $('.dropdown', f).hoverIntent(dropdownshow, dropdownhide);
                $(".submitbutton,.bt").button();
            });
        });
        return false;
    });
    $("#future").live('click', function (ev) {
        ev.preventDefault();
        var f = $(this).closest('form');
        var q = f.serialize();
        $.post($(f).attr("action"), q, function (ret) {
            $(f).html(ret);
        });
    });
    $("form.DisplayEdit").submit(function () {
        if (!$("#submitit").val())
            return false;
    });
    $.validator.setDefaults({
        highlight: function (input) {
            $(input).addClass("ui-state-highlight");
        },
        unhighlight: function (input) {
            $(input).removeClass("ui-state-highlight");
        },
        rules: {
            "NickName": { maxlength: 15 },
            "Title": { maxlength: 10 },
            "First": { maxlength: 25, required: true },
            "Middle": { maxlength: 15 },
            "Last": { maxlength: 100, required: true },
            "Suffix": { maxlength: 10 },
            "AltName": { maxlength: 100 },
            "Maiden": { maxlength: 20 },
            "HomePhone": { maxlength: 20 },
            "CellPhone": { maxlength: 20 },
            "WorkPhone": { maxlength: 20 },
            "EmailAddress": { maxlength: 150 },
            "School": { maxlength: 60 },
            "Employer": { maxlength: 60 },
            "Occupation": { maxlength: 60 },
            "WeddingDate": { date: true },
            "Birthday": { date: true },
            "DeceasedDate": { date: true },
            "Grade": { number: true },
            "Address1": { maxlength: 40 },
            "Address2": { maxlength: 40 },
            "City": { maxlength: 30 },
            "Zip": { maxlength: 15 },
            "FromDt": { date: true },
            "ToDt": { date: true }
        }
    });
    $('#addrf').validate();
    $('#addrp').validate();
    $('#basic').validate();
    $(".atck").live("change", function (ev) {
        var ck = $(this);
        $.post("/Meeting/MarkAttendance/", {
            MeetingId: $(this).attr("mid"),
            PeopleId: $(this).attr("pid"),
            Present: ck.is(':checked')
        }, function (ret) {
            if (ret.error) {
                ck.attr("checked", !ck.is(':checked'));
                alert(ret.error);
            }
            else {
                var f = ck.closest('form');
                var q = f.serialize();
                $.post($(f).attr("action"), q, function (ret) {
                    $(f).html(ret);
                });
            }
        });
    });
    $("#newextravalue").live("click", function (ev) {
        ev.preventDefault();
        $("#newvalueform").dialog({
            buttons: {
                "Ok": function () {
                    var v = $("input[name='typeval']:checked").val();
                    $.post("/Person/NewExtraValue/" + $("#PeopleId").val(), { field: $("#fieldname").val(), type: v }, function (ret) {
                        if (ret.startsWith("error"))
                            alert(ret);
                        else {
                            $.getTable($("#extras-tab form"));
                            $.extraEditable();
                        }
                    });
                    $(this).dialog("close");
                    $(this).dialog("destroy");
                }
            }
        });
        return false;
    });
    $("#deleteextra").live("click", function (ev) {
        ev.preventDefault();
                    $.post("/Person/DeleteExtra/" + $("#PeopleId").val(), { field: $("#fieldname").val() }, function (ret) {
                        if (ret.startsWith("error"))
                            alert(ret);
                        else {
                            $.getTable($("#extras-tab form"));
                            $.extraEditable();
                        }
                    });
                    $(this).dialog("close");
                    $(this).dialog("destroy");
                }
            }
        });
        return false;
    });
});
function RebindMemberGrids(from) {
    $.updateTable($('#current-tab form'));
    $.updateTable($('#pending-tab form'));
    $("#memberDialog").dialog('close');
}
function RebindUserInfoGrid(from) {
    $.updateTable($('#user-tab form'));
    $("#memberDialog").dialog('close');
}
