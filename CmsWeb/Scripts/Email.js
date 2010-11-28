$(function () {
    setInterval(KeepSessionAlive, 120000);
    $("#progress").dialog({ autoOpen: false });
    $.Send = function () {
        var d = $("#progress");
        d.dialog('open');
        $('#Body').text(CKEDITOR.instances["Body"].getData());
        var q = $('form').serialize();
        $.post('/Email/QueueEmails', q, function (ret) {
            var taskid = ret.id;
            if (taskid == 0) {
                $('#progress').html(ret.content);
            }
            else {
                window.setInterval(function () {
                    $.post('/Email/TaskProgress/' + taskid, function (ret) {
                        $('#progress').html(ret);
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
            $('#progress').html(ret);
        });
    };
});
function KeepSessionAlive() {
    $.post("/Account/KeepAlive", null, null);
}  
