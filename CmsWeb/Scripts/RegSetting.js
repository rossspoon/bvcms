$(function () {
    CKEDITOR.replace('editor', {
        height: 200,
        filebrowserUploadUrl: '/Account/CKEditorUpload/',
        filebrowserImageUploadUrl: '/Account/CKEditorUpload/' 
    });
    $("body").on("click", 'ul.enablesort div.newitem > a', function (ev) {
        if (!$(this).attr("href"))
            return false;
        ev.preventDefault();
        var a = $(this);
        $.post(a.attr("href"), null, function (ret) {
            a.parent().prev().append(ret);
            a.parent().prev().find(".tip").tooltip({ opacity: 0, showBody: "|" });
        });
    });

    $.regsettingeditclick = function (f) {
        $(".tip", f).tooltip({ opacity: 0, showBody: "|" });
        $("ul.enablesort.sort, ul.enablesort ul.sort", f).sortable();
        $("ul.noedit input", f).attr("disabled", "disabled");
        $("ul.noedit select", f).attr("disabled", "disabled");
        $("ul.noedit a", f).not('[target="otherorg"]').removeAttr("href");
        $("ul.noedit a", f).not('[target="otherorg"]').css("color", "grey");
        $("ul.noedit a", f).not('[target="otherorg"]').unbind("click");
        $('#notifylist').SearchUsers({
            UpdateShared: function(topid) {
                $.post("/Organization/UpdateNotifyIds", { id: $("#OrganizationId").val(), topid: topid }, function (ret) {
                    $("#notifylist").html(ret);
                });
            }
        });
    };
    $.regsettingeditclick();
    $("a.editor").live("click", function (ev) {
        if (!$(this).attr("href"))
            return false;
        var name = $(this).attr("tb");
        ev.preventDefault();
        $("#editor").val($("#" + name).val());
        dimOn();
        $("#EditorDialog").center().show();
        $("#saveedit").off("click").on("click", function() {
            var v = $("#editor").val();
            $("#" + name).val(v);
            $("#" + name + "_ro").html(v);
            $('#EditorDialog').hide("close");
            dimOff();
        });
        return false;
    });
    $("#canceledit").live("click", function() {
        $('#EditorDialog').hide("close");
        dimOff();
    });
});
CKEDITOR.on('dialogDefinition', function (ev) {
    var dialogName = ev.data.name;
    var dialogDefinition = ev.data.definition;
    if (dialogName == 'link') {
        var advancedTab = dialogDefinition.getContents('advanced');
	advancedTab.label = "SpecialLinks";
        advancedTab.remove('advCSSClasses');
        advancedTab.remove('advCharset');
        advancedTab.remove('advContentType');
        advancedTab.remove('advStyles');
        advancedTab.remove('advAccessKey');
        advancedTab.remove('advName');
        advancedTab.remove('advLangCode');
        advancedTab.remove('advTabIndex');

        var relField = advancedTab.get('advRel');
        relField.label = "SmallGroup";
        var titleField = advancedTab.get('advTitle');
        titleField.label = "Message";
        var idField = advancedTab.get('advId');
        idField.label = "OrgId/MeetingId";
        var langdirField = advancedTab.get('advLangDir');
        langdirField.label = "Confirmation";
	langdirField.items[1][0] = "Yes, send confirmation";
	langdirField.items[2][0] = "No, do not send confirmation";
    }
});