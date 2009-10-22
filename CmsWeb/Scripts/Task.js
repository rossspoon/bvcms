$(function() {
    SearchClicked = RefreshList;
    $('#SearchPeopleDialog').SearchPeopleInit({ overlay: { background: "#000", opacity: 0.3} });
    $('#tabs').tabs();
    $("#tabs > ul > li > a").click(function() {
        var x = $(this).attr("href").substring(1);
        return ClickTab(x);
    });

    SelectTab();
    StripeList();
    var c = $.cookie("tasklast");
    $('#tasks > thead a.sortable').click(function(ev) {
        $("#Sort").val($(this).text());
        RefreshList();
    });
    BindSearchPeople();
});
function RefreshList() {
    var q = $('#form').formSerialize2();
    $.navigate("/Task/List",q);
}
function GotoPage(pg) {
    var q = $('#form').formSerialize2();
    q = q.appendQuery("Page=" + pg);
    $.navigate("/Task/List", q);
}
function SetPageSize(sz) {
    var q = $('#form').formSerialize2();
    q = q.appendQuery("PageSize=" + sz);
    $.navigate("/Task/List", q);
}
function SelectTab(tab) {
    if (tab)
        $('#CurTab').val(tab);
    $('#tabs').tabs('select', '#' + $('#CurTab').val()); 
}
function ClickTab(tab) {
    if (tab)
        $('#CurTab').val(tab);
    $.cookie('CurTaskTab', tab, { expires: 360 });
    RefreshList();
    return false;
}
function StripeList() {
    $('#tasks > tbody > tr:even').addClass('alt');
}
function DeleteList(qs) {
    $.post('/Task/Action/', qs, function(ret) {
        var a = ret.split("<---------->");
        $('#tabs').html(a[0]);
        $("#actions").html(a[1]);
        $('#tasks > tbody').html(a[2]).ready(StripeList);
    });
}
function DoAction() {
    var v = $('#actions option:selected').val();
    var ai = $(".actionitem:checked").getCheckboxVal().join(",");
    var qs = "option=" + v + "&curtab=" + $('#CurTab').val() + "&items=" + ai;
    $('#actions').attr('selectedIndex', 0);
    if (ai = "")
        return;
    switch (v) {
        case '':
        case '-':
            return;
        case 'delegate':
        case 'sharelist':
            alert('not implemented yet');
            return;
        case 'deletelist':
            if (confirm('Are you sure you want to delete the list?')) {
                $.block();
                DeleteList(qs);
                $.unblock();
            }
            return;
        default:
            $.block();
            $.post('/Task/Action/', qs, function(ret) {
                $('#tasks > tbody').html(ret).ready(StripeList);
                $.unblock();
            });
            return;
    }
}
function AddListEnter(e) {
    var key = window.event ? e.keyCode : e.which;
    if (key != 13)
        return true;
    AddListClick()
    return false;
}
function AddListClick() {
    var qs = "ListName=" + $("#ListName").val();
    $.post('/Task/AddList/', qs, function(ret) {
        var a = ret.split("<---------->");
        $('#tabs').html(a[0]);
        $("#actions").html(a[1]);
        $('#ListName').val('');
    });
    return false;
}
function AddTaskEnter(e) {
    var key = window.event ? e.keyCode : e.which;
    if (key != 13)
        return true;
    AddTaskClick();
    return false;
}
function AddTaskClick() {
    var qs = "TaskDesc=" + $("#TaskDesc").val() + "&CurTab=" + $("#CurTab").val();
    $.post('/Task/AddTask/', qs, function(ret) {
        $("#nomatch").remove();
        var alt = !($('#tasks > tbody tr:visible:first').hasClass("alt") || false);
        $('#tasks > tbody tr:first').before(ret);
        if (alt)
            $('#tasks > tbody tr:first').addClass("alt");
        $('#TaskDesc').val('');
    });
}
function ShowDetail(id) {
    var drid = $("#TaskId").val();
    if (drid) {
        $.post("/Task/Detail/" + id + "/Row/" + drid, function(ret) {
            var a = ret.split("<---------->");
            $('#r' + drid).html(a[0]);
            $('#r' + drid).removeClass("detailrow");
            $('#r' + id).addClass("detailrow").html(a[1]);
            BindSearchPeople();
        });
    }
    else {
        $.post('/Task/Detail/' + id, function(ret) {
            $('#r' + id).addClass("detailrow");
            $('#r' + id).html(ret);
            BindSearchPeople();
        });
    }
}
function Deselect() {
    var id = $("#TaskId").val();
    $.post('/Task/Columns/' + id, function(ret) {
        $('#r' + id).removeClass("detailrow").html(ret);
    });
    return false;
}
function SetPriority(id, priority) {
    $.getJSON('/Task/Priority/'+id+'?priority='+priority, null, function(ret) {
        $('#Priority').text(ret.Priority);
    });
    return false;
}
function SetComplete(id) {
    $.post('/Task/SetComplete/' + id, null, function(ret) {
        $('#r' + id).removeClass('detailrow').html(ret);
    });
    return false;
}
function Accept(id) {
    $.post('/Task/Accept/' + id, null, function(ret) {
        $('#r' + id).html(ret);
        BindSearchPeople();
    });
    return false;
}
$(function() {
    $("#dialogbox").dialog({
        overlay: { background: "#000", opacity: 0.8 },
        bgiframe: true,
        modal: true,
        autoOpen: false,
        closeOnEscape: true,
        width: 700,
        height: 525,
        position: 'top',
        close: function(event, ui) {
            $('#dialogbox').empty();
            SearchClicked = RefreshList;
        }
    });
});

