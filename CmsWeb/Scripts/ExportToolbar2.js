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
        $("#cmdOK").click(function() {
            var url = ev.target.href
                + "&titles=" + $('#UseTitle').val()
                + "&format=" + $('input[name=addressedto]:checked').val();
            $("#ChooseLabelType").dialog("close");
            window.open(url, "_blank");
        });
        return false;
    });
}); 
