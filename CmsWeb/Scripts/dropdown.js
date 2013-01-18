$(function() {
    $.fn.dropdown = function() {
        return this.each(function() {
            $(this).click(function(ev) {
                ev.preventDefault();
                var trigger = $(this),
                    dd = $($(this).next('.dropdown-menu')),
                    isOpen = trigger.hasClass('dropdown-open');
                $("div.dropdown-menu").hide();
                $("a.trigger-dropdown").removeClass("dropdown-open");
                if (isOpen)
                    return false;
                dd.show();
                dd.css({
                    left: dd.hasClass('anchor-right') ?
                        trigger.position().left - (dd.outerWidth() - trigger.outerWidth())
                        : trigger.position().left 
                });
                trigger.addClass('dropdown-open');
                return false;
            });
        });
    };
    function hideDropdowns(event) {
		var targetGroup = event ? $(event.target).parents().andSelf() : null;
		if (targetGroup && targetGroup.is('.dropdown-menu') && !targetGroup.is('A'))
		    return;
		$('div.dropdown-menu').hide();
		$('a.trigger-dropdown').removeClass('dropdown-open');
        $("li.hideAlt").hide();
	};
	$(function () {
		$('HTML').on('click.dropdown', hideDropdowns);
		if( !$.browser.msie || ($.browser.msie && $.browser.version >= 9) ) {
			$(window).on('resize.dropdown', hideDropdowns);
		}
	    $(document).keydown(function(e) {
	        if (e.keyCode == 17 && $("a.trigger-dropdown").hasClass("dropdown-open")) {
	            $("li.hideAlt").toggle();
	        }
	    });
        $("li.hideAlt").hide();
	});
});