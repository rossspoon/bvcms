$(document).ready(function () {
    $("#ChooseLabelType").dialog({
        overlay: { background: "#000", opacity: 0.3 },
        bgiframe: true,
        modal: true,
        autoOpen: false,
        closeOnEscape: true,
        width: 400
    });
    $("body").on("click", '.ChooseLabelType', function (ev) {
        ev.preventDefault();
        var d = $("#ChooseLabelType");
        d.dialog("open");

        var sep = ev.target.href.search(/\?/) == -1 ? "?" : "&";
        $("#cmdOK").unbind("click").click(function (ev2) {
            ev2.preventDefault();
            var url = ev.target.href
                + sep + "titles=" + $('#UseTitle')[0].checked
                + "&format=" + $('input[name=addressedto]:checked').val()
                + "&usephone=" + $('#UsePhone')[0].checked
                + "&sortzip=" + $('#SortZip')[0].checked
				+ "&skipNum=" + $('#SkipNum').val();
            $("#ChooseLabelType").dialog("close");
            window.open(url);
            return false;
        });
        return false;
    });
    $('body').on("click", '#TagAll,#UnTagAll', function (ev) {
        ev.preventDefault();
        $.block();
        $.post(this.href, null, function (ret) {
            $(".taguntag:visible").text(ret);
            $.unblock();
        });
        return false;
    });
    $('body').on("click", '#AddContact', function (ev) {
        ev.preventDefault();
        if (!confirm("Are you sure you want to add a contact for all these people?"))
            return false;
        $.block();
        $.post(this.href, null, function (ret) {
            $.unblock();
            if (ret < 0)
                $.growlUI("error", "too many people to add to a contact (max 100)");
            else if (ret == 0)
                $.growlUI("error", "no results");
            else
                window.location = ret;
        });
        return false;
    });
    $('body').on("click", '#AddTasks', function (ev) {
        ev.preventDefault();
        if (!confirm("Are you sure you want to add a task for each of these people?"))
            return false;
        $.block();
        $.post(this.href, null, function (ret) {
            $.unblock();
            if (ret < 0)
                $.growlUI("error", "too many people to add tasks for (max 100)");
            else if (ret == 0)
                $.growlUI("error", "no results");
            else
                window.location = "/Task";
        });
        return false;
    });
    $.QueryString = function (q, item) {
        var r = new Object();
        $.each(q.split('&'), function () {
            var kv = this.split('=');
            r[kv[0]] = kv[1];
        });
        return r[item];
    };
    $.block = function () {
        $.blockUI({ message: 'working on it...<img src="/images/loading.gif"/>' });
    };
    $.unblock = function () {
        $.unblockUI({ fadeOut: 150 });
    };
    $.navigate = function (url, data) {
        url += (url.match(/\?/) ? "&" : "?") + data;
        window.location = url;
    };
    var $di3 = {
        overlay: { background: "#000", opacity: 0.3 },
        bgiframe: true,
        modal: true,
        autoOpen: false,
        closeOnEscape: true
    };
    $("#ExportStartEnd").dialog($di3);
    $("#SetExtraValues").dialog($di3);
    $("body").on("click", '.ChooseStartEnd', function (ev) {
        ev.preventDefault();
        var d = $("#ExportStartEnd");
        d.dialog("open");
        $("#ExportStartEndRun").unbind("click").click(function (ev2) {
            ev2.preventDefault();
            var url = ev.target.href;
            url = url.appendQuery("start=" + $("#startdt", d).val());
            url = url.appendQuery("end=" + $("#enddt", d).val());
            $("#ExportStartEnd").dialog("close");
            window.open(url);
            return false;
        });
        return false;
    });
    $("body").on("click", 'a.ChooseExtraValues', function (ev) {
        ev.preventDefault();
        var d = $("#SetExtraValues");
        d.dialog('option', 'title', $(this).text().trim());
        d.dialog("open");
        $("#SetExtraValuesRun").unbind("click").click(function (ev2) {
            ev2.preventDefault();
            var q = { field: $("#field", d).val(), value: $("#value", d).val() };
            $.block();
            $.post(ev.target.href, q, function (ret) {
                $.unblock();
                alert(ret);
                $("#SetExtraValues").dialog("close");
            });
            return false;
        });
        return false;
    });
    $.DateValid = function(d, growl) {
        var reDate = /^(0?[1-9]|1[012])[\/-](0?[1-9]|[12][0-9]|3[01])[\/-]((19|20)?[0-9]{2})$/i;
        if ($.dateFormat.startsWith('d'))
            reDate = /^(0?[1-9]|[12][0-9]|3[01])[\/-](0?[1-9]|1[012])[\/-]((19|20)?[0-9]{2})$/i;
        var v = true;
        if (!reDate.test(d)) {
            if (growl == true)
                $.growlUI("error", "enter valid date");
            v = false;
        }
        return v;
    };
});
String.prototype.startsWith = function(t, i) {
    return (t == this.substring(0, t.length));
};
String.prototype.appendQuery = function(q) {
    if (this && this.length > 0)
        if (this.contains("&") || this.contains("?"))
            return this + '&' + q;
        else
            return this + '?' + q;
    return q;
};
String.prototype.contains = function(it) { 
    return this.indexOf(it) != -1; 
};
String.prototype.endsWith = function(t, i) {
    return (t == this.substring(this.length - t.length));
};
String.prototype.addCommas = function() {
    var x = this.split('.');
    var x1 = x[0];
    var x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
};



