function addhover() {
    var navli, i;
    // loop through all elements in the nav
    navli = document.getElementById('centeredmenu').getElementsByTagName('li')
    for (i = 0; i < navli.length; i++) {
        // add the hover functions to the li onmouseover and onmouseout
        navli[i].onmouseover = function () { hover(this, 'hover'); };
        navli[i].onmouseout = function () { hover(this, ''); };
    }
}
function hover(o, sClass) {
    if (o) {
        o.className = sClass;
    }
}
addhover();