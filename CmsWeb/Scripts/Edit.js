$(function () {
    $('textarea.editor').ckeditor(function () { }, {
        height: 400,
        fullPage: true,
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
    });
    $('textarea.smalleditor').ckeditor(function () { }, {
        height: 100,
        filebrowserUploadUrl: '/Account/CKEditorUpload/',
        filebrowserImageUploadUrl: '/Account/CKEditorUpload/',
        toolbar_Full: [
    ['Source'],
    ['Bold', 'Italic', 'Underline'],
    ['Image'],
    ['Font', 'FontSize'],
    ['TextColor', 'BGColor'],
    ]
    });
});