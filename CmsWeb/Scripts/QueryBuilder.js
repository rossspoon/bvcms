var qs = "";
$(function() {
    $("#tabber > ul").tabs();
    $("#SelectCondition").SelectCondition();
    $('#Tags').click(function(ev) {
        $('#TagsPopup').show();
    });
    $(".datepicker").datepicker({ dateFormat: 'm/d/yy' });
    HighlightCondition();
    $('#Program').change(function(ev) {
        $.post('/QueryBuilder/GetDivisions/' + $(this).val(), null, function(ret) {
            $('#Division').fillOptions(ret.Divisions);
            $('#Organization').fillOptions(ret.Organizations);
            CascadeDivision();
        }, "json");
    });
    $('#AddToGroup').click(function() {
        qs = $('#conditionForm').serialize();
        $.post('/QueryBuilder/AddToGroup/', qs, function(ret) {
            var a = ret.split("<---------->");
            if (!ShowErrors(a[1]))
                FillConditionGrid(a[0]);
        });
        return false;
    });
    $('#Add').click(function() {
        qs = $('#conditionForm').serialize();
        $.post('/QueryBuilder/Add/', qs, function(ret) {
            var a = ret.split("<---------->");
            if (!ShowErrors(a[1]))
                FillConditionGrid(a[0]);
        });
        return false;
    });
    $('#Update').click(function() {
        qs = $('#conditionForm').serialize();
        $.post('/QueryBuilder/Update/', qs, function(ret) {
            var a = ret.split("<---------->");
            if (!ShowErrors(a[1]))
                FillConditionGrid(a[0]);
        });
        return false;
    });
    $('#Remove').click(function() {
        qs = $('#conditionForm').serialize();
        $.post('/QueryBuilder/Remove/', qs, function(ret) {
            UpdateView(ret);
            $.post('/QueryBuilder/Conditions/', null, function(ret) {
                FillConditionGrid(ret);
            });
        }, "json");
        return false;
    });
    $('#Run').click(function(ev) {
        qs = $('#conditionForm').serialize();
        RefreshList();
        return false;
    });
    dialogOptions = {
        overlay: { background: "#000", opacity: 0.3 },
        bgiframe: true,
        modal: true,
        autoOpen: false,
        closeOnEscape: true,
        width: 500
    };
    $('#SaveQueryDiv').dialog(dialogOptions);
    $('#OpenQueryDiv').dialog(dialogOptions);

    $('#ShowSaveQuery').click(function(ev) {
        $('#SaveQueryDiv').dialog("open");
    });
    $('#ShowOpenQuery').click(function(ev) {
        $.post("/QueryBuilder/SavedQueries", null, function(ret) {
            $('#ExistingQueries').fillOptions(ret);
        }, "json");
        $('#OpenQueryDiv').dialog("open");
        return false;
    });
    $('#OpenQuery').click(function(ev) {
        $('#OpenQueryDiv').dialog("close");
        window.location = "/QueryBuilder/Main/" + $("#ExistingQueries").val();
    });
    $('#SaveQuery').click(function(ev) {
        $('#OpenQueryDiv').dialog("close");
        $.post("/QueryBuilder/SaveQuery/", {
            SavedQueryDesc: $('#SavedQueryDesc').val(),
            IsPublic: $('#IsPublic').val()
        }, function(ret) {
            $("#Description").text(ret);
        });
        return false;
    });
    $('#TagAll').click(function(ev) {
        $.block();
        $.post("/QueryBuilder/TagAll/", null, function(ret) {
            $(".taguntag").text(ret);
            $.unblock();
        });
        return false;
    });
    $('#UnTagAll').click(function(ev) {
        $.block();
        $.post("/QueryBuilder/UnTagAll/", null, function(ret) {
            $(".taguntag").text(ret);
            $.unblock();
        });
        return false;
    });
});
function HighlightCondition() {
    $('#ConditionGrid li a').removeClass('SelectedRow');
    $('#ConditionGrid li a#' + $('#SelectedId').val()).addClass('SelectedRow');
    $('#ConditionGrid li a').click(EditCondition);
    $(".conditionPopup").contextMenu({ menu: 'InsCopyMenu' }, function(action, el, pos) {
	    switch (action) {
	        case "ins":
	            $.post("/QueryBuilder/InsGroupAbove/" + $(el).attr('id'), null, function(ret) {
	                $.navigate("/QueryBuilder/Main/" + ret);
	            });
	            break;
	        case "copy":
	            $.post("/QueryBuilder/CopyAsNew/" + $(el).attr('id'), null, function(ret) {
	                $.navigate("/QueryBuilder/Main/" + ret);
	            });
	            break;
	    }
	});
}
function FillConditionGrid(html) {
    $('#ConditionGrid').html(html);
    HighlightCondition();
    $('#ConditionGrid li a').click(EditCondition);
}
(function($) {
    $.QueryString = function(q, item) {
        var r = new Object();
        $.each(q.split('&'), function() {
            var kv = this.split('=');
            r[kv[0]] = kv[1];
        });
        return r[item];
    };
    $.block = function() {
        $.blockUI({ message: 'working on it...<img src="/content/loading.gif"/>' });
    };
    $.unblock = function() {
        $.unblockUI({ fadeOut: 150 });
    };
    $.navigate = function(url, data) {
        url += (url.match(/\?/) ? "&" : "?") + data;
        window.location = url;
    };
    $.fn.showhide = function(bool) {
        if (bool)
            $(this).show();
        else
            $(this).hide();
        return this;
    };
    $.fn.enabled = function(bool) {
        if (bool)
            $(this).removeAttr("disabled");
        else
            $(this).attr("disabled", "disabled");
        return this;
    };
    $.fn.fillOptions = function(a, multiple) {
        var options = '';
        if (a)
            for (var i = 0; i < a.length; i++) {
            options += '<option value="' + a[i].Value + '"';
            if (a[i].Selected)
                options += ' selected=\'selected\''
            options += '>' + a[i].Text + '</option>';
        }
        return this.each(function() {
            var s = "<select id='" + this.id + "' name='" + this.id + "'";
            if (multiple)
                s += " multiple='multiple'";
            s += ">" + options + "</select>";
            $(this).replaceWith(s);
        });
    };
    $.fn.multiSelectRemove = function() {
        $(this).each(function() {
            $(this).next('.multiSelect').remove();
            $(this).next('.multiSelectOptions').remove();
        });
        return $(this);
    };
})(jQuery);

