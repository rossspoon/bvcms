$(function () {
    $(".bt").button();
    $(".datepicker").datepicker();
    $.fmtTable = function () {
        $('table.grid > tbody > tr:even').addClass('alt');
    };
    $.fmtTable();
    $("#search").click(function (ev) {
        ev.preventDefault();
        $.getTable();
        return false;
    });
    $.gotoPage = function (ev, pg) {
        $("#Page").val(pg);
        $.getTable();
        return false;
    };
    $.setPageSize = function (ev) {
        $('#Page').val(1);
        $("#PageSize").val($(ev).val());
        return $.getTable();
    };
    $.getTable = function () {
        var f = $('#results').closest('form');
        var q = f.serialize();
        $.post('/ContactSearch/Results', q, function (ret) {
            $('#results').replaceWith(ret).ready($.fmtTable);
        });
    };
    $("#ConvertToQuery").click(function (ev) {
        ev.preventDefault();
        var f = $('#results').closest('form');
        var q = f.serialize();
        $.post($(this).attr('href'), q, function (ret) {
            window.location = ret;
        });
        return false;
    });
    $("#ContactorSummary").click(function (ev) {
        ev.preventDefault();
        if ($("#StartDate").val() && $("#EndDate").val() && $("#form").valid()) {
            window.location = $(this).attr('href')
                + "?start=" + $("#StartDate").val()
                + "&end=" + $("#EndDate").val()
                + "&ministry=" + $("#Ministry").val();
        } else
            alert("need valid dates");

        return false;
    });
    $("#ContactTypeTotals").click(function (ev) {
        ev.preventDefault();
        var loc = window.location = $(this).attr('href') + "?ministry=" + $("#Ministry").val();
        if ($("#StartDate").val())
            loc = loc.appendQuery("start=" + $("#StartDate").val());
        if ($("#EndDate").val())
            loc = loc.appendQuery("end=" + $("#EndDate").val());
        hideDropdowns();
        $.block();
        window.location = loc;
        return false;
    });
    $("#NewSearch").click(function () {
        form.reset();
    });
    $.validator.addMethod("date2", function (value, element, params) {
        var v = $.DateValid(value);
        return this.optional(element) || v;
    }, $.format("Please enter valid date"));
    
    $.validator.setDefaults({
        highlight: function (input) {
            $(input).addClass("ui-state-highlight");
        },
        unhighlight: function (input) {
            $(input).removeClass("ui-state-highlight");
        },
        rules: {
            "StartDate": { date2: true },
            "EndDate": { date2: true }
        }
    });
    $('#form').validate();
});