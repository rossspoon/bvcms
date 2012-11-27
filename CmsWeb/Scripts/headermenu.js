
$(document).ready(function () {
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
	$('#AddDialog').dialog({
		bgiframe: true,
		autoOpen: false,
		width: 750,
		height: 700,
		modal: true,
		overlay: {
			opacity: 0.5,
			background: "black"
		}, close: function () {
			$('iframe', this).attr("src", "");
		}
	});
	$('#addpeople').click(function (e) {
		e.preventDefault();
		var d = $('#AddDialog');
		$('iframe', d).attr("src", "/SearchAdd?type=addpeople");
		d.dialog("option", "title", "Add People");
		d.dialog("open");
		d.resize();
	});
	$('#addorg').click(function (e) {
		e.preventDefault();
		var d = $('#AddDialog');
		$('iframe', d).attr("src", "/AddOrganization");
		d.dialog("option", "title", "Add Organization");
		d.dialog("open");
	});
	$('#cleartag').click(function (e) {
		e.preventDefault();
		//if (alert("are you sure?"))
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
	    $(this).attr("value", $(this).attr('default'));
	    $(this).addClass('text-label');
	    $(this).focus(function () {
	        if (this.value == $(this).attr('default')) {
	            this.value = '';
	            $(this).removeClass('text-label');
	        }
	    });
	    $(this).blur(function () {
	        if (this.value == '') {
	            this.value = $(this).attr('default');
	            $(this).addClass('text-label');
	        }
	    });
	});
});
function CloseAddDialog() {
    $("#AddDialog").dialog("close");
    window.location = "/Person/Current";
}
function CloseAddOrgDialog(id) {
    $("#AddDialog").dialog("close");
    window.location = "/Organization/Index/" + id;
}