    $(document).ready(function(){ 
        $("ul.sf-tab").superfish({ 
            autoArrows:  false
        }); 
    }); 

    function TBNewWindow(url)
    {
        window.open(url, "_blank");
    }
    function TBNavWindow(url)
    {
        window.open(url, "_self");
    }
    var target, queryid, titles, format;
    function TBExportGo()
    {
        url = target
            + "id=" + queryid
            + "&titles=" + $get(titles).checked
            + "&format=" + $('input[name=' + format + ']:checked').val();
        window.open(url, "_blank");
    }
    function TBshowPopup(where, qid, usetitles, option, popup)
    {
        target = where;
        queryid = qid;
        titles = usetitles;
        format = option;
        $find(popup).show();
   }
