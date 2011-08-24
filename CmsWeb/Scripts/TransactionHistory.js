$(function () {
    $("table.grid > tbody > tr:even").addClass("alt");
    $("a.deltran").live("click", function (ev) {
        ev.preventDefault();
        if (confirm("are you sure"))
            $.post($(this).attr("href"), {}, function (ret) {
                $("#history").replaceWith(ret);
                $("#history > tbody > tr:even").addClass("alt");
            });
        return false;
    });
//    $.editable.addInputType('datepicker', {
//        element: function (settings, original) {
//            var input = $('<input>');
//            if (settings.width != 'none') { input.width(settings.width); }
//            if (settings.height != 'none') { input.height(settings.height); }
//            input.attr('autocomplete', 'off');
//            $(this).append(input);
//            return (input);
//        },
//        plugin: function (settings, original) {
//            var form = this;
//            settings.onblur = 'ignore';
//            $(this).find('input').datepicker().bind('click', function () {
//                $(this).datepicker('show');
//                return false;
//            }).bind('dateSelected', function (e, selectedDate, $td) {
//                $(form).submit();
//            });
//        }
//    });
    $("span.date").editable('/TransactionHistory/Edit/', {
        tooltip: 'click to edit...',
        event: 'click',
        submit: 'OK',
        cancel: 'Cancel',
        width: '100px',
        height: 25
    });
});
