$(document).ready(function () {
	// override the getTable function in pager.js
	$.getTable = function () {
		var f = $("#form").submit();
		return false;
	};
	$('#filter').click(function (ev) {
		ev.preventDefault();
		$.getTable();
	});
	$("a[person]").live("click", function (ev) {
		$("#peopleid").val($(this).attr("id"));
		ev.preventDefault();
		$.getTable();
	});
	$("#clearname").click(function (ev) {
		ev.preventDefault();
		$("#peopleid").val(0);
		$.getTable();
	});
	$("#clear").click(function (ev) {
		ev.preventDefault();
		$("input:text").val("");
		$('input:checkbox').removeAttr('checked');
		$('#peopleid').val(0);
		$("#activity").val(0);
		$.getTable();
	});
	$.formatTable = function () {
		$('table.grid > tbody > tr:even').addClass('alt');
	};
	$.formatTable();
	$(".bt").button();
});
