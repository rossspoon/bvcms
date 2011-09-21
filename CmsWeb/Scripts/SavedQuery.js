/// <reference path="../Content/js/jquery-1.5.2-vsdoc.js" />
$(function () {
    $.onready = function () {
        $(".clickEdit").editable("/SavedQuery/Edit/", {
            indicator: "<img src='/images/loading.gif'>",
            tooltip: "Click to edit...",
            style: 'display: inline',
            width: '200px',
            height: 25,
            submit: "OK"
        });
    }
    $.onready();
    $("#onlyMine").live("click", function () {
        $.getTable($(this).closest("form"));
    });
    $(".bt").button();

    $.editable.addInputType("checkbox", {
        element: function (settings, original) {
            var input = $('<input type="checkbox">');
            $(this).append(input);
            $(input).click(function () {
                var value = $(input).attr("checked") ? 'yes' : 'no';
                $(input).val(value);
            });
            return (input);
        },
        content: function (string, settings, original) {
            var checked = string == "yes" ? 1 : 0;
            var input = $(':input:first', this);
            $(input).attr("checked", checked);
            var value = $(input).attr("checked") ? 'yes' : 'no';
            $(input).val(value);
        }
    });
    $('span.yesno').editable('/SavedQuery/Edit/', {
        type: 'checkbox',
        onblur: 'ignore',
        submit: 'OK'
    });
    $("a.delete").live("click", function () {
        var a = $(this);
        $.post("/SavedQuery/Edit", { id: a.attr("id") }, function (ret) {
            a.closest("tr").fadeOut().remove();
        });
    });
});
