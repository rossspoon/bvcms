$(function () {
    $('a.editconditionlink').live("click", function () {
        var qid = $(this).attr("qid");
        $.post('/Search/Advanced/EditCondition/' + qid, null, function (ret) {
            $("#conditions").replaceWith(ret).ready($.AdjustEditCondition);
        });
        return false;
    });
    $.AdjustEditCondition = function() {
        $(".bt").button();
        $('#CodeValues').multiselect();
        $("#editconditionli").css("height", $("#editcondition").outerHeight() + "px");
        var pos = $("#editconditionli").position();
        $("#editcondition").css("left", pos.left).css("top", pos.top);
    };
    $('#Comparison').live("change", function (ev) {
        if ($("#Codesdiv").length > 0) {
            var q = $('#conditionForm').serialize();
            $.post('/Search/Advanced/CodesDropdown', q, function (ret) {
                $("#CodesDiv").html(ret).ready(function () {
                    $('#CodeValues').multiselect();
                });
            });
        }
    });

    $('#QueryConditionHelp').dialog({
        title: 'Help on Condition',
        bgiframe: true,
        autoOpen: false,
        width: 750,
        height: 700,
        modal: true,
        overlay: {
            opacity: 0.5,
            background: "black"
        }, close: function () {
            $('iframe', this).attr("src", "");
        }
    });
    $('a.help').live("click", function (event) {
        event.preventDefault();
        var d = $('#QueryConditionHelp');
        if (this.href.endsWith('-'))
            $('iframe', d).attr("src", this.href + $("#ConditionName").val());
        else
            $('iframe', d).attr("src", this.href);
        d.dialog("open");
    });
    $("a.help,.helptip").tooltip({ showBody: "|", showURL: false });
    $('#Tags').click(function (ev) {
        $('#TagsPopup').show();
    });
    $(".datepicker").datepicker();
    $(".bt").button();
    $("#editcondition select").css("width", "100%");

    $('#Program').live("change", function (ev) {
        $.post('/Search/Advanced/Divisions/' + $(this).val(), null, function (ret) {
            $("#Division").replaceWith(ret);
            $("#Organization").replaceWith("<select id='Organization' name='Organization'><option value='0'>(not specified)</option></select>")
        });
    });
    $('#Division').live("change", function (ev) {
        $.post('/Search/Advanced/Organizations/' + $(this).val(), null, function (ret) {
            $("#Organization").replaceWith(ret);
        });
    });
    $('#AddCondition').live("click", function () {
        var qs = $('#conditionForm').serialize();
        $.post('/Search/Advanced/AddCondition/', qs, function (ret) {
            $("#conditions").replaceWith(ret).ready(function () {
                $(".bt").button();
            });
        });
        return false;
    });
    $('#AddConditionToGroup').live("click", function () {
        var qs = $('#conditionForm').serialize();
        $.post('/Search/Advanced/AddConditionToGroup/', qs, function (ret) {
            $("#conditions").replaceWith(ret).ready($.AdjustEditCondition);
        });
        return false;
    });
    $('#SaveCondition').live("click", function () {
        var qs = $('#conditionForm').serialize();
        $.post('/Search/Advanced/SaveCondition/', qs, function (ret) {
            $("#conditions").replaceWith(ret).ready(function () {
                $(".bt").button();
            });
        });
        return false;
    });
    $('#RemoveCondition').live("click", function () {
        var qid = $("#SelectedId").val();
        $.post('/Search/Advanced/RemoveCondition/' + qid, null, function (ret) {
            $("#conditions").replaceWith(ret).ready(function () {
                $(".bt").button();
            });
        });
        return false;
    });
    $('#Run').click(function (ev) {
        var qs = $('#conditionForm').serialize();
        RefreshList();
        return false;
    });
    $('#Export').click(function (ev) {
        window.location = "/Search/Advanced/Export/" + $("#QueryId").val();
    });
    var dialogOptions = {
        overlay: { background: "#000", opacity: 0.3 },
        modal: true,
        autoOpen: false,
        closeOnEscape: true,
        width: 500
    };
    $('#SaveQueryDiv').dialog(dialogOptions);
    $('#OpenQueryDiv').dialog(dialogOptions);

    $('#ShowSaveQuery').live("click", function (ev) {
        $('#SaveQueryDesc').val($('#Description').text());
        $('#SaveQueryDiv').dialog("open");
    });
    $('#ShowOpenQuery').live("click", function (ev) {
        $.post("/Search/Advanced/SavedQueries", null, function (ret) {
            $('#ExistingQueries').fillOptions(ret);
        });
        $('#OpenQueryDiv').dialog("open");
        return false;
    });
    $('#OpenQuery').click(function (ev) {
        $('#OpenQueryDiv').dialog("close");
        var a = $("#ExistingQueries").val().split(':');
        window.location = "/Search/Advanced/Main/" + a[0];
    });
    $('#SaveQuery').click(function (ev) {
        $('#SaveQueryDiv').dialog("close");
        $.post("/Search/Advanced/SaveQuery/", {
            SavedQueryDesc: $('#SaveQueryDesc').val(),
            IsPublic: $('#IsPublic').is(':checked')
        }, function (ret) {
            $("#Description").text(ret);
        });
        return false;
    });
    if ($("#AutoRun").val() == "True")
        $("#Run").click();

    $('#QueryConditionSelect').dialog({
        overlay: { background: "#000", opacity: 0.3 },
        bgiframe: true,
        modal: true,
        autoOpen: false,
        closeOnEscape: true,
        width: 750,
        height: 575,
        position: 'top'
    });
    $("#tabber").tabs();
    
    $("#SelectCondition").live("click", function (ev) {
        $('#QueryConditionSelect').dialog("open");
        return false;
    });
    $('div.FieldLink a').click(function (ev) {
        ev.preventDefault();
        var qid = $("#SelectedId").val();
        $.post('/Search/Advanced/SelectCondition/' + qid, { conditionName: ev.target.id }, function (ret) {
            $('#QueryConditionSelect').dialog("close");
            $("#conditions").replaceWith(ret).ready(function () {
                $('#CodeValues').multiselect();
                $(".bt").button();
            });
        });
        return false;
    });
    $("a.closeit").click(function (ev) {
        $.unblockUI();
    });
    $.navigate = function (url, data) {
        url += (url.match(/\?/) ? "&" : "?") + data;
        window.location = url;
    };
    $.fn.enabled = function (bool) {
        if (bool)
            $(this).attr("href", "#").removeClass("disabled");
        else
            $(this).removeAttr("href").addClass("disabled");
        return this;
    };
});

