$(function () {
    $.onready = function () {
        $(".clickEdit").editable("/SavedQuery/Edit/", {
            indicator: "<img src='/images/loading.gif'>",
            tooltip: "Click to edit...",
            style: 'display: inline',
            width: '200px',
            height: 25,
            submit: "OK"
        });
    }
    $.onready();
    $("#onlyMine").click(function () {
        var q = $("form").serialize();
        $.post("/SavedQuery/Rows/", q, function (ret) {
            $("table.grid > tbody").html(ret).ready($.onready);
        });
    });
    $(".bt").button();
});
