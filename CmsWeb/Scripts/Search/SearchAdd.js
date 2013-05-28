$(function () {
    $("a.searchadd").live("click", function (ev) {
        ev.preventDefault();
        $("<div id='search-add' class='modal fade hide' data-width='600' />")
            .load($(this).attr("href"), {}, function () {
                $(this).modal("show");
                $(this).on('hidden', function () {
                    $(this).remove();
                });
            });
    });
    $("#search-add a.clear").live('click', function (ev) {
        ev.preventDefault();
        $("#name").val('');
        $("#phone").val('');
        $("#address").val('');
        $("#dob").val('');
        return false;
    });
    $("div.modal form.ajax").live("submit", function (event) {
        var $form = $(this);
        var $target = $form.closest("div.modal");
        $.ajax({
            type: 'POST',
            url: $form.attr('action'),
            data: $form.serialize(),
            success: function (data, status) {
                //$target.removeClass("fade");
                $target.html(data);
                var top = ($(window).height() - $target.height()) / 2;
                if (top < 10)
                    top = 10;
                $target.css({ 'margin-top': top, 'top': '0' });
            }
        });
        event.preventDefault();
    });
    $("form.ajax a.ajax").live("click", function (event) {
        var $this = $(this);
        var $form = $this.closest("form.ajax");
        var $modal = $form.closest("div.modal");
        var url = $this[0].href;
        var data = $form.serialize();
        $.ajax({
            type: 'POST',
            url: url,
            data: data,
            success: function(data, status) {
                if ($modal.length > 0) {
                    //$modal.removeClass("fade");
                    $modal.html(data);
                    var top = ($(window).height() - $modal.height()) / 2;
                    if (top < 10)
                        top = 10;
                    $modal.css({ 'margin-top': top, 'top': '0' });
                } else {
                    $form.html(data);
                }
            },
            error: function(xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
        event.preventDefault();
    });

    $("form.ajax tbody > tr a.reveal").live("click", function (e) {
        e.stopPropagation();
    });
    $.NotReveal = function(ev) {
        if ($(ev.target).is("a"))
            if (!$(ev.target).is('.reveal'))
                return;
    };
    $("form.ajax tr.section.notshown").live("click", function (ev) {
        if ($.NotReveal(ev)) return;
        ev.preventDefault();
        ev.stopPropagation();
        $(this).removeClass("notshown").addClass("shown");
        $(this).nextUntil("tr.section").find("div.collapse")
            .off("hidden")
            .on("hidden", function(e) { e.stopPropagation(); })
            .collapse('show');
    });
    $("form.ajax tr.section.shown").live("click", function (ev) {
        if ($.NotReveal(ev)) return;
        ev.preventDefault();
        $(this).nextUntil("tr.section").find("div.collapse")
            .off("hidden")
            .on("hidden", function(e) { e.stopPropagation(); })
            .collapse('hide');
        $(this).removeClass("shown").addClass("notshown");
    });
    $('form.ajax a[rel="reveal"]').live("click", function (ev) {
        ev.preventDefault();
        $(this).parents("tr").next("tr").find("div.collapse")
            .off('hidden')
            .on("hidden", function(e) { e.stopPropagation(); })
            .collapse("toggle");
    });
    $("form.ajax tr.master").live("click", function (ev) {
        if ($.NotReveal(ev)) return;
        ev.preventDefault();
        $(this).next("tr").find("div.collapse")
            .off('hidden')
            .on("hidden", function(e) { e.stopPropagation(); })
            .collapse("toggle");
    });
    $("form.ajax tr.details").live("click", function (ev) {
        if ($.NotReveal(ev)) return;
        ev.preventDefault();
        ev.stopPropagation();
        $(this).find("div.collapse")
            .off("hidden")
            .on("hidden", function(e) { e.stopPropagation(); })
            .collapse('hide');
    });
});