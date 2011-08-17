$(function () {
    $(".bt").button();
    $("ul.sort").sortable();
    $("a.newitem").click(function (ev) {
        ev.preventDefault();
        var a = $(this);
        $.post(a.attr("href"), null, function (ret) {
            a.parent().prev().append(ret);
        });
    });
    $("a.del").live("click", function (ev) {
        ev.preventDefault();
        $(this).parent().remove();
    });
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
    $("span.edithtml").each(function () {
        var v = $(this).parent().next().val();
        if (v.length > 0)
            $(this).addClass("bold");
    });
    $("#editor").ckeditor(editor_config);
    $("a.editor").live("click", function (ev) {
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
                    var n = $("#" + name);
                    n.val(v);
                    var s = n.prev().find("span.edithtml");
                    s.removeClass("bold");
                    if (v.length > 0)
                        s.addClass("bold");
                    $(this).dialog('close');
                }
            }
        });
        return false;
    });
    $('#save').click(function (ev) {
        ev.preventDefault();
        var f = $(this).closest("form");
        var q = f.serialize();
        $.post("/RegSetting/UpdateGui/" + $("#Id").val(), q, function (ret) {
            if (ret == "ok") {
                window.location = "/Organization/Index/" + $("#Id").val();
                return false;
            }
            alert(ret);
        });
        return false;
    });
});