
        $(document).ready(function () {
            $('#SearchText').keypress(function (e) {
                if ((e.keyCode || e.which) == 13) {
                    e.preventDefault();
                    var s = $('#SearchText').val();
                    if (s != "") {
                        s = s.replace(/^\s+/g, "");
                        s = s.replace(/\s+$/g, " ");
                        var u = '/QuickSearch?name=' + escape(s);
                        window.location = u;
                    }
                }
                return true;
            });
            $('#AddDialog').dialog({
                bgiframe: true,
                autoOpen: false,
                width: 712,
                height: 650,
                modal: true,
                overlay: {
                    opacity: 0.5,
                    background: "black"
                }, close: function () {
                    $('iframe', this).attr("src", "");
                }
            });
            $('#addpeople').click(function (e) {
                e.preventDefault();
                var d = $('#AddDialog');
                $('iframe', d).attr("src", "/SearchAdd?type=addpeople");
                d.dialog("option", "title", "Add People");
                d.dialog("open");
            });
            $('#addorg').click(function (e) {
                e.preventDefault();
                var d = $('#AddDialog');
                $('iframe', d).attr("src", "/AddOrganization");
                d.dialog("option", "title", "Add Organization");
                d.dialog("open");
            });
        });
function CloseAddDialog() {
    $("#AddDialog").dialog("close");
    window.location = "/Person/Current"
}
function CloseAddOrgDialog(id) {
    $("#AddDialog").dialog("close");
    window.location = "/Organization/Index/" + id
}