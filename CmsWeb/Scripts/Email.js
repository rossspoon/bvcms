$(function () {
    setInterval(KeepSessionAlive, 120000);
    $("#progress").dialog({
        autoOpen: false,
        modal: true,
        close: function () {
            $('#progress').html("<h2>Working...</h2>");
        }
    });
    $.Send = function () {
        var d = $("#progress");
        d.dialog('open');
        $('#Body').text(CKEDITOR.instances["Body"].getData());
        var q = $('form').serialize();
        $.post('/Email/QueueEmails', q, function (ret) {
            var taskid = ret.id;
            if (taskid == 0) {
                d.html(ret.content);
            }
            else {
                var intervalid = window.setInterval(function () {
                    $.post('/Email/TaskProgress/' + taskid, function (ret) {
                        if (ret.substr(0, 20).toLowerCase().indexOf('<!--completed-->') >= 0)
                            window.clearInterval(intervalid);
                        d.html(ret);
                    });
                }, 3000);
            }
        }, "json");
    };
    $.TestSend = function () {
        var d = $("#progress");
        d.dialog('open');
        $('#Body').text(CKEDITOR.instances["Body"].getData());
        var q = $('form').serialize();
        $.post('/Email/TestEmail', q, function (ret) {
            d.html(ret);
        });
    };
});
function KeepSessionAlive() {
    $.post("/Account/KeepAlive", null, null);
}  
