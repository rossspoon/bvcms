$(function () {
    $(".datepicker").datepicker();
	$("#run").click(function (ev) {
	    ev.preventDefault();
	    if (!$.DateValid($("#Dt1").val(), true))
	        return;
	    if (!$.DateValid($("#Dt2").val(), true))
	        return;
	    var f = $(this).closest('form');
		var q = f.serialize();
		$.post("/FinanceReports/DonorTotalsByRangeResults", q, function (ret) {
			$("#results").html(ret).ready(function () {
				$('table.grid tbody tr:even').addClass('alt');
			});
		});
	});
	$(".bt").button();
});
