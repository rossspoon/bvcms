$(document).ready(function() {
    $("ul.sf-tab").superfish({
        autoArrows: false
    });
    $("#ChooseLabelType").dialog({
        overlay: { background: "#000", opacity: 0.3 },
        bgiframe: true,
        modal: true,
        autoOpen: false,
        closeOnEscape: true,
        width: 400
    });
    $(".ChooseLabelType").click(function(ev) {
        $("#ChooseLabelType").dialog("open");
        $("#cmdOK").click(function() {
            var url = ev.target.href
                + "&titles=" + $('#UseTitle')[0].checked
                + "&format=" + $('input[name=addressedto]:checked').val()
                + "&web=" + $('#WebView')[0].checked;
            $("#ChooseLabelType").dialog("close");
            window.open(url, "_blank");
        });
        return false;
    });
});