var queryString = "";
function ChangePage() { }
function SearchClicked() { }
function SelectPerson(id) { }

function SearchContacts() {
    SearchClicked = SearchContactClicked;
    ChangePage = ChangeContactPage;
    $('#dialogbox').dialog("option", "title", "Select Contact");
    $('#dialogbox').load("/Task/SearchContact/", null, function() {
        queryString = $('#searchform').formSerialize2();
        $(".datepicker").datepicker({ changeYear: true, changeMonth: true });
        $("#contacts").initPager();
        $('#contacts > thead a.sortable').click(function(ev) {
            $("#contacts #Sort").val($(ev.target).text());
            queryString = $('#searchform').formSerialize2();
            $.post('/Task/SearchContact/0', queryString, function(ret) {
                $('#contacts > tbody').html(ret).ready(function() { $("#contacts").initPager(); });
            });
            return false;
        });
    });
    $('#dialogbox').dialog("open");
}
function AddSourceContact(contactid) {
    var taskid = $('#TaskId').val();
    $.post('/Task/AddSourceContact/' + taskid + "?contactid=" + contactid, null, function(ret) {
        $('#r' + taskid).html(ret);
        BindSearchPeople();
    });
    $('#dialogbox').dialog("close");
}
function CompleteWithContact() {
    var taskid = $('#TaskId').val();
    $.post('/Task/CompleteWithContact/' + taskid, null, function(ret) {
        window.location = "/Contact.aspx?edit=1&id=" + ret.ContactId;
    }, "json");
}
function BindSearchPeople() {
    $('#changeowner').click(function(ev) {
        $('#SearchPeopleDialog').SearchPeople(ev, function(id, peopleid) {
            ActOnPerson('/Task/ChangeOwner/', peopleid);
        });
        return false;
    });
    $('#delegate').click(function(ev) {
        $('#SearchPeopleDialog').SearchPeople(ev, function(id, peopleid) {
            ActOnPerson('/Task/Delegate/', peopleid);
        });
        return false;
    });
    $('#changeabout').click(function(ev) {
        $('#SearchPeopleDialog').SearchPeople(ev, function(id, peopleid) {
            ActOnPerson('/Task/ChangeAbout/', peopleid);
        });
        return false;
    });
}
function ActOnPerson(action, peopleid) {
    var taskid = $('#TaskId').val();
    $.post(action + taskid + "?peopleid=" + peopleid, null, function(ret) {
        $('#r' + taskid).html(ret);
        BindSearchPeople();
    });
    $('#dialogbox').dialog("close");
}
function ChangeContactPage(page, pager) {
    $.post('/Task/SearchContact/' + page, queryString, function(ret) {
        $('#contacts > tbody').html(ret);
    });
    return false;
}
function ChangePeoplePage(page, pager) {
    $.post('/SearchPeople/' + page, queryString, function(ret) {
        $('#people > tbody').html(ret);
    });
    return false;
}
$.fn.initPager = function() {
    this.each(function() {
        $(".pagination", this).pagination($("#Count", this).val(), {
            items_per_page: $("#PageSize", this).val(),
            num_display_entries: 5,
            num_edge_entries: 1,
            current_page: 0,
            callback: ChangePage
        });
        $('#NumItems', this).text($('#Count', this).val().addCommas() + " items");
    });
    return this;
};
function SearchContactClicked() {
    queryString = $('#searchform').formSerialize2();
    $.post('/Task/SearchContact/0', queryString, function(ret) {
        $('#contacts > tbody').html(ret).ready(function() { $("#contacts").initPager(); });
    });
    return false;
}
function Edit() {
    var id = $("#TaskId").val();
    $.post('/Task/Edit/' + id, function(ret) {
        $('#r' + id).html(ret);
        $(".datepicker").datepicker({ changeYear: true, changeMonth: true });
    });
} 
function Update() {
    var id = $("#TaskId").val();
    var qs = $("#Edit").formSerialize2();
    $.post('/Task/Update/' + id, qs, function(ret) {
        $('#r' + id).html(ret);
        BindSearchPeople();
    }, "html");
    return false;
}
