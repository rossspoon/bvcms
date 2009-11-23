$(document).ready(function() {
    $.blockUI.defaults.growlCSS = {
        width: '350px',
        top: '40%',
        left: '35%',
        right: '10px',
        border: 'none',
        padding: '5px',
        opacity: '0.7',
        cursor: null,
        color: '#fff',
        backgroundColor: '#000',
        '-webkit-border-radius': '10px',
        '-moz-border-radius': '10px'
    };
    $.growlUI2 = function(title, message, timeout) {
        var $m = $('<div class="growlUI2"></div>');
        if (title) $m.append('<h1>' + title + '</h1>');
        if (message) $m.append('<h2>' + message + '</h2>');
        if (timeout == undefined) timeout = 3000;
        $.blockUI({
            message: $m, fadeIn: 400, fadeOut: 700, centerY: false,
            timeout: timeout, showOverlay: false,
            css: $.blockUI.defaults.growlCSS
        });
    };
    $.growlUI1 = function(title, message, timeout) {
        var $m = $('<div class="growlUI"></div>');
        if (title) $m.append('<h1>' + title + '</h1>');
        if (message) $m.append('<h2>' + message + '</h2>');
        if (timeout == undefined) timeout = 3000;
        $.blockUI({
            message: $m, fadeIn: 400, fadeOut: 700, centerY: false,
            timeout: timeout, showOverlay: false,
            css: $.blockUI.defaults.growlCSS
        });
    };
    $('#KeyCard').keypress(function(e) {
        var key = window.event ? e.keyCode : e.which;
        if (key != 13)
            return;
        if ($('#newCard').attr('checked'))
            $.post("/CheckIn/NewKeyCard/", {
                KeyCode: $('#KeyCard').val(),
                pid: $('#PeopleId').val()
            }, function(ret) {
                $.growlUI('Notification', ret);
                $('#newCard').attr('checked', false);
            });
        else
            $.post("/CheckIn/PostCheckIn/" + $('#OrgId').val(), {
                KeyCode: $('#KeyCard').val()
            }, function(ret) {
                if (ret.pid > 0)
                    window.location = "/CheckIn/CheckIn/" + $('#OrgId').val() + "?pid=" + ret.pid;
                else
                    $.growlUI2('Notification', 'KeyCode not found');
            }, "json");
        $('#KeyCard').val('');
    });
    $('#newCard').click(function() {
        $('#KeyCard').focus();
    });
    $('#tkup').click(function() {
        $.blockUI({ message: $('#GOdialog'), css: { width: '275px'} });
    });

    $('#GOrefresh').click(function() {
        $.unblockUI();
        window.location = "/CheckIn/CheckIn/" + $('#OrgId').val() + "?pid=" + $('#PeopleId').val();
    });

    $('#KeyCard').focus();
});
