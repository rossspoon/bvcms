var mustSearch = 1;

$(function () {
    $(document).on("click", "#supportfeedback", null, supportOpen);
    $(document).on("keyup", "#gsc-i-id1", null, scheduleRefresh);
    $(document).on("click", ".gsc-search-button", null, searchDone);
    $(document).on("click", "#sendSupport", null, supportSend);
    $(document).on("click", "#cancelSupport", null, supportClose);
    $(document).on("click", "[supportHelp]", null, supportHelpOpen);
    $(document).on("click", "[supportCancelHelp]", null, supportHelpClose);

    $("#supportDialog").dialog({ autoOpen: false, resizable: false, width: 800, height: 500, modal: true, dialogClass: "no-title" });
    $("#supportHelp").dialog({ autoOpen: false, resizable: false, width: "auto", height: "auto", modal: true, dialogClass: "no-title" });
});

function scheduleRefresh(e) {
    if (e.keyCode == 13)
        searchDone(e);
}

function searchDone(e) {
    mustSearch--;
    if (mustSearch <= 0) {
        $("#reqBody").removeAttr("disabled");
        $("#sendSupport").removeAttr("disabled");
        $("#reqSelect").removeAttr("disabled");
    }

}

function supportOpen(e) {
    mustSearch = 1;

    $("#reqBody").attr("disabled", "disabled").val("");
    $("#sendSupport").attr("disabled", "disabled");
    $("#reqSelect").attr("disabled", "disabled");

    $("#supportDialog").dialog("open");
}

function supportClose(e) {
    $("#supportDialog").dialog("close");
}

function supportSend(e) {
    var theURL = "/Home/SendSupportRequest";
    var thePost = $(this).closest("form").serialize();
    $.ajax(theURL, { data: thePost, type: "POST", success: supportSentComplete });
}

function supportSentComplete(data, status, xhr) {
    $("#supportDialog").dialog("close");
}

function supportHelpOpen(e) {
    var contentID = $(this).attr("contentID");
    $("#supportHelp").html($("#" + contentID).html());
    $("#supportHelp").dialog("open");
}

function supportHelpClose(e) {
    $("#supportHelp").dialog("close");
}