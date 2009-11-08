$(document).ready(function() {
    $(document).keydown(function(eventObj) {
        var k = getKey(eventObj);
        if (k == 27)
            window.close();
        if (WaitForEnter && k != 13)
            return;
        WaitForEnter = false;
        if (WordN >= Words.length) {
            init();
            return;
        }
        if (WordN < Words.length) {
            var w = Words[WordN];
            var i = 0;
            while (!isAlpha(w.charAt(i)))
                i++;
            var firstc = w.charAt(i);
            if (firstc.toUpperCase() == String.fromCharCode(k).toUpperCase() || k == 0x20) {
                if (PartialText.length() > 0) PartialText.append(" ");
                PartialText.append(w);
                WordN += 1;
            }
            else {
                $("#verse").highlightFade({ color: 'red', speed: 100 })
                return;
            }
        }
        $('#verse').text(PartialText.allstrings());
        if (WordN >= Words.length) {
            $("#verse").highlightFade({ color: 'yellow', speed: 3000, iterator: 'exponential' })
            WaitForEnter = true;
        }
    });
    $('#helpshow').colorbox({ inline: true, href: "#helppanel" });
    $('#helphide').click(function() {
        $.fn.colorbox.close();
    });
    function init() {
        WordN = 0;
        Words = $("#HiddenField1").val().split(" ");
        PartialText = new StringBuilder();
        $('#verse').text("");
    }
    function isAlpha(c) {
        if (c == null)
            return false;

        if (c.length == 0)
            return false;

        if ("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".indexOf(c.charAt(0)) < 0)
            return false;
        return true;
    }
    var WordN = 0;
    var Words;
    var PartialText;
    var WaitForEnter;
    init();
    function getKey(key) {
        if (key == null)
            keycode = event.keyCode;
        else
            keycode = key.keyCode;
        return keycode;
    }

});

function StringBuilder() {
    this.strings = new Array("");
}
StringBuilder.prototype.append = function(value) {
    if (value)
        this.strings.push(value);
}
StringBuilder.prototype.clear = function() {
    this.strings.length = 1;
}
StringBuilder.prototype.allstrings = function() {
    return this.strings.join("");
}
StringBuilder.prototype.length = function() {
    return this.strings.join("").length;
}