function RefreshList() {
    $.block();
    var qs = $("#conditionForm").serialize();
    $.ajax({
        type: "POST",
        url: "/Search/Advanced/Results/",
        data: qs,
        timeout: 1200000, // in milliseconds
        success: function (ret) {
            $('#toolbar').show();
            $('#Results').html(ret);
            $('#people tbody tr:even').addClass('alt');
            $('a.taguntag').click(function (ev) {
                $.post('/Search/Advanced/ToggleTag/' + $(this).attr('value'), null, function (ret) {
                    if (ret.error)
                        alert(ret.error);
                    else
                        $(ev.target).text(ret.HasTag ? "Remove" : "Add");
                });
                return false;
            });
            $('#people thead a.sortable').click(function (ev) {
                var newsort = $(this).text();
                var oldsort = $("#Sort").val();
                $("#Sort").val(newsort);
                var dir = $("#Direction").val();
                if (oldsort == newsort && dir == 'asc')
                    $("#Direction").val('desc');
                else
                    $("#Direction").val('asc');
                RefreshList();
                return false;
            });
            $.unblock();
        },
        error: function (request, status, err) {
            $.unblock();
            alert(err);
        }
    });
}
function GotoPage(pg) {
    qs = $('#conditionForm').serialize() + "&Page=" + pg;
    RefreshList();
    return false;
}
function SetPageSize(sz) {
    qs = $('#conditionForm').serialize() + "&PageSize=" + sz;
    RefreshList();
    return false;
}

function ShowErrors(j) {
    $('.validate').each(function () {
        $(this).next(".error").remove();
    });
    var e = eval('(' + j + ')');
    if (e.count == 0)
        return false;
    $('.validate').each(function () {
        if (e[this.id])
            $(this).after("<span class='error'> " + e[this.id] + "</span>");
    });
    return true;
}