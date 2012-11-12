$(function () {
    var editor_config = {
        height: 200,
        filebrowserUploadUrl: '/Account/CKEditorUpload/',
        filebrowserImageUploadUrl: '/Account/CKEditorUpload/',
        toolbar_Full: [
            ['Source'],
            ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'SpellChecker', 'Scayt'],
            ['Undo', 'Redo', '-', 'Find', 'Replace', '-', 'SelectAll', 'RemoveFormat'],
            '/',
            ['Bold', 'Italic', 'Underline', 'Strike', '-', 'Subscript', 'Superscript'],
            ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', 'Blockquote', 'CreateDiv'],
            ['JustifyLeft', 'JustifyCenter', 'JustifyRight'],
            ['Link', 'Unlink', 'Anchor'],
            ['Image', 'Table', 'SpecialChar'],
            '/',
            ['Styles', 'Format', 'Font', 'FontSize'],
            ['TextColor', 'BGColor'],
            ['Maximize', 'ShowBlocks', '-', 'About']
        ]
    };
    $("#editor").ckeditor(editor_config);
    $("ul.enablesort div.newitem > a").live("click", function (ev) {
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
    };
    $.regsettingeditclick();
    $("a.editor").live("click", function (ev) {
        if (!$(this).attr("href"))
            return false;
        var name = $(this).attr("tb");
        ev.preventDefault();
        $("#EditorDialog").dialog({
            width: 650,
            height: 450,
            modal: true,
            draggable: true,
            resizable: true,
            open: function () {
                $("#editor").val($("#" + name).val());
            },
            buttons: {
                'Save': function () {
                    var v = $("#editor").val();
                    $("#" + name).val(v);
                    $("#" + name + "_ro").html(v);
                    $(this).dialog('close');
                }
            }
        });
        return false;
    });
    $('#notifylist').live("click", function (e) {
        if (!$(this).attr("href"))
            return false;
        e.preventDefault();
        var d = $('#usersDialog');
        $('iframe', d).attr("src", this.href);
        d.dialog("open");
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