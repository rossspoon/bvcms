var currentDiv = null;
$(function () {
    CKEDITOR.config.jqueryOverrideVal = true;
    CKEDITOR.config.fullPage = false;
    CKEDITOR.config.enterMode = CKEDITOR.ENTER_BR;

    var editor_large = {
        autoParagraph: false,
        filebrowserUploadUrl: '/Account/CKEditorUpload/',
        filebrowserImageUploadUrl: '/Account/CKEditorUpload/',
        toolbar_Full: [
            ['Save'], ['Source'],
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

    $('div[bvedit]').bind('click', hClick).addClass("ti");
    $('td[bvrepeat]').mouseenter(hHoverIn).mouseleave(hHoverOut);
    $('#htmleditor').ckeditor(editor_large);

    $("#progress").dialog(
        {
            autoOpen: false,
            modal: true,
            close: function () {
                $('#progress').html("<h2>Working...</h2>");
            }
        });

    $("#popupeditor").dialog({ autoOpen: false, modal: true, closeOnEscape: true, title: "Edit Template Section", minWidth: 600, minHeight: 400 });
    $("#askName").dialog({ autoOpen: false, modal: true, closeOnEscape: true, title: "Save Draft", resizable: false, width: 'auto' });

    $("#Send").click(function () {
        var d = $("#progress");
        d.dialog('open');

        $('#body').val($("#tempateBody").html());
        var q = $("#SendEmail").serialize();

        $.post('/Email/QueueEmails', q, function (ret) {
            if (ret == "timeout") {
                window.location = "/Email/Timeout";
                return;
            }
            var taskid = ret.id;
            if (taskid == 0) {
                d.html(ret.content);
            } else {
                var intervalid = window.setInterval(function () {
                    $.post('/Email/TaskProgress/' + taskid, null, function (ret) {
                        if (ret.substr(0, 20).indexOf('<!--completed-->') >= 0)
                            window.clearInterval(intervalid);
                        d.html(ret);
                    });
                }, 3000);
            }
        });
    });

    $("#SaveDraft").click(function () {
        if ($(this).attr("saveType") == "0") {
            var d = $("#askName");
            d.dialog('open');
        } else {
            clearTemplateClass();
            $("#body").val($("#tempateBody").html());
            $("#name").val($("#newName").val());
            addTemplateClass();

            $("#SendEmail").attr("action", "/Email/SaveDraft");
            $("#SendEmail").submit();
        }
    });

    $("#SaveDraftButton").click(function () {
        clearTemplateClass();
        $("#body").val($("#tempateBody").html());
        $("#name").val($("#newName").val());
        addTemplateClass();

        $("#SendEmail").attr("action", "/Email/SaveDraft");
        $("#SendEmail").submit();
    });

    $("#TestSend").click(function () {
        var d = $("#progress");
        d.dialog('open');

        clearTemplateClass();
        $("#body").val($("#tempateBody").html());
        addTemplateClass();

        var q = $("#SendEmail").serialize();

        $.post('/Email/TestEmail', q, function (ret) {
            if (ret == "timeout") {
                window.location = "/Email/Timeout";
                return;
            }
            d.html(ret);
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
});



    function clearTemplateClass() {
        removeButtons();
        $("div[bvedit]").removeClass();
        $("div[bveditadd]").removeClass();
    }

    function addTemplateClass() {
        $("div[bveditadd]").addClass("ti");
        $("div[bvedit]").addClass("ti");
    }

    function hClick(e) {
        currentDiv = this;
        removeButtons();
        $('#htmleditor').ckeditorGet().setData($(this).html());
        $('#popupeditor').dialog("open");
    }

    function hClickAdd(e) {
        removeButtons();
        parentTR = $(currentHover).parent();
        $(parentTR).after("<tr><td bvrepeatadd>" + $(currentHover).html() + "</td></tr>");
        $('td[bvrepeatadd]').mouseenter(hAddHoverIn).mouseleave(hHoverOut);
        $('div[bvedit]').bind('click', hClick).addClass("ti");
    }

    function hHoverIn(ev) {
        currentHover = this;
        $(this).css("border", "solid 1px #ff0000");
        $(this).append("<div id='controlButtons' class='tiAdd'><input id='addButton' type='button' value='Copy Section' /></div>");
        $("#controlButtons").css("top", $(this).offset().top + 5).css("left", $(this).offset().left + 5);
        $("#addButton").bind("click", hClickAdd);
        ev.stopPropagation();
    }

    function hHoverOut(ev) {
        currentHover = null;
        $(this).css("border", "");
        removeButtons();
        ev.stopPropagation();
    }

    function removeButtons() {
        $("#controlButtons").remove();
    }

    function hAddHoverIn(ev) {
        currentHover = this;
        $(this).css("border", "solid 1px #ff0000");
        $(this).append("<div id='controlButtons' class='tiAdd'><input id='removeButton' type='button' value='Remove' /></div>");
        $("#controlButtons").css("top", $(this).offset().top + 5).css("left", $(this).offset().left + 5);
        $("#removeButton").bind('click', removeSection);
    }

    function updateDiv() {
        $(currentDiv).html($('#htmleditor').ckeditorGet().getData());
        $('#popupeditor').dialog("close");
    }
    
    function addRowAfter(ev) {
        $(this).parent().parent().after("<tr bvrepeatadd=''>" + $(this).html() + "</tr>");
        $("div[bvrepeatadd]").hover(hAddHoverIn, hHoverOut);
        ev.stopPropagation();
    }

    function removeSection(ev) {
        $(currentHover).parent().remove();
        ev.stopPropagation();
    }
