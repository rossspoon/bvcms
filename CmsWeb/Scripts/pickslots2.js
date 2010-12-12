$(function () {
    $('td.slot').tooltip({
        track: true,
        delay: 0,
        showURL: false,
        showBody: " - ",
        fade: 250
    });
    function trim(str) {
        str = str.replace(new RegExp("^[\\s]+", "g"), "");
        return str.replace(new RegExp("[\\s]+$", "g"), "");
    }
    $("input").live("click", function (sender) {
        var sid = this.id;
        var $this = $(this);
        var ck = this.checked;
        $.post('/OnlineReg/ToggleSlot/' + $('#pid').val(), { oid: $('#oid').val(), slot: this.id, ck: ck }, function (ret) {
            if (!ret) {
                $this.attr('checked', !ck);
                return;
            }
            var a = ret.split("<!-- -->");
            $this.parent().replaceWith(a[1]).ready(function () {
                $('td.slot[title]').tooltip({
                    track: true,
                    delay: 0,
                    showURL: false,
                    showBody: " - ",
                    fade: 250
                });
            });

            switch (trim(a[0])) {
                case "Yours":
                    $.growlUI1('Notification', 'Time slot is now yours');
                    break;
                case 'Open':
                    $.growlUI1('Notification', 'Time slot is now open');
                    break;
                case 'Taken':
                    $.growlUI2('Notification', 'Sorry, time slot has recently been taken by someone else');
                    break;
                case 'Limit':
                    $.growlUI2('Notification', 'Your limit has been reached');
                    break;
                case 'NoChange':
                    $.growlUI2('Notification', 'No change');
                    break;
            }
            return false;
        });
    });
});
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
$.growlUI2 = function (title, message, timeout) {
    var $m = $('<div class="growlUI2"></div>');
    if (title) $m.append('<h1>' + title + '</h1>');
    if (message) $m.append('<h2>' + message + '</h2>');
    if (timeout == undefined) timeout = 1000;
    $.blockUI({
        message: $m, fadeIn: 400, fadeOut: 700, centerY: false,
        timeout: timeout, showOverlay: false,
        css: $.blockUI.defaults.growlCSS
    });
};
$.growlUI1 = function (title, message, timeout) {
    var $m = $('<div class="growlUI"></div>');
    if (title) $m.append('<h1>' + title + '</h1>');
    if (message) $m.append('<h2>' + message + '</h2>');
    if (timeout == undefined) timeout = 1000;
    $.blockUI({
        message: $m, fadeIn: 400, fadeOut: 700, centerY: false,
        timeout: timeout, showOverlay: false,
        css: $.blockUI.defaults.growlCSS
    });
};
