var curVisible=null;
var curObjID=null;

document.attachEvent('onclick',handleClick);
var IE = document.all?true:false
if (!IE) document.captureEvents(Event.MOUSEMOVE)

function findPos(obj) {
    var location = Sys.UI.DomElement.getLocation(obj);
  	return [location.x,location.y];
  	
//	var curleft = curtop = 0;
//	if (obj.offsetParent) {
//		curleft = obj.offsetLeft
//		curtop = obj.offsetTop
//		while (obj = obj.offsetParent) {
//			curleft += obj.offsetLeft
//			curtop += obj.offsetTop
//		}
//	}
//	return [curleft,curtop];
}

function placeDiv(objid) {
    if(curObjID == objid){ removeDiv(curVisible); return; }
    if(curVisible!=null) { removeDiv(curVisible); }

	var dd  = document.getElementById(objid);
	var div = document.getElementById(objid+'div');
	var checkboxes = div.getElementsByTagName('input');
	var ddm = ';' + dd.value + ';'
	for (i=0; i<checkboxes.length; i++)
		checkboxes[i].checked=ddm.match(';' + checkboxes[i].value + ';') != null; 
	setLyr(dd,div);
	showItem(div);
	div.focus();
	curVisible=div;
	curObjID=objid;
}

function removeDiv(div) {
	var dd  = document.getElementById(div.id.replace('div',''));
	var checkboxes = div.getElementsByTagName('input');
	var returnArray=new Array(0);
	
	for (i=0; i<checkboxes.length; i++)
		if(checkboxes[i].checked)
			returnArray.push(checkboxes[i].value);
	dd.value = returnArray.join(';');
	hideItem(div);
	curVisible=null;
	curObjID=null;
	PerformPostActions(dd.getAttribute('alt'));
}

function setLyr(obj,lyr) {
    var loc = Sys.UI.DomElement.getLocation(obj);
    Sys.UI.DomElement.setLocation(lyr, loc.x, loc.y + 22);
}

function showItem(obj) {
	obj.style.visibility='visible';
}

function hideItem(obj) {
	obj.style.visibility='hidden';
}

function handleClick() {
	if(curVisible!=null)
	{
		var r = {l: curVisible.offsetLeft, t: curVisible.offsetTop, r: curVisible.offsetWidth, b: curVisible.offsetHeight};
		var curVisibleOP = curVisible.offsetParent; 
		r.l += curVisibleOP.offsetLeft;
		r.t += curVisibleOP.offsetTop;
		r.r += (r.l+18);
		r.b += r.t;
		r.t -= 22;
		var p = getMouseXY(document);
		if( (p.x>r.r) || (p.x<r.l) || (p.y>r.b) || (p.y<r.t) )  //no hit!
			removeDiv(curVisible);
	}	
}

function getMouseXY(e) {
  if (IE) {
    tempX = event.clientX + document.body.scrollLeft;
    tempY = event.clientY + document.body.scrollTop;
  } else {
    tempX = e.pageX;
    tempY = e.pageY;
  }  
  // catch possible negative values in NS4
  if (tempX < 0) tempX = 0;
  if (tempY < 0) tempY = 0;  
  return {x: tempX, y: tempY};
}
