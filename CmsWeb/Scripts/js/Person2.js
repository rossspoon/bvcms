$(function () {
    //$("button.trigger-dropdown").dropdown();
    /* accordion */
    $(".accordion h3").click(function () {
        var h = $(".accordion h3.active");
        if (h)
            h.removeClass("active").next("div").slideUp("fast");
        if (!h || h.get(0) != this)
            $(this).addClass("active").next("div").slideDown("fast");
    });
    /* end accordion */
    /* set up modals/dialogs */
    $('a[data-modal-id]').live('click', function (e) {
        e.preventDefault();
        var modalLocation = $(this).attr('data-modal-id');
        $('#' + modalLocation).dialog();
    });
    /* end modal */
    /* edit button for form
        will need more advanced config for form save/cancel */
    $("button.display").click(function (e) {
        e.preventDefault();
        $("fieldset.display").hide();
        $("fieldset.edit").show();
        $(this).hide().siblings(".edit").show();
    }).siblings("button.btn-neutral").click(function (e) {
        e.preventDefault();
        $("fieldset.display").show();
        $("fieldset.edit").hide();
        $(this).hide().siblings(".btn-primary").hide().siblings(".display").show();
    });
    /* end edit button */

    /* local environment js */
    /*                                  */
    if (window.location.hash)
        displayPane(window.location.hash, false);
    else
        displayPane("#personal", false);

    $("#nav-tabs a").click(function (e) { 
        e.preventDefault();
        if (!$(this).parents(".active").size()) {
            $($(this).attr("href")).addClass("loading");
            $(".pane:visible").fadeOut("fast", function () {
                displayPane("#" + $(".pane.loading").removeClass("loading").attr("id"), true);
            });
        }
        return false;
    });
    /* demo sorting styles */
    $("th a").live("click", function (e) {
        e.preventDefault();
        if (!$(this).parents("th.active").size()) {
            var th = $(this).parents("th").eq(0);
            th.addClass("active asc");
            th.siblings("th").removeClass("asc desc active");
        } else {
            if ($(this).parents("th.asc").size()) $(this).parents("th").removeClass("asc").addClass("desc");
            else $(this).parents("th").removeClass("desc").addClass("asc");
        }
    });

});
function displayPane(hash, pushURL) {
    $(hash).fadeIn("fast", function () {
        $("li.active", "#person").removeClass("active");
        $("a[href=#" + $(".pane:visible").attr("id") + "]").parents("li:eq(0)").addClass("active");
        //if (pushURL && window.history && window.history.pushState && window.history.replaceState) {
        //    window.location.replace(hash);
        //    window.history.pushState(null, null, window.location.href);
        //}
    });
}