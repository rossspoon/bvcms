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
        width: 300
    });
    $(".ChooseLabelType").click(function(ev) {
        $("#ChooseLabelType").dialog("open");
        var sep = ev.target.href.search(/\?/) == -1 ? "?" : "&";
        $("#cmdOK").click(function() {
            var url = ev.target.href
                + sep + "titles=" + $('#UseTitle').val()
                + "&format=" + $('input[name=addressedto]:checked').val()
                + "&web=" + $('#WebView').val();
            $("#ChooseLabelType").dialog("close");
            window.open(url, "_blank");
        });
        return false;
    });
}); 
