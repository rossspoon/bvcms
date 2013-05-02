$(function () {
    $("a.bt").button();
    $.URL = function(base) {
        var u = base + $("#OrgId").val()
            + "?sg1=" + $("#sg1").val() + "&sg2=" + $("#sg2").val();
        if ($("#SortByWeek").val() === "True")
            return u + "&sortbyweek=true";
        return u;
    };
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
    $("body").on("change", '.smallgroup', function () {
        $.block();
        $.post("/Volunteers/ManageArea2/" + $("#OrgId").val(), {
            sg1: $("#sg1").val(),
            sg2: $("#sg2").val(),
            SortByWeek: $("#SortByWeek").val()
        }, function (ret) {
            $("#managearea").html(ret).ready($readyHover);
            $.unblock();
        });
    });
    $("body").on("click", '#SortIt', function (ev) {
        ev.preventDefault();
        if ($("#SortByWeek").val() === "False")
            $("#SortByWeek").val("True");
        else
            $("#SortByWeek").val("False");
        $.block();
        window.location = $.URL("/Volunteers/Calendar/");
    });
    $("body").on("click", '.selectable', function (ev) {
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
    $("body").on("click", "div.drop.ui-state-active", function (ev) {
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
            url: "/Volunteers/ManageArea/",
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
