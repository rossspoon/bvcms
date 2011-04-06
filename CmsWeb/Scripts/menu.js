$(function () {
    $('.dropdown').hoverIntent(dropdownshow, dropdownhide);
});
function dropdownshow() {
    $('.sublinks').hide();
    if ($(this).hasClass('left')) {
        var left = $(this).offset().left +
            	$(this).width() - $('.sublinks', this).width();
        $('.sublinks', this).css("left", left + 'px')
    }
    $('.sublinks', this)
			.css("position", "absolute")
        	.show();
};
function dropdownhide() {
    $('.sublinks', this).hide();
};
