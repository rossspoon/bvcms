//######
//## This work is licensed under the Creative Commons Attribution-Share Alike 3.0 
//## United States License. To view a copy of this license, 
//## visit http://creativecommons.org/licenses/by-sa/3.0/us/ or send a letter 
//## to Creative Commons, 171 Second Street, Suite 300, San Francisco, California, 94105, USA.
//######

(function($) {
    $.fn.idleTimeout = function(options) {
        var defaults = {
            inactivity: 1200000, //20 Minutes
            noconfirm: 10000, //10 Seconds
            redirect_url: '/js_sandbox/'
        }

        var opts = $.extend(defaults, options);
        var liveTimeout, confTimeout;
        var modal = "<div id='modal_pop'><p>You are about to be signed out due to inactivity.</p></div>";

        var start_liveTimeout = function() {
            clearTimeout(liveTimeout);
            clearTimeout(confTimeout);
            liveTimeout = setTimeout(logout, opts.inactivity);
        }

        var logout = function() {
            confTimeout = setTimeout(redirect, opts.noconfirm);
            $(modal).dialog({
                buttons: { "Stay Logged In": function() {
                    $(this).dialog('close');
                    stay_logged_in();
                }
                },
                modal: true,
                title: 'Auto Logout'
            });

        }

        var redirect = function() {
            window.location.href = opts.redirect_url;
        }

        var stay_logged_in = function(el) {
            start_liveTimeout();
        }

        return this.each(function() {
            obj = $(this);
            $(document).bind('click', start_liveTimeout);
        });
    };
})(jQuery);
