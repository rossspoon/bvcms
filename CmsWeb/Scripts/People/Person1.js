﻿$(function () {
    $("#split").live("click", function (ev) {
        ev.preventDefault();
        var href = $(this).attr("href");
        bootbox.confirm("Are you sure you want to split this person into their own family?", function (result) {
            if (result === true) {
                $.post(href, {}, function (ret) {
                    window.location = ret;
                });
            }
        });
    });
    $("#deletePerson").live("click", function (ev) {
        ev.preventDefault();
        var href = $(this).attr("href");
        bootbox.confirm("Are you sure you want to delete this record?", function (result) {
            if (result === true) {
                $.post(href, {}, function (ret) {
                    window.location = ret;
                });
            }
        });
    });
    $("#search-add a.commit").live("click", function (ev) {
        ev.preventDefault();
        var f = $(this).closest("form");
        var q = f.serialize();
        var loc = $(this).attr("href");
        var tar = f.closest("div.modal");
        $.post(loc, q, function (ret) {
            tar.modal("hide");
            if (ret.message) {
                alert(ret.message);
            }
            else
                switch (ret.from) {
                    case 'RelatedFamily':
                        $("#related-families-div").load('/Person2/RelatedFamilies/' + ret.pid, {}, function () {
                            $.SetRelationEditable();
                            $(ret.key).editable("toggle");
                        });
                        break;
                    case 'Family':
                        $("#family-div").load('/Person2/FamilyGrid/' + ret.pid, {});
                        break;
                    case 'Menu':
                        window.location = '/Person2/' + ret.pid;
                        break;
                }
        });
        return false;
    });
    $("#deleterelation").live("click", function (ev) {
        ev.preventDefault();
        var p = $(this).closest("p");
        var href = p.data("delete");
        bootbox.confirm("Are you sure you want to remove this relationship?", function (result) {
            if (result === true) {
                $("#related-families-div").load(href, {});
            }
        });
        return false;
    });
    $("a.editaddr").live("click", function (ev) {
        ev.preventDefault();
        $("<div class='modal fade hide' />").load($(this).attr("href"), {}, function () {
            var modal = $(this);
            modal.modal("show");
            modal.on('hidden', function () {
                $(this).remove();
            });
            modal.on("click", "a.close-saved-address", function () {
                $.post($(this).attr("href"), {}, function (ret) {
                    $("#snapshot").html(ret).ready($SetSnapshotEditable);
                });
            });
        });
    });
    $("#family_related a.edit").live("click", function (ev) {
        ev.stopPropagation();
        ev.preventDefault();
        $(this).closest('div.open').removeClass('open');
        $(this).closest("li.relation-item").find("span.relation-description").editable("toggle");
    });
    $.fn.editableform.buttons =
        '<button type="submit" class="btn btn-success editable-submit"><i class="icon-ok icon-white"></i></button>' +
        '<button type="button" class="btn editable-cancel"><i class="icon-remove"></i></button>';
    $.SetRelationEditable = function () {
        $('span.relation-description').editable({
            type: "textarea",
            toggle: "manual",
            name: "description",
            url: function (params) {
                var d = new $.Deferred;
                $.post('/Person2/EditRelation/' + params.pk, { value: params.value }, function (data) {
                    d.resolve();
                });
                return d.promise();
            }

        });
        $('span.relation-description').on('hidden', function (e, reason) {
            if (reason === 'save' || reason === 'cancel') {
                $.fn.editableform.buttons =
                    '<button type="submit" class="btn btn-success editable-submit"><i class="icon-ok icon-white"></i></button>' +
                    '<button type="button" class="btn editable-cancel"><i class="icon-remove"></i></button>';
            }
        });
        $('span.relation-description').on('shown', function (e, reason) {
            $.fn.editableform.buttons =
                 '<button type="submit" class="btn btn-success editable-submit"><i class="icon-ok icon-white"></i></button>' +
                 '<button type="button" class="btn editable-cancel"><i class="icon-remove"></i></button>' +
                 '<br><br><button id="deleterelation" type="button" class="btn btn-danger pull-right">delete</button>';
        });
    };
    $.SetRelationEditable();

    $('a.deloptout').live("click", function (ev) {
        ev.preventDefault();
        var href = $(this).attr("href");
        if (confirm('Are you sure you want to delete?')) {
            $.post(href, {}, function (ret) {
                if (ret != "ok")
                    $.growlUI("failed", ret);
                else {
                    $.updateTable($('#user-tab form'));
                    $.growlUI("Success", "OptOut deleted");
                }
            });
        }
    });
    $('#moveperson').click(function (ev) {
        ev.preventDefault();
        var d = $('#dialogbox');
        $('iframe', d).attr("src", this.href);
        d.dialog("option", "title", "Merge To Person");
        d.dialog("open");
        return false;
    });

    $("form.ajax a.membertype").live("click", function (ev) {
        ev.preventDefault();
        $("<div class='modal fade hide' />").load($(this).attr("href"), {}, function () {
            $(this).modal("show");
            $(this).on('hidden', function () {
                $(this).remove();
            });
            //            $(this).on("click", "a.close-saved-address", function () {
            //                $("#primaryaddress").html($("#primaryaddressnew").html());
            //                var target = $("#addressnew").data("target");
            //                $("#" + target).html($("#addressnew").html());
            //            });
        });
    });

    $("#currentLink").click(function () {
        $.showTable($('#current form'));
    });
    $("#previousLink").click(function () {
        $.showTable($('#previous form'));
    });
    $("#pendingLink").click(function () {
        $.showTable($('#pending form'));
    });
    $("#attendsLink").click(function () {
        $.showTable($('#attends form'));
    });
    $("#contacts-link").click(function () {
        $("#contacts-tab form").each(function () {
            $.showTable($(this));
        });
    });
    $("#member-link").click(function () {
        var f = $("#memberdisplay");
        if ($("table", f).size() == 0) {
            $.post(f.attr('action'), null, function (ret) {
                $(f).html(ret).ready(function () {
                    $.UpdateForSection(f);
                });
            });
            $.showTable($("#extras-tab form"));
            $.extraEditable('#extravalues');
        }
    });
    $("#system-link").click(function () {
        $.showTable($("#user-tab form"));
    });
    $("#changes-link").click(function () {
        $.showTable($("#changes-tab form"));
    });
    $("#volunteer-link").click(function () {
        $.showTable($("#volunteer-tab form"));
    });
    $("#duplicates-link").click(function () {
        $.showTable($("#duplicates-tab form"));
    });
    $("#optouts-link").click(function () {
        $.showTable($("#optouts-tab form"));
    });
    $("#recreg-link").click(function (ev) {
        ev.preventDefault();
        var f = $('#recreg-tab form');
        if ($('table', f).size() > 0)
            return false;
        var q = f.serialize();
        $.post(f.attr('action'), q, function (ret) {
            $(f).html(ret);
            $(".bt", f).button();
        });
        return false;
    });

    $('#future').live("click", function (ev) {
        ev.preventDefault();
        var f = $(this).closest('form');
        var q = f.serialize();
        $.post($("#FutureLink").val(), q, function (ret) {
            $(f).html(ret);
        });
    });
    $.validator.addMethod("date2", function (value, element, params) {
        var v = $.DateValid(value);
        return this.optional(element) || v;
    }, $.format("Please enter valid date"));

    $.validator.setDefaults({
        highlight: function (input) {
            $(input).addClass("ui-state-highlight");
        },
        unhighlight: function (input) {
            $(input).removeClass("ui-state-highlight");
        },
        rules: {
            "NickName": { maxlength: 15 },
            "Title": { maxlength: 10 },
            "First": { maxlength: 25 },
            "Middle": { maxlength: 15 },
            "Last": { maxlength: 100, required: true },
            "Suffix": { maxlength: 10 },
            "AltName": { maxlength: 100 },
            "Maiden": { maxlength: 20 },
            "HomePhone": { maxlength: 20 },
            "CellPhone": { maxlength: 20 },
            "WorkPhone": { maxlength: 20 },
            "EmailAddress": { maxlength: 150 },
            "School": { maxlength: 60 },
            "Employer": { maxlength: 60 },
            "Occupation": { maxlength: 60 },
            "WeddingDate": { date2: true },
            "DeceasedDate": { date2: true },
            "Grade": { number: true },
            "Address1": { maxlength: 40 },
            "Address2": { maxlength: 40 },
            "City": { maxlength: 30 },
            "Zip": { maxlength: 15 },
            "FromDt": { date2: true },
            "ToDt": { date2: true },
            "DecisionDate": { date2: true },
            "JoinDate": { date2: true },
            "BaptismDate": { date2: true },
            "BaptismSchedDate": { date2: true },
            "DropDate": { date2: true },
            "NewMemberClassDate": { date2: true }
        }
    });
    $('#addrf').validate();
    $('#addrp').validate();
    $('#basic').validate();
    $("body").on("change", '.atck', function (ev) {
        var ck = $(this);
        $.post("/Meeting/MarkAttendance/", {
            MeetingId: $(this).attr("mid"),
            PeopleId: $(this).attr("pid"),
            Present: ck.is(':checked')
        }, function (ret) {
            if (ret.error) {
                ck.attr("checked", !ck.is(':checked'));
                alert(ret.error);
            }
            else {
                var f = ck.closest('form');
                var q = f.serialize();
                $.post($(f).attr("action"), q, function (ret) {
                    $(f).html(ret);
                });
            }
        });
    });
    $("body").on("click", 'a.deleteextra', function (ev) {
        ev.preventDefault();
        if (confirm("are you sure?"))
            $.post("/Person/DeleteExtra/" + $("#PeopleId").val(), { field: $(this).attr("field") }, function (ret) {
                if (ret.startsWith("error"))
                    alert(ret);
                else {
                    $.getTable($("#extras-tab form"));
                    $.extraEditable('#extravalues');
                }
            });
        return false;
    });
    $("form").on('click', 'a.reverse', function (ev) {
        ev.preventDefault();
        var f = $(this).closest('form');
        $.post("/Person/Reverse", {
            id: $("#PeopleId").val(),
            field: $(this).attr("field"),
            value: $(this).attr("value"),
            pf: $(this).attr("pf")
        }, function (ret) {
            $(f).html(ret);
        });
    });
    $('#vtab>ul>li').click(function () {
        $('#vtab>ul>li').removeClass('selected');
        $(this).addClass('selected');
        var index = $('#vtab>ul>li').index($(this));
        $('#vtab>div').hide().eq(index).show();
    });
    var getMap = function (opts) {
        var src = "http://maps.googleapis.com/maps/api/staticmap?",
            params = $.extend({
                center: 'New York, NY',
                size: '128x128',
                sensor: false
            }, opts),
            query = [];

        $.each(params, function (k, v) {
            query.push(k + '=' + encodeURIComponent(v));
        });

        src += query.join('&');
        return '<img src="' + src + '" /><br><a href="https://www.google.com/maps/?q=' + opts['center'] + '" rel="external" target="_blank">View in Google Maps</a>\
      <br><a href="http://www.bing.com/maps/?q=' + opts['center'] + '" rel="external" target="_blank">View in Bing Maps</a>';
    };
    var $SetSnapshotEditable = function () {
        $('[class="popover-map"]').each(function () {
            var $this = $(this);
            $this.data('html', true).data('content', getMap({ center: $this.text() }));
            $this.popover();
        });
        $('#FamilyPosition').editable({
            source: [{
                value: 10,
                text: "Primary Adult"
            }, {
                value: 20,
                text: "Secondary Adult"
            }, {
                value: 30,
                text: "Child"
            }],
            type: "select",
            url: "/Person2/PostData",
            name: "position"
            //        success: function (data) {
            //            $("#family-div").load('/Person2/FamilyGrid/' + $("#position").data("pk"), {});
            //        }
        });
        $('#Campus').editable({
            source: "/Person2/Campuses",
            type: "select",
            url: "/Person2/PostData",
            name: "campus"
        });
    };
    $SetSnapshotEditable();
    $('body').on('click', function (e) {
        $('[rel=popover]').each(function () {
            //the 'is' for buttons that trigger popups
            //the 'has' for icons within a button that triggers a popup
            if (!$(this).is(e.target) && $(this).has(e.target).length === 0 && $('.popover').has(e.target).length === 0) {
                $(this).popover('hide');
            }
        });
    });
});



function RebindMemberGrids() {
    $.updateTable($('#current-tab form'));
    $.updateTable($('#pending-tab form'));
    $("#memberDialog").dialog('close');
}
function RebindUserInfoGrid() {
    $.updateTable($('#user-tab form'));
    $("#memberDialog").dialog('close');
}
function AddSelected(ret) {
    window.location = "/Merge?PeopleId1=" + $("#PeopleId").val() + "&PeopleId2=" + ret.pid;
}
function dialogError(arg) {
    return arg;
}