$(document).ready(function() {
    $("#ChooseLabelType").dialog({
        overlay: { background: "#000", opacity: 0.3 },
        bgiframe: true,
        modal: true,
        autoOpen: false,
        closeOnEscape: true,
        width: 400
    });
    $(".ChooseLabelType").live("click", function(ev) {
        ev.preventDefault();
        var d = $("#ChooseLabelType");
        d.dialog("open");

        var sep = ev.target.href.search(/\?/) == -1 ? "?" : "&";
        $("#cmdOK").unbind("click").click(function(ev2) {
            ev2.preventDefault();
            var url = ev.target.href
                + sep + "titles=" + $('#UseTitle')[0].checked
                + "&format=" + $('input[name=addressedto]:checked').val()
                + "&usephone=" + $('#UsePhone')[0].checked;
            $("#ChooseLabelType").dialog("close");
            window.open(url);
            return false;
        });
        return false;
    });
    $('#TagAll,#UnTagAll').live("click", function(ev) {
        ev.preventDefault();
        $.block();
        $.post(this.href, null, function(ret) {
            $(".taguntag:visible").text(ret);
            $.unblock();
        });
        return false;
    });
    $('#AddContact').live("click", function(ev) {
        ev.preventDefault();
        if (!confirm("Are you sure you want to add a contact for all these people?"))
            return false;
        $.block();
        $.post(this.href, null, function(ret) {
            $.unblock();
            if (ret < 0)
                alert("too many people to add to a contact (max 100)");
            else if (ret == 0)
                alert("no results");
            else
                window.location = "/Contact.aspx?id=" + ret;
        });
        return false;
    });
    $('#AddTasks').live("click", function(ev) {
        ev.preventDefault();
        if (!confirm("Are you sure you want to add a task for each of these people?"))
            return false;
        $.block();
        $.post(this.href, null, function(ret) {
            $.unblock();
            if (ret < 0)
                alert("too many people to add tasks for (max 100)");
            else if (ret == 0)
                alert("no results");
            else
                window.location = "/Task";
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



