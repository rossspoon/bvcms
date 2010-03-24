(function($){

$.idleTimer = function(newTimeout){
    var idle    = false,        //indicates if the user is idle
        enabled = true,        //indicates if the idle timer is enabled
        timeout = 30000,        //the amount of time (ms) before the user is considered idle
        events  = 'mousemove keydown DOMMouseScroll mousewheel mousedown', // activity is one of these events
    toggleIdleState = function(){
        idle = !idle;
        $(document).trigger((idle ? "idle" : "active") + '.idleTimer');            
    },
    isRunning = function(){
        return enabled;
    },
    isIdle = function(){
        return idle;
    },
    stop = function(){
        enabled = false;
        clearTimeout($.idleTimer.tId);
        $(document).unbind('.idleTimer');
    },
    handleUserEvent = function(){
        clearTimeout($.idleTimer.tId);
        if (enabled){
            if (idle){
                toggleIdleState();           
            } 
            $.idleTimer.tId = setTimeout(toggleIdleState, timeout);
        }    
     };
    if (typeof newTimeout == "number"){
        timeout = newTimeout;
    } else if (newTimeout === 'destroy') {
        stop();
        return;  
    }
    $(document).bind($.trim((events+' ').split(' ').join('.idleTimer ')),handleUserEvent);
    $.idleTimer.tId = setTimeout(toggleIdleState, timeout);
};
})(jQuery);