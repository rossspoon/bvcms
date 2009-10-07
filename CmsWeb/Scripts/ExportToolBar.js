$(document).ready(function() {
    $(document).ready(function() {
        $("ul.sf-menu").supersubs({
            minWidth: 8,
            maxWidth: 27,
            extraWidth: 1,
            autoArrows: false
        }).superfish();
        $("ul.sf-tab").superfish();
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

function EnterSubmit(e, myform) {
    var key = window.event ? e.keyCode : e.which;
    if (key == 13) {
        var s = $get('<%=SearchText.ClientID%>');
        if (s.value != "Quick Search")
            window.location = '<%=ResolveUrl("~/Search.aspx?name=")%>' + s.value;
    }
    else
        return true;
}
function TBNewWindow(url)
{
    window.open(url, "_blank");
}
function TBNavWindow(url)
{
    window.open(url, "_self");
}

