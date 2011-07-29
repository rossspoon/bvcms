$(function () {
    $(".bt").button();
    $("#searchvalues select").css("width", "100%");
    $("#clear").click(function (ev) {
        ev.preventDefault();
        $("input:text").val("");
        $("#memberstatus,#campus").val(0); //.sb("refresh");
        $("#gender,#marital").val(99); //.sb("refresh");
        return false;
    });
    $('#Name').focus();
    $("#search").click(function (ev) {
        ev.preventDefault();
        $.getTable();
        return false;
    });
    $("a.bt").bind("contextmenu", function (e) {
        e.preventDefault();
    });
    $("#targetpeople").click(function (ev) {
        ev.preventDefault();
        if ($('a.target[target="people"]').length == 0)
            $("a.target").attr("target", "people");
        else
            $("a.target").removeAttr("target");
        return false;
    });
    $("#convert").click(function (ev) {
        ev.preventDefault();
        var f = $('#results').closest('form');
        var q = f.serialize();
        $.post($('#convert').attr('href'), q, function (ret) {
            window.location = ret;
        });
        return false;
    });
    $.gotoPage = function (e, pg) {
        $("#Page").val(pg);
        $.getTable();
        return false;
    }
    $.setPageSize = function (e) {
        $('#Page').val(1);
        $("#PageSize").val($(e).val());
        return $.getTable();
    }
    $.getTable = function () {
        var f = $('#results').closest('form');
        var q = f.serialize();
        $.blockUI();
        $.post($('#search').attr('href'), q, function (ret) {
            $('#results').replaceWith(ret).ready(function () {
                $('#results > tbody > tr:even').addClass('alt');
                $("#totalcount").text($("#totcnt").val());
                $.unblockUI();
            });
        });
        return false;
    }
    $('#results > tbody > tr:even').addClass('alt');
    $('#results > thead a.sortable').live('click', function (ev) {
        ev.preventDefault();
        var newsort = $(this).text();
        var sort = $("#Sort");
        var dir = $("#Direction");
        if ($(sort).val() == newsort && $(dir).val() == 'asc')
            $(dir).val('desc');
        else
            $(dir).val('asc');
        $(sort).val(newsort);
        $.getTable();
        return false;
    });
    $("form input").live("keypress", function (e) {
        if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
            $('#search').click();
            return false;
        }
        return true;
    });
    $('a.taguntag').live('click', function (ev) {
        ev.preventDefault();
        var a = $(this);
        $.post('/Tags/ToggleTag/' + $(this).attr('value'), null, function (ret) {
            $(ev.target).text(ret);
        });
        return false;
    });
});


