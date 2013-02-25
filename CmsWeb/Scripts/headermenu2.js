$(function () {
    $("a.trigger-dropdown").dropdown();
    $('#SearchText').keypress(function (e) {
		if ((e.keyCode || e.which) == 13) {
			e.preventDefault();
			var s = $('#SearchText').val();
			if (s != "") {
				s = s.replace(/^\s+/g, "");
				s = s.replace(/\s+$/g, " ");
				var u = '/QuickSearch/Index?q=' + escape(s);
				window.location = u;
			}
		}
		return true;
    });
	$("a.tutorial").click(function (ev) {
		ev.preventDefault();
		startTutorial($(this).attr("href"));
	});
//	$('#AddDialog').dialog({
//		bgiframe: true,
//		autoOpen: false,
//		width: 750,
//		height: 700,
//		modal: true,
//		overlay: {
//			opacity: 0.5,
//			background: "black"
//		}, close: function () {
//			$('iframe', this).attr("src", "");
//		}
//	});
//	$('#addpeople').click(function (e) {
    //		e.preventDefault();
    //		var d = $('#AddDialog');
//		$('iframe', d).attr("src", "/SearchAdd?type=addpeople");
//		d.dialog("option", "title", "Add People");
//		d.dialog("open");
//	});
//	$('#addorg').click(function (e) {
//		e.preventDefault();
//		var d = $('#AddDialog');
//		$('iframe', d).attr("src", "/AddOrganization");
//		d.dialog("option", "title", "Add Organization");
//		d.dialog("open");
//	});
	$('#cleartag').click(function (e) {
		e.preventDefault();
		if (confirm("are you sure you want to empty the active tag?"))
    		$.post("/Tags/ClearTag", {}, function () {
    			window.location.reload();
    		});
	});
	$('.warntip').tooltip({
		delay: 150,
		showBody: "|",
		showURL: false
	});
	$('#SearchText').each(function () {
	    $(this).tooltip({
	        showBody: "|"
	    });
	    //$(this).attr("value", $(this).attr('default'));
	    $(this).addClass('text-label');
	    $(this).focus(function () {
	        if (this.value == $(this).attr('default')) {
	            this.value = '';
	            $(this).removeClass('text-label');
	        }
	    });
	    $(this).blur(function () {
	        if (this.value == '' && $(this).attr('default')) {
	            this.value = $(this).attr('default');
	            $(this).addClass('text-label');
	        }
	    });
	});
    $.QueryString = function (q, item) {
        var r = new Object();
        $.each(q.split('&'), function () {
            var kv = this.split('=');
            r[kv[0]] = kv[1];
        });
        return r[item];
    };
    $.block = function (message) {
        if (!message)
            message = '<h1>working on it...</h1>';
        $.blockUI({
                message: message,
                overlayCSS: { opacity: 0 },
                css: {
                    border: 'none',
                    padding: '15px',
                    backgroundColor: '#000',
                    '-webkit-border-radius': '10px',
                    '-moz-border-radius': '10px',
                    opacity: .5,
                    color: '#fff'
                }
            });
        };
    $.unblock = function () {
        $.unblockUI();
    };
    $.navigate = function (url, data) {
        url += (url.match(/\?/) ? "&" : "?") + data;
        window.location = url;
    };
    $.DateValid = function(d, growl) {
        var reDate = /^(0?[1-9]|1[012])[\/-](0?[1-9]|[12][0-9]|3[01])[\/-]((19|20)?[0-9]{2})$/i;
        if($.dtoptions.format.startsWith('d'))
            reDate = /^(0?[1-9]|[12][0-9]|3[01])[\/-](0?[1-9]|1[012])[\/-]((19|20)?[0-9]{2})$/i;
        var v = true;
        if (!reDate.test(d)) {
            if (growl == true)
                $.growlUI("error", "enter valid date");
            v = false;
        }
        return v;
    };
    jQuery.fn.center = function(parent) {
        if (parent) {
            parent = this.parent();
        } else {
            parent = window;
        }
        this.css({
            "position": "absolute",
            "top": ((($(parent).height() - this.outerHeight()) / 2) + $(parent).scrollTop() + "px"),
            "left": ((($(parent).width() - this.outerWidth()) / 2) + $(parent).scrollLeft() + "px")
        });
        return this;
    };
});

function dimOff() {
    $("#darkLayer").hide();
}
function dimOn()
{
    $("#darkLayer").show();
}
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
