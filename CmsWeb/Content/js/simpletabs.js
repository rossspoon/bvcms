$.fn.simpletabs = function() {
    return this.each(function() {
        var ul = $(this);
        ul.find('a[href^=#]').each(function(i) {
            var link = $(this);
            if (i)
                $(link.attr('href')).hide();
            else
                link.addClass('selected');

            link.click(function() {
                $(ul.find('a.selected').removeClass('selected').attr('href')).hide();
                $(link.addClass('selected').attr('href')).show();
                return false;
            });
        });
    });
};