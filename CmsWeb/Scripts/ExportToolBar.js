$(document).ready(function() {
    $("ul.sf-tab").superfish({
        autoArrows: false
    });
    $("#ChooseLabelType").dialog({
        overlay: { background: "#000", opacity: 0.3 },
        bgiframe: true,
        modal: true,
        autoOpen: false,
        closeOnEscape: true,
        width: 400
    });
    $(".ChooseLabelType").click(function(ev) {
        $("#ChooseLabelType").dialog("open");
        $("#cmdOK").click(function() {
            var url = ev.target.href
                + "&titles=" + $('#UseTitle')[0].checked
                + "&format=" + $('input[name=addressedto]:checked').val()
                + "&web=" + $('#WebView')[0].checked;
            $("#ChooseLabelType").dialog("close");
            window.open(url, "_blank");
        });
        return false;
    });
    $('#TagAll,#UnTagAll').click(function(ev) {
        $.block();
        $.post($(this).attr("href"), null, function(ret) {
            $(".taguntag").text(ret);
            $.unblock();
            return false;
        });
        return false;
    });
    $.QueryString = function(q, item) {
        var r = new Object();
        $.each(q.split('&'), function() {
            var kv = this.split('=');
            r[kv[0]] = kv[1];
        });
        return r[item];
    };
    $.block = function() {
        $.blockUI({ message: 'working on it...<img src="/content/loading.gif"/>' });
    };
    $.unblock = function() {
        $.unblockUI({ fadeOut: 150 });
    };
    $.navigate = function(url, data) {
        url += (url.match(/\?/) ? "&" : "?") + data;
        window.location = url;
    };
});
String.prototype.startsWith = function(t, i) {
    return (t == this.substring(0, t.length));
}
String.prototype.appendQuery = function(q) {
    if (this && this.length > 0)
        return this + '&' + q;
    return q;
}
String.prototype.endsWith = function(t, i) {
    return (t == this.substring(this.length - t.length));
}
String.prototype.addCommas = function() {
    var x = this.split('.');
    var x1 = x[0];
    var x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}