function RefreshList() {
    $.block()
    $.post('/QueryBuilder/Results/', qs, function(ret) {
        $('#toolbar').show();
        $('#Results').html(ret);
        $('#people tbody tr:even').addClass('alt');
        $('a.taguntag').click(function(ev) {
            $.post('/QueryBuilder/ToggleTag/' + $(this).attr('value'), null, function(ret) {
                $(ev.target).text(ret.HasTag ? "Remove" : "Add");
            }, "json");
            return false;
        });
        $('#people thead a.sortable').click(function(ev) {
            var newsort = $(this).text();
            var oldsort = $("#Sort").val();
            $("#Sort").val(newsort);
            var dir = $("#Direction").val();
            if (oldsort == newsort && dir == 'asc')
                $("#Direction").val('desc');
            else
                $("#Direction").val('asc');
            qs = $('#conditionForm').serialize();
            RefreshList();
            return false;
        });
        $.unblock();
    });
}
function GotoPage(pg) {
    qs = $('#conditionForm').serialize() + "&Page=" + pg;
    RefreshList();
    return false;
}
function SetPageSize(sz) {
    qs = $('#conditionForm').serialize() + "&PageSize=" + sz;
    RefreshList();
    return false;
}
function CascadeDivision() {
    $('#Division').change(function(ev) {
        $.post('/QueryBuilder/GetOrganizations/' + $(this).val(), null, function(ret) {
            $('#Organization').fillOptions(ret);
        }, "json");
    });
}
function CascadeComparison() {
    
    $('#Comparison').change(function(ev) {
        $.post('/QueryBuilder/GetCodes/', {
            ConditionName: $('#ConditionName').val(),
            Comparison: $('#Comparison').val()
        }, function(ret) {
            UpdateCodes(ret);
        }, "json");
    });
}
function EditCondition(ev) {
    $('#ConditionGrid li').removeClass('SelectedRow');
    var qid = $(this).parent("li").attr("id");
    $('#ConditionGrid li#' + qid).addClass('SelectedRow');
    $.post('/QueryBuilder/EditCondition/' + qid, null, function(ret) {
        UpdateView(ret);
    }, "json");
    return false;
}

