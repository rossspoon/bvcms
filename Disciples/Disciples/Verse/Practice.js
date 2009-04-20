var WordN = 0;
var Words;
var PartialText;
var WaitForEnter;
function StringBuilder()
{
    this.strings = new Array("");
}
StringBuilder.prototype.append = function (value)
{
    if (value)
        this.strings.push(value);
}
StringBuilder.prototype.clear = function ()
{
    this.strings.length = 1;
}
StringBuilder.prototype.toString = function ()
{
    return this.strings.join("");
}
StringBuilder.prototype.length = function ()
{
	return this.strings.join("").length;
}
function isAlpha(c) 
{
    if(c == null)
	    return false;
		
    if(c.length == 0)
	    return false;
		
	if("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".indexOf(c.charAt(0)) < 0)
		    return false;
    return true;
}

function window_onload() 
{
    WordN = 0;
    Words = document.all("HiddenField1").value.split(" ");
    PartialText = new StringBuilder();
    verse.innerText = "";
}

function window_onkeydown() 
{
  var e = window.event;
  if (e.keyCode == 27) 
    window.close();
  if (WaitForEnter && e.keyCode != 13) 
    return;
  WaitForEnter = false;
  if (WordN >= Words.length) 
  {
        window_onload();
        return;
  }
  if (WordN < Words.length)
  {
    var w = Words[WordN];
    var i=0;
    while (!isAlpha(w.charAt(i)))
        i++;
    var firstc = w.charAt(i);
    if (firstc.toUpperCase() == String.fromCharCode(e.keyCode).toUpperCase() || e.keyCode == 0x20) 
    {
        if (PartialText.length() > 0) PartialText.append(" ");
        PartialText.append(w);
        WordN += 1;
    }
    else 
    {
        document.all.beep.src='../chord.wav';
        return;
    }
  }
  verse.innerText = PartialText.toString();
  if (WordN >= Words.length) 
  {
     document.all.beep.src='../tada.wav';
     WaitForEnter = true;
  }
}
