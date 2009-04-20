$(function() {
    $('a.orgchooser').ChooseOrg();
    $('#VbsApps a.sortable').click(function() {
        var newsort = $(this).text();
        var oldsort = $("#Sort").val();
        $("#Sort").val(newsort);
        var dir = $("#Dir").val();
        if (oldsort == newsort && dir == '')
            $("#Dir").val('desc');
        else
            $("#Dir").val('');
        RefreshPage();
    });
    $("#UserInfo").change(RefreshPage);
    $("#Grade").change(RefreshPage);
    $("#NewAppsOnly").click(RefreshPage);
});
function RefreshPage() {
    var q = $('#filter').formSerialize2();
    $.navigate("/VBS", q);
}
(function($) {
    var o = {};
    $.fn.ChooseOrg = function() {
        $('#OrgChooser').dialog({
            overlay: { background: "#000", opacity: 0.3 },
            bgiframe: true,
            modal: true,
            autoOpen: false,
            closeOnEscape: true,
            width: 366,
            height: 138
        });
        this.click(function(ev) {
            o.Id = this.id;
            PopDivs(ev);
            $('#OrgChooser').data('title.dialog', $("a#n" + o.Id).text())
                .dialog("open");
            return false;
        });
        $("#Divs").change(PopDivs);
        $('#selectorg').click(function() {
            var data = { DivId: $("#Divs").val(), OrgId: $("#Orgs").val() };
            $.post('/VBS/SelectOrg/' + o.Id, data, function(ret) {
                $("a#" + o.Id).text(ret.OrgName)
                    .attr("divid", data.DivId)
                    .attr("orgid", data.OrgId);
                $('#OrgChooser').dialog("close");
            }, "json");
        });
        return this;
    };
    var PopDivs = function(ev) {
        var divid = $("#Divs").val();
        var data = {};
        if ($(ev.target).attr("divid"))
            data = { DivId: $(ev.target).attr("divid"), OrgId: $(ev.target).attr('orgid') }        
        $.getJSON('/VBS/OrgOptions/' + divid, data || {}, function(j) {
            if (data)
                $('#Divs').val(data.DivId);
            var options = '';
            for (var i = 0; i < j.length; i++) {
                options += '<option value="' + j[i].Value + '"';
                if (j[i].Selected)
                    options += ' selected=\'true\''
                options += '>' + j[i].Text + '</option>';
            }
            $('#Orgs').html(options);
        });
    };
})(jQuery);