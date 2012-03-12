$(function () {
    $(".datepicker").datepicker({
        dateFormat: 'm/d/yy',
        changeMonth: true,
        changeYear: true
    });
    $("#run").click(function (ev) {
        ev.preventDefault();
        var f = $(this).closest('form');
        var q = f.serialize();
        $.post("/FinanceReports/TotalsByFundResults", q, function (ret) {
            $("#results").html(ret).ready(function () {
                $('table.grid tbody tr:even').addClass('alt');
            });
        });
    });
    $("#export").click(function (ev) {
        ev.preventDefault();
        $.blockUI({
            theme: true,
            title: 'Producing Contributions Export',
            message: '<p>Click the page to continue after your download appears.</p>'
        });
        var f = $(this).closest('form');
        var q = f.serialize();
        window.location = "/Export/Contributions?totals=false&" + q; 
        $('.blockOverlay').attr('title', 'Click to unblock').click($.unblockUI);
    });
    $("#exporttotals").click(function (ev) {
        ev.preventDefault();
        $.blockUI({
            theme: true,
            title: 'Producing Contribution Totals Export',
            message: '<p>Click the page to continue after your download appears.</p>'
        });
        var f = $(this).closest('form');
        var q = f.serialize();
        window.location = "/Export/Contributions?totals=true&" + q;
        $('.blockOverlay').attr('title', 'Click to unblock').click($.unblockUI);
    });
    $(".bt").button();
});