(function($) {
    var o = {};
    $.fn.SelectCondition = function() {
        $('#QueryConditionSelect').dialog({
            overlay: { background: "#000", opacity: 0.3 },
            bgiframe: true,
            modal: true,
            autoOpen: false,
            closeOnEscape: true,
            width: 700,
            height: 525
        });
        this.click(function(ev) {
            o.Id = this.id;
            $('#QueryConditionSelect').dialog("open");
            return false;
        });
        $('.FieldLink').click(function(ev) {
            $.post('/QueryBuilder/SelectCondition/', {
                ConditionName: ev.target.id,
                Id: $('#SelectedId').val()
            }, function(ret) {
                $('#QueryConditionSelect').dialog("close");
                UpdateView(ret);
            }, "json");
            return false;
        });
        return this;
    };
})(jQuery);

function UpdateCodes(ret) {
    $('#values').multiSelectRemove();
    $('#CodeValue').remove();
    if (ret.CodeVisible || ret.CodesVisible) {
        if (ret.SelectMultiple) {
            $('#values').after('<select id="CodeValues"></select>');
            $('#CodeValues').fillOptions(ret.CodeData, true);
            $('#CodeValues').multiSelect({ oneOrMoreSelected: '*' });
        }
        else {
            $('#values').after('<select id="CodeValue"></select>');
            $('#CodeValue').fillOptions(ret.CodeData);
        }
    }
}

function ShowErrors(j) {
    $('.validate').each(function() {
        $(this).next(".error").remove();
    });
    var e = eval('(' + j + ')');
    if (e.count == 0)
        return false;
    $('.validate').each(function() {
        if (e[this.id])
            $(this).after("<span class='error'> " + e[this.id] + "</span>");
    });
    return true;
}
function UpdateView(vs) {
    $('#SelectedId').val(vs.SelectedId);
    UpdateCodes(vs);
    $('#Comparison').fillOptions(vs.CompareData);
    if (vs.CodeVisible || vs.CodesVisible)
        CascadeComparison();
    $('#Division').fillOptions(vs.DivisionData);
    CascadeDivision();
    $('#Organization').fillOptions(vs.OrganizationData);

    $('#ConditionName').val(vs.ConditionName);
    $('#ConditionText').text(vs.ConditionText);
    $('#TextValue').val(vs.TextValue);
    $('#IntegerValue').val(vs.IntegerValue);
    $('#NumberValue').val(vs.NumberValue);
    $('#DateValue').val(vs.DateValue);
    $('#CodeValue').val(vs.CodeValue);
    $('#CodesValue').val(vs.CodesValue);
    $('#Program').val(vs.Program);
    $('#Division').val(vs.Division);
    $('#Organization').val(vs.Organization);
    $('#Schedule').val(vs.Schedule);
    $('#Tags').val(vs.Tags);
    $('#Days').val(vs.Days);
    $('#Week').val(vs.Week);
    $('#Quarters').val(vs.Quarters);
    $('#StartDate').val(vs.StartDate);
    $('#EndDate').val(vs.EndDate);
    $('#Comparison').val(vs.Comparison);

    $('#SchedDiv').showhide(vs.ScheduleVisible);
    $('#ProgDiv').showhide(vs.ProgramVisible);
    $('#DivDiv').showhide(vs.DivisionVisible);
    $('#EndDiv').showhide(vs.EndDateVisible);
    $('#StartDiv').showhide(vs.StartDateVisible);
    $('#OrgDiv').showhide(vs.OrganizationVisible);
    $('#DaysDiv').showhide(vs.DaysVisible);
    $('#WeekDiv').showhide(vs.WeekVisible);
    $('#SavedQueryDiv').showhide(vs.SavedQueryVisible);
    $('#QuartersDiv').showhide(vs.QuartersVisible);
    $('#TagsDiv').showhide(vs.TagsVisible);

    $('#RightPanel').showhide(vs.RightPanelVisible);
    $('#TextValue').showhide(vs.TextVisible);
    $('#CodeValue').showhide(vs.CodeVisible);
    $('#CodesValue').showhide(vs.CodesVisible);
    $('#DateValue').showhide(vs.DateVisible);
    $('#NumberValue').showhide(vs.NumberVisible);
    $('#IntegerValue').showhide(vs.IntegerVisible);

    $('#Update').enabled(vs.UpdateEnabled);
    $('#Add').enabled(vs.AddEnabled);
    $('#AddToGroup').enabled(vs.AddToGroupEnabled);
    $('#Remove').enabled(vs.RemoveEnabled);
}
