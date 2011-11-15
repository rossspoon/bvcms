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
    }
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
    $("ul.enablesort a.del").live("click", function (ev) {
        if (!$(this).attr("href"))
            return false;
        ev.preventDefault();
        $(this).parent().parent().parent().remove();
    });

    $.regsettingeditclick = function (f) {
        $(".tip", f).tooltip({ opacity: 0, showBody: "|" });
        $("ul.enablesort ul.sort", f).sortable();
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