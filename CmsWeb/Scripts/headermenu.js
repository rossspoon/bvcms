$(document).ready(function () {
    $("a.trigger-dropdown").dropdown();

    if ($.oldQuickSearch) {
        $('#SearchText').keypress(function (e) {
            if ((e.keyCode || e.which) === 13) {
                e.preventDefault();
                var s = $('#SearchText').val();
                if (s !== "") {
                    s = s.replace(/^\s+/g, "");
                    s = s.replace(/\s+$/g, " ");
                    var u = '/QuickSearch/Index?q=' + escape(s);
                    window.location = u;
                }
            }
            return true;
        });
    }
    else {
        $('#SearchText').each(function () {
            $(this).autocomplete({
                appendTo: "#SearchResults",
                position: { my: "right top", at: "right bottom", of: $("#SearchText") },
                autoFocus: true,
                minLength: 3,
                source: function (request, response) {
                    if (request.term === '---')
                        response([
                            { id: -1, line1: "People Search" },
                            { id: -2, line1: "Advanced Search" },
                            { id: -3, line1: "Organization Search" }
                        ]);
                    else
                        $.post("/Home/Names", request, function (ret) {
                            response(ret.slice(0, 15));
                        }, "json");
                },
                select: function (event, ui) {
                    if (ui.item.id === -1)
                        window.location = "/PeopleSearch";
                    else if (ui.item.id === -2)
                        window.location = "/QueryBuilder/Main";
                    else if (ui.item.id === -3)
                        window.location = "/OrgSearch";
                    else
                        window.location = (ui.item.isOrg ? "/Organization/Index/" : "/Person/Index/") + ui.item.id;
                }
            }).data("uiAutocomplete")._renderItem = function (ul, item) {
                if (item.id === 0)
                    return $("<li>").append("<hr/>").appendTo(ul);
                var li = "<a><b>" + (item.isOrg ? "Org: " : "") + item.line1 + "</b>";
                if (item.id > 0)
                    li += "<br>" + (item.isOrg ? "Div: " : "") + item.line2;
                li += "</a>";
                return $("<li>")
                    .append(li)
                    .appendTo(ul);
            };
        });
    }

    $("a.tutorial").click(function (ev) {
        ev.preventDefault();
        startTutorial($(this).attr("href"));
    });
    $('#AddDialog').dialog({
        bgiframe: true,
        autoOpen: false,
        width: 750,
        height: 700,
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
    $('#cleartag').click(function (e) {
        e.preventDefault();
        if (confirm("are you sure you want to empty the active tag?"))
            $.post("/Tags/ClearTag", {}, function () {
                window.location.reload();
            });
    });
    $('.warntip').tooltip({
        delay: 150,
        showBody: "|",
        showURL: false
    });
    $('#SearchText').each(function () {
        $(this).addClass('text-label');
        $(this).focus(function () {
            if (this.value === '' || this.value === $(this).attr('default')) {
                this.value = '';
                $(this).removeClass('text-label');
                if(!$.oldQuickSearch)
                    $(this).autocomplete("search", "---");
            }
        });
        $(this).blur(function () {
            if (this.value === '' && $(this).attr('default')) {
                this.value = $(this).attr('default');
                $(this).addClass('text-label');
            }
        });
    });
});
function CloseAddDialog() {
    $("#AddDialog").dialog("close");
    window.location = "/Person/Current";
}
function CloseAddOrgDialog(id) {
    $("#AddDialog").dialog("close");
    window.location = "/Organization/Index/" + id;
}