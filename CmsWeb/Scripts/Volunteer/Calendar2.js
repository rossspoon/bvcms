$(function () {
    $("a.bt").button();
    $.URL = function(base) {
        var u = base + $("#OrgId").val()
            + "?sg1=" + $("#sg1").val() + "&sg2=" + $("#sg2").val();
        if ($("#SortByWeek").val() === "True")
            return u + "&sortbyweek=true";
        return u;
    };
    $("a.tip").tooltip({ showBody: "|", showURL: false });
    $("#swk > li > a").click(function(ev) {
        ev.preventDefault();
        var wk = $(this).data("week");
        if (wk === 0) {
            $("#month .wki").removeClass("hidewkn");
        } else {
            $("#month .wki").addClass("hidewkn");
            $("#month .wkn" + wk).removeClass("hidewkn");
        }
        $("#swk > li > a").removeClass("selected");
        $(this).addClass("selected");
        $.DisplayWeeks(1);
    });
    $("#goright").click(function(ev) {
        ev.preventDefault();
        var pg = parseInt($("#page").val()) + 1;
        $.DisplayWeeks(pg);
    });
    $("#goleft").click(function(ev) {
        ev.preventDefault();
        var pg = parseInt($("#page").val()) - 1;
        $.DisplayWeeks(pg);
        return false;
    });
    $.DisplayWeeks = function(pg) {
        $("#month .wki").removeClass("hidewki");
        var items = $("#month thead th:visible").map(function() {
            return $(this).data("item");
        });
        var weekstoshow = parseInt($("#weekstoshow").val());
        var i;
        var li = items[items.length - 1];
        $("#month .wki").addClass("hidewki");
        var fi = (pg - 1) * weekstoshow;
        if (fi >= li) {
            fi = li - 1;
            pg -= 1;
        }
        if (li - fi < weekstoshow)
            fi = li - weekstoshow;
        if (fi < 0)
            return $.DisplayWeeks(1);
        for (i = fi; i < (fi + weekstoshow) && i < li; i++)
            $("#month .wki" + items[i]).removeClass("hidewki");
        $("#page").val(pg);
        return false;
    };
    $("#CreateMeeting").live("click", function(ev) {
        ev.preventDefault();
        var q = $(this).data("item");
        var td = $(this).closest("td");
        $.post("/Org/Volunteers/NewMeetingSlot", q, function (ret) {
            td.html(ret);
        });
        return false;
    });
    $("div[source=registered]").tooltip({ showBody: "|" });
    $('body').on("click", '#sendreminders', function (ev) {
        ev.preventDefault();
        var href = $(this).attr("href");
        if (confirm('Are you sure you want to send reminders?')) {
            $.block("sending reminders");
            $.post(href, { "emailall": $("#emailall").is(':checked') }, function (ret) {
                if (ret != "ok") {
                    $.unblock();
                    $.growlUI("error", ret);
                }
                else {
                    $.unblock();
                    $.growlUI("Email", "Reminders Sent");
                }
            });
        }
    });
    $(".smallgroup").live("change", function () {
        $.block();
        $.post("/Org/Volunteers/ManageArea2/" + $("#OrgId").val(), {
            sg1: $("#sg1").val(),
            sg2: $("#sg2").val(),
            SortByWeek: $("#SortByWeek").val()
        }, function (ret) {
            $("#managearea").html(ret).ready($readyHover);
            $.unblock();
        });
    });
    $("#SortId").live("click", function (ev) {
        ev.preventDefault();
        if ($("#SortByWeek").val() === "False")
            $("#SortByWeek").val("True");
        else
            $("#SortByWeek").val("False");
        $.block();
        window.location = $.URL("/Org/Volunteers/Calendar/");
    });
    $(".selectable").live("click", function (ev) {
        if (ev.target.nodeName != 'A') {
            if ($(this).attr("source") != $(".selected").attr("source"))
                $(".selectable").removeClass("pidHighlight2")
                    .removeClass("selected")
                    .removeClass("pidHighlight2");

            if ($(this).hasClass("selected")) {
                $(this).removeClass("selected");
                $("[pid]").removeClass("pidHighlight2");
                return;
            }
            $(this).addClass("selected");
            if($(this).is("[pid]"))
                $("[pid='" + $(this).attr("pid") + "']").addClass("pidHighlight2");
        }
    });
    $readyHover = function () {
        $("div.drop").hover(
          function () {
              $(this).addClass("ui-state-active");
          },
          function () {
              $(this).removeClass("ui-state-active");
          }
        );
        $("div.selectable").hover(
            function () {
                if ($(this).is("[pid]"))
                    $("[pid='" + $(this).attr("pid") + "']").addClass("pidHighlight");
                else
                    $(this).addClass("pidHighlight");
            },
            function () {
                if ($(this).is("[pid]"))
                    $("[pid='" + $(this).attr("pid") + "']").removeClass("pidHighlight");
                else
                    $(this).removeClass("pidHighlight");
            }
        );
    };
    $readyHover();
    $("div.drop.ul-state-active").live("click", function (ev) {
        var $this = $(this);
        var list = [];
        $(".selected").each(function (index) {
            list.push({
                "source": $(this).attr("source"),
                "pid": $(this).attr("pid"),
                "mid": $(this).attr("mid")
            });
        });
        if (list.length == 0)
            return;
        $.block();
        var $info = {
            "id": $("#OrgId").val(),
            "sg1": $("#sg1").val(),
            "sg2": $("#sg2").val(),
            "target": $this.attr("target"),
            "week": $this.attr("week"),
            "time": $this.attr("time"),
            "SortByWeek": $("#SortByWeek").val(),
            "list": list
        };
        $.ajax({
            url: "/Org/Volunteers/ManageArea/",
            data: JSON.stringify($info),
            success: function (ret) {
                $("#managearea").html(ret).ready($readyHover);
                $.unblock();
            },
            error: function (ret) {
                alert(ret);
            },
            type: 'POST',
            contentType: 'application/json, charset=utf-8',
            dataType: 'html'
        });
    });
});
