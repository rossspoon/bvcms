///#source 1 1 /Scripts/jquery/jquery.address-1.5.js
/*
 * jQuery Address Plugin v1.5
 * http://www.asual.com/jquery/address/
 *
 * Copyright (c) 2009-2010 Rostislav Hristov
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * Date: 2012-11-18 23:51:44 +0200 (Sun, 18 Nov 2012)
 */
(function ($) {

    $.address = (function () {

        var _trigger = function(name) {
               var ev = $.extend($.Event(name), 
                 (function() {
                            var parameters = {},
                                parameterNames = $.address.parameterNames();
                            for (var i = 0, l = parameterNames.length; i < l; i++) {
                                parameters[parameterNames[i]] = $.address.parameter(parameterNames[i]);
                            }
                            return {
                                value: $.address.value(),
                                path: $.address.path(),
                                pathNames: $.address.pathNames(),
                                parameterNames: parameterNames,
                                parameters: parameters,
                                queryString: $.address.queryString()
                            };
                        }).call($.address)
                    );

               $($.address).trigger(ev);
               return ev;
            },
            _array = function(obj) {
                return Array.prototype.slice.call(obj);
            },
            _bind = function(value, data, fn) {
                $().bind.apply($($.address), Array.prototype.slice.call(arguments));
                return $.address;
            },
            _unbind = function(value,  fn) {
                $().unbind.apply($($.address), Array.prototype.slice.call(arguments));
                return $.address;
            },
            _supportsState = function() {
                return (_h.pushState && _opts.state !== UNDEFINED);
            },
            _hrefState = function() {
                return ('/' + _l.pathname.replace(new RegExp(_opts.state), '') + 
                    _l.search + (_hrefHash() ? '#' + _hrefHash() : '')).replace(_re, '/');
            },
            _hrefHash = function() {
                var index = _l.href.indexOf('#');
                return index != -1 ? _crawl(_l.href.substr(index + 1), FALSE) : '';
            },
            _href = function() {
                return _supportsState() ? _hrefState() : _hrefHash();
            },
            _window = function() {
                try {
                    return top.document !== UNDEFINED && top.document.title !== UNDEFINED ? top : window;
                } catch (e) { 
                    return window;
                }
            },
            _js = function() {
                return 'javascript';
            },
            _strict = function(value) {
                value = value.toString();
                return (_opts.strict && value.substr(0, 1) != '/' ? '/' : '') + value;
            },
            _crawl = function(value, direction) {
                if (_opts.crawlable && direction) {
                    return (value !== '' ? '!' : '') + value;
                }
                return value.replace(/^\!/, '');
            },
            _cssint = function(el, value) {
                return parseInt(el.css(value), 10);
            },
            
            // Hash Change Callback
            _listen = function() {
                if (!_silent) {
                    var hash = _href(),
                        diff = decodeURI(_value) != decodeURI(hash);
                    if (diff) {
                        if (_msie && _version < 7) {
                            _l.reload();
                        } else {
                            if (_msie && !_hashchange && _opts.history) {
                                _st(_html, 50);
                            }
                            _old = _value;
                            _value = hash;
                            _update(FALSE);
                        }
                    }
                }
            },

            _update = function(internal) {
                var changeEv = _trigger(CHANGE),
                    xChangeEv = _trigger(internal ? INTERNAL_CHANGE : EXTERNAL_CHANGE);
                
                _st(_track, 10);

                if (changeEv.isDefaultPrevented() || xChangeEv.isDefaultPrevented()){
                  _preventDefault();
                }
            },

            _preventDefault = function(){
              _value = _old;
              
              if (_supportsState()) {
                  _h.popState({}, '', _opts.state.replace(/\/$/, '') + (_value === '' ? '/' : _value));
              } else {
                  _silent = TRUE;
                  if (_webkit) {
                      if (_opts.history) {
                          _l.hash = '#' + _crawl(_value, TRUE);
                      } else {
                          _l.replace('#' + _crawl(_value, TRUE));
                      }
                  } else if (_value != _href()) {
                      if (_opts.history) {
                          _l.hash = '#' + _crawl(_value, TRUE);
                      } else {
                          _l.replace('#' + _crawl(_value, TRUE));
                      }
                  }
                  if ((_msie && !_hashchange) && _opts.history) {
                      _st(_html, 50);
                  }
                  if (_webkit) {
                      _st(function(){ _silent = FALSE; }, 1);
                  } else {
                      _silent = FALSE;
                  }
              }
              
            },

            _track = function() {
                if (_opts.tracker !== 'null' && _opts.tracker !== NULL) {
                    var fn = $.isFunction(_opts.tracker) ? _opts.tracker : _t[_opts.tracker],
                        value = (_l.pathname + _l.search + 
                                ($.address && !_supportsState() ? $.address.value() : ''))
                                .replace(/\/\//, '/').replace(/^\/$/, '');
                    if ($.isFunction(fn)) {
                        fn(value);
                    } else if ($.isFunction(_t.urchinTracker)) {
                        _t.urchinTracker(value);
                    } else if (_t.pageTracker !== UNDEFINED && $.isFunction(_t.pageTracker._trackPageview)) {
                        _t.pageTracker._trackPageview(value);
                    } else if (_t._gaq !== UNDEFINED && $.isFunction(_t._gaq.push)) {
                        _t._gaq.push(['_trackPageview', decodeURI(value)]);
                    }
                }
            },
            _html = function() {
                var src = _js() + ':' + FALSE + ';document.open();document.writeln(\'<html><head><title>' + 
                    _d.title.replace(/\'/g, '\\\'') + '</title><script>var ' + ID + ' = "' + encodeURIComponent(_href()).replace(/\'/g, '\\\'') + 
                    (_d.domain != _l.hostname ? '";document.domain="' + _d.domain : '') + 
                    '";</' + 'script></head></html>\');document.close();';
                if (_version < 7) {
                    _frame.src = src;
                } else {
                    _frame.contentWindow.location.replace(src);
                }
            },
            _options = function() {
                if (_url && _qi != -1) {
                    var i, param, params = _url.substr(_qi + 1).split('&');
                    for (i = 0; i < params.length; i++) {
                        param = params[i].split('=');
                        if (/^(autoUpdate|crawlable|history|strict|wrap)$/.test(param[0])) {
                            _opts[param[0]] = (isNaN(param[1]) ? /^(true|yes)$/i.test(param[1]) : (parseInt(param[1], 10) !== 0));
                        }
                        if (/^(state|tracker)$/.test(param[0])) {
                            _opts[param[0]] = param[1];
                        }
                    }
                    _url = NULL;
                }
                _old = _value;
                _value = _href();
            },
            _load = function() {
                if (!_loaded) {
                    _loaded = TRUE;
                    _options();
                    var complete = function() {
                            _enable.call(this);
                            _unescape.call(this);
                        },
                        body = $('body').ajaxComplete(complete);
                    complete();
                    if (_opts.wrap) {
                        var wrap = $('body > *')
                            .wrapAll('<div style="padding:' + 
                                (_cssint(body, 'marginTop') + _cssint(body, 'paddingTop')) + 'px ' + 
                                (_cssint(body, 'marginRight') + _cssint(body, 'paddingRight')) + 'px ' + 
                                (_cssint(body, 'marginBottom') + _cssint(body, 'paddingBottom')) + 'px ' + 
                                (_cssint(body, 'marginLeft') + _cssint(body, 'paddingLeft')) + 'px;" />')
                            .parent()
                            .wrap('<div id="' + ID + '" style="height:100%;overflow:auto;position:relative;' + 
                                (_webkit && !window.statusbar.visible ? 'resize:both;' : '') + '" />');
                        $('html, body')
                            .css({
                                height: '100%',
                                margin: 0,
                                padding: 0,
                                overflow: 'hidden'
                            });
                        if (_webkit) {
                            $('<style type="text/css" />')
                                .appendTo('head')
                                .text('#' + ID + '::-webkit-resizer { background-color: #fff; }');
                        }
                    }
                    if (_msie && !_hashchange) {
                        var frameset = _d.getElementsByTagName('frameset')[0];
                        _frame = _d.createElement((frameset ? '' : 'i') + 'frame');
                        _frame.src = _js() + ':' + FALSE;
                        if (frameset) {
                            frameset.insertAdjacentElement('beforeEnd', _frame);
                            frameset[frameset.cols ? 'cols' : 'rows'] += ',0';
                            _frame.noResize = TRUE;
                            _frame.frameBorder = _frame.frameSpacing = 0;
                        } else {
                            _frame.style.display = 'none';
                            _frame.style.width = _frame.style.height = 0;
                            _frame.tabIndex = -1;
                            _d.body.insertAdjacentElement('afterBegin', _frame);
                        }
                        _st(function() {
                            $(_frame).bind('load', function() {
                                var win = _frame.contentWindow;
                                _old = _value;
                                _value = win[ID] !== UNDEFINED ? win[ID] : '';
                                if (_value != _href()) {
                                    _update(FALSE);
                                    _l.hash = _crawl(_value, TRUE);
                                }
                            });
                            if (_frame.contentWindow[ID] === UNDEFINED) {
                                _html();
                            }
                        }, 50);
                    }
                    _st(function() {
                        _trigger('init');
                        _update(FALSE);
                    }, 1);
                    if (!_supportsState()) {
                        if ((_msie && _version > 7) || (!_msie && _hashchange)) {
                            if (_t.addEventListener) {
                                _t.addEventListener(HASH_CHANGE, _listen, FALSE);
                            } else if (_t.attachEvent) {
                                _t.attachEvent('on' + HASH_CHANGE, _listen);
                            }
                        } else {
                            _si(_listen, 50);
                        }
                    }
                    if ('state' in window.history) {
                        $(window).trigger('popstate');
                    }
                }
            },
            _enable = function() {
                var el, 
                    elements = $('a'), 
                    length = elements.size(),
                    delay = 1,
                    index = -1,
                    sel = '[rel*="address:"]',
                    fn = function() {
                        if (++index != length) {
                            el = $(elements.get(index));
                            if (el.is(sel)) {
                                el.address(sel);
                            }
                            _st(fn, delay);
                        }
                    };
                _st(fn, delay);
            },
            _popstate = function() {
                if (decodeURI(_value) != decodeURI(_href())) {
                    _old = _value;
                    _value = _href();
                    _update(FALSE);
                }
            },
            _unload = function() {
                if (_t.removeEventListener) {
                    _t.removeEventListener(HASH_CHANGE, _listen, FALSE);
                } else if (_t.detachEvent) {
                    _t.detachEvent('on' + HASH_CHANGE, _listen);
                }
            },
            _unescape = function() {
                if (_opts.crawlable) {
                    var base = _l.pathname.replace(/\/$/, ''),
                        fragment = '_escaped_fragment_';
                    if ($('body').html().indexOf(fragment) != -1) {
                        $('a[href]:not([href^=http]), a[href*="' + document.domain + '"]').each(function() {
                            var href = $(this).attr('href').replace(/^http:/, '').replace(new RegExp(base + '/?$'), '');
                            if (href === '' || href.indexOf(fragment) != -1) {
                                $(this).attr('href', '#' + encodeURI(decodeURIComponent(href.replace(new RegExp('/(.*)\\?' + 
                                    fragment + '=(.*)$'), '!$2'))));
                            }
                        });
                    }
                }
            },
            UNDEFINED,
            NULL = null,
            ID = 'jQueryAddress',
            STRING = 'string',
            HASH_CHANGE = 'hashchange',
            INIT = 'init',
            CHANGE = 'change',
            INTERNAL_CHANGE = 'internalChange',
            EXTERNAL_CHANGE = 'externalChange',
            TRUE = true,
            FALSE = false,
            _opts = {
                autoUpdate: TRUE, 
                crawlable: FALSE,
                history: TRUE, 
                strict: TRUE,
                wrap: FALSE
            },
            _browser = $.browser, 
            _version = parseFloat(_browser.version),
            _msie = !$.support.opacity,
            _webkit = _browser.webkit || _browser.safari,
            _t = _window(),
            _d = _t.document,
            _h = _t.history, 
            _l = _t.location,
            _si = setInterval,
            _st = setTimeout,
            _re = /\/{2,9}/g,
            _agent = navigator.userAgent,
            _hashchange = 'on' + HASH_CHANGE in _t,
            _frame,
            _form,
            _url = $('script:last').attr('src'),
            _qi = _url ? _url.indexOf('?') : -1,
            _title = _d.title, 
            _silent = FALSE,
            _loaded = FALSE,
            _juststart = TRUE,
            _updating = FALSE,
            _listeners = {}, 
            _value = _href();
            _old = _value;
            
        if (_msie) {
            _version = parseFloat(_agent.substr(_agent.indexOf('MSIE') + 4));
            if (_d.documentMode && _d.documentMode != _version) {
                _version = _d.documentMode != 8 ? 7 : 8;
            }
            var pc = _d.onpropertychange;
            _d.onpropertychange = function() {
                if (pc) {
                    pc.call(_d);
                }
                if (_d.title != _title && _d.title.indexOf('#' + _href()) != -1) {
                    _d.title = _title;
                }
            };
        }
        
        if (_h.navigationMode) {
            _h.navigationMode = 'compatible';
        }
        if (document.readyState == 'complete') {
            var interval = setInterval(function() {
                if ($.address) {
                    _load();
                    clearInterval(interval);
                }
            }, 50);
        } else {
            _options();
            $(_load);
        }
        $(window).bind('popstate', _popstate).bind('unload', _unload);

        return {
            bind: function(type, data, fn) {
                return _bind.apply(this, _array(arguments));
            },
            unbind: function(type, fn) {
                return _unbind.apply(this, _array(arguments));
            },
            init: function(data, fn) {
                return _bind.apply(this, [INIT].concat(_array(arguments)));
            },
            change: function(data, fn) {
                return _bind.apply(this, [CHANGE].concat(_array(arguments)));
            },
            internalChange: function(data, fn) {
                return _bind.apply(this, [INTERNAL_CHANGE].concat(_array(arguments)));
            },
            externalChange: function(data, fn) {
                return _bind.apply(this, [EXTERNAL_CHANGE].concat(_array(arguments)));
            },
            baseURL: function() {
                var url = _l.href;
                if (url.indexOf('#') != -1) {
                    url = url.substr(0, url.indexOf('#'));
                }
                if (/\/$/.test(url)) {
                    url = url.substr(0, url.length - 1);
                }
                return url;
            },
            autoUpdate: function(value) {
                if (value !== UNDEFINED) {
                    _opts.autoUpdate = value;
                    return this;
                }
                return _opts.autoUpdate;
            },
            crawlable: function(value) {
                if (value !== UNDEFINED) {
                    _opts.crawlable = value;
                    return this;
                }
                return _opts.crawlable;
            },
            history: function(value) {
                if (value !== UNDEFINED) {
                    _opts.history = value;
                    return this;
                }
                return _opts.history;
            },
            state: function(value) {
                if (value !== UNDEFINED) {
                    _opts.state = value;
                    var hrefState = _hrefState();
                    if (_opts.state !== UNDEFINED) {
                        if (_h.pushState) {
                            if (hrefState.substr(0, 3) == '/#/') {
                                _l.replace(_opts.state.replace(/^\/$/, '') + hrefState.substr(2));
                            }
                        } else if (hrefState != '/' && hrefState.replace(/^\/#/, '') != _hrefHash()) {
                            _st(function() {
                                _l.replace(_opts.state.replace(/^\/$/, '') + '/#' + hrefState);
                            }, 1);
                        }
                    }
                    return this;
                }
                return _opts.state;
            },
            strict: function(value) {
                if (value !== UNDEFINED) {
                    _opts.strict = value;
                    return this;
                }
                return _opts.strict;
            },
            tracker: function(value) {
                if (value !== UNDEFINED) {
                    _opts.tracker = value;
                    return this;
                }
                return _opts.tracker;
            },
            wrap: function(value) {
                if (value !== UNDEFINED) {
                    _opts.wrap = value;
                    return this;
                }
                return _opts.wrap;
            },
            update: function() {
                _updating = TRUE;
                this.value(_value);
                _updating = FALSE;
                return this;
            },
            title: function(value) {
                if (value !== UNDEFINED) {
                    _st(function() {
                        _title = _d.title = value;
                        if (_juststart && _frame && _frame.contentWindow && _frame.contentWindow.document) {
                            _frame.contentWindow.document.title = value;
                            _juststart = FALSE;
                        }
                    }, 50);
                    return this;
                }
                return _d.title;
            },
            value: function(value) {
                if (value !== UNDEFINED) {
                    value = _strict(value);
                    if (value == '/') {
                        value = '';
                    }
                    if (_value == value && !_updating) {
                        return;
                    }
                    _old = _value;
                    _value = value;
                    if (_opts.autoUpdate || _updating) {
                        _update(TRUE);
                        if (_supportsState()) {
                            _h[_opts.history ? 'pushState' : 'replaceState']({}, '', 
                                    _opts.state.replace(/\/$/, '') + (_value === '' ? '/' : _value));
                        } else {
                            _silent = TRUE;
                            if (_webkit) {
                                if (_opts.history) {
                                    _l.hash = '#' + _crawl(_value, TRUE);
                                } else {
                                    _l.replace('#' + _crawl(_value, TRUE));
                                }
                            } else if (_value != _href()) {
                                if (_opts.history) {
                                    _l.hash = '#' + _crawl(_value, TRUE);
                                } else {
                                    _l.replace('#' + _crawl(_value, TRUE));
                                }
                            }
                            if ((_msie && !_hashchange) && _opts.history) {
                                _st(_html, 50);
                            }
                            if (_webkit) {
                                _st(function(){ _silent = FALSE; }, 1);
                            } else {
                                _silent = FALSE;
                            }
                        }
                    }
                    return this;
                }
                return _strict(_value);
            },
            path: function(value) {
                if (value !== UNDEFINED) {
                    var qs = this.queryString(),
                        hash = this.hash();
                    this.value(value + (qs ? '?' + qs : '') + (hash ? '#' + hash : ''));
                    return this;
                }
                return _strict(_value).split('#')[0].split('?')[0];
            },
            pathNames: function() {
                var path = this.path(),
                    names = path.replace(_re, '/').split('/');
                if (path.substr(0, 1) == '/' || path.length === 0) {
                    names.splice(0, 1);
                }
                if (path.substr(path.length - 1, 1) == '/') {
                    names.splice(names.length - 1, 1);
                }
                return names;
            },
            queryString: function(value) {
                if (value !== UNDEFINED) {
                    var hash = this.hash();
                    this.value(this.path() + (value ? '?' + value : '') + (hash ? '#' + hash : ''));
                    return this;
                }
                var arr = _value.split('?');
                return arr.slice(1, arr.length).join('?').split('#')[0];
            },
            parameter: function(name, value, append) {
                var i, params;
                if (value !== UNDEFINED) {
                    var names = this.parameterNames();
                    params = [];
                    value = value === UNDEFINED || value === NULL ? '' : value.toString();
                    for (i = 0; i < names.length; i++) {
                        var n = names[i],
                            v = this.parameter(n);
                        if (typeof v == STRING) {
                            v = [v];
                        }
                        if (n == name) {
                            v = (value === NULL || value === '') ? [] : 
                                (append ? v.concat([value]) : [value]);
                        }
                        for (var j = 0; j < v.length; j++) {
                            params.push(n + '=' + v[j]);
                        }
                    }
                    if ($.inArray(name, names) == -1 && value !== NULL && value !== '') {
                        params.push(name + '=' + value);
                    }
                    this.queryString(params.join('&'));
                    return this;
                }
                value = this.queryString();
                if (value) {
                    var r = [];
                    params = value.split('&');
                    for (i = 0; i < params.length; i++) {
                        var p = params[i].split('=');
                        if (p[0] == name) {
                            r.push(p.slice(1).join('='));
                        }
                    }
                    if (r.length !== 0) {
                        return r.length != 1 ? r : r[0];
                    }
                }
            },
            parameterNames: function() {
                var qs = this.queryString(),
                    names = [];
                if (qs && qs.indexOf('=') != -1) {
                    var params = qs.split('&');
                    for (var i = 0; i < params.length; i++) {
                        var name = params[i].split('=')[0];
                        if ($.inArray(name, names) == -1) {
                            names.push(name);
                        }
                    }
                }
                return names;
            },
            hash: function(value) {
                if (value !== UNDEFINED) {
                    this.value(_value.split('#')[0] + (value ? '#' + value : ''));
                    return this;
                }
                var arr = _value.split('#');
                return arr.slice(1, arr.length).join('#');                
            }
        };
    })();
    
    $.fn.address = function(fn) {
        var sel;
        if (typeof fn == 'string') {
            sel = fn;
            fn = undefined;
        }
        if (!$(this).attr('address')) {
            var f = function(e) {
                if (e.shiftKey || e.ctrlKey || e.metaKey || e.which == 2) {
                    return true;
                }
                if ($(this).is('a')) {
                    e.preventDefault();
                    var value = fn ? fn.call(this) : 
                        /address:/.test($(this).attr('rel')) ? $(this).attr('rel').split('address:')[1].split(' ')[0] : 
                        $.address.state() !== undefined && !/^\/?$/.test($.address.state()) ? 
                                $(this).attr('href').replace(new RegExp('^(.*' + $.address.state() + '|\\.)'), '') : 
                                $(this).attr('href').replace(/^(#\!?|\.)/, '');
                    $.address.value(value);
                }
            };
            $(sel ? sel : this).live('click', f).live('submit', function(e) {
                if ($(this).is('form')) {
                    e.preventDefault();
                    var action = $(this).attr('action'),
                        value = fn ? fn.call(this) : (action.indexOf('?') != -1 ? action.replace(/&$/, '') : action + '?') + 
                            $(this).serialize();
                    $.address.value(value);
                }
            }).attr('address', true);
        }
        return this;
    };
    
})(jQuery);

///#source 1 1 /Scripts/People/person1.js
$(function () {
    //    $('#dialogbox').dialog({
    //        title: 'Search Dialog',
    //        bgiframe: true,
    //        autoOpen: false,
    //        width: 700,
    //        height: 630,
    //        modal: true,
    //        overlay: {
    //            opacity: 0.5,
    //            background: "black"
    //        }, close: function () {
    //            $('iframe', this).attr("src", "");
    //        }
    //    });
    $('#position').editable({
        source: [{
            value: 10,
            text: "Primary Adult"
        }, {
            value: 20,
            text: "Secondary Adult"
        }, {
            value: 30,
            text: "Child"
        }],
        type: "select",
        url: "/Person2/PostData",
        name: "position"
    });

    $("a.editfamily").live("click", function(ev) {
        ev.stopPropagation();
        ev.preventDefault();
        $(this).closest('div.open').removeClass('open');
        $(this).closest("li.relation-item").find("span.relation-description").editable("toggle");
    });
    $('span.relation-description').editable({
        type: "textarea",
        toggle: "manual",
        name: "description",
        url: function(params) {
            var d = new $.Deferred;
            $.post('/Person2/EditRelation/' + params.pk, {value: params.value}, function(data) {
                d.resolve();
            });
            return d.promise();
        }
    });
            //return d.promise();
    $("#clipaddr").live('click', function () {
        var inElement = $('#addrhidden')[0];
        if (inElement.createTextRange) {
            var range = inElement.createTextRange();
            if (range)
                range.execCommand('Copy');
        }
        return false;
    });
    $('#deleteperson').click(function () {
        var href = $(this).attr("href");
        if (confirm('Are you sure you want to delete?')) {
            $.post(href, null, function (ret) {
                if (ret != "ok") {
                    $.block("delete Failed: " + ret);
                    $('.blockOverlay').attr('title', 'Click to unblock').click($.unblock);
                }
                else {
                    $.block("person deleted");
                    $('.blockOverlay').attr('title', 'Click to unblock').click(function () {
                        $.unblock();
                        window.location = "/";
                    });
                }
            });
        }
        return false;
    });
    $('a.deloptout').live("click", function (ev) {
        ev.preventDefault();
        var href = $(this).attr("href");
        if (confirm('Are you sure you want to delete?')) {
            $.post(href, {}, function (ret) {
                if (ret != "ok")
                    $.growlUI("failed", ret);
                else {
                    $.updateTable($('#user-tab form'));
                    $.growlUI("Success", "OptOut deleted");
                }
            });
        }
    });
    $('#moveperson').click(function (ev) {
        ev.preventDefault();
        var d = $('#dialogbox');
        $('iframe', d).attr("src", this.href);
        d.dialog("option", "title", "Merge To Person");
        d.dialog("open");
        return false;
    });

    //    $.editable.addInputType('datepicker', {
    //        element: function (settings, original) {
    //            var input = $('<input>');
    //            if (settings.width != 'none') { input.width(settings.width); }
    //            if (settings.height != 'none') { input.height(settings.height); }
    //            input.attr('autocomplete', 'off');
    //            $(this).append(input);
    //            return (input);
    //        },
    //        plugin: function (settings, original) {
    //            var form = this;
    //            settings.onblur = 'ignore';
    //            $(this).find('input').datepicker().bind('click', function () {
    //                $(this).datepicker('show');
    //                return false;
    //            }).bind('dateSelected', function (e, selectedDate, $td) {
    //                $(form).submit();
    //            });
    //        }
    //    });
    //    $.editable.addInputType("multiselect", {
    //        element: function (settings, original) {
    //            var select = $('<select multiple="multiple" />');
    //
    //            if (settings.width != 'none') { select.width(settings.width); }
    //            if (settings.size) { select.attr('size', settings.size); }
    //
    //            $(this).append(select);
    //            return (select);
    //        },
    //        content: function (json, settings, original) {
    //            for (var key in json) {
    //                var option = $('<option />').val(key).text(key);
    //                if (json[key] == true)
    //                    option.attr("selected", true);
    //                $('select', this).append(option);
    //            }
    //            $("select", this).multiselect({
    //                close: function (event, ui) {
    //                    var values = $("select").val();
    //                },
    //                position: {
    //                    my: 'left bottom',
    //                    at: 'left top'
    //                }
    //            });
    //        }
    //    });
    //    $.extraEditable = function (table) {
    //        $('.editarea', table).editable('/Person/EditExtra/', {
    //            type: 'textarea',
    //            submit: 'OK',
    //            rows: 10,
    //            width: 600,
    //            indicator: '<img src="/images/loading.gif">',
    //            tooltip: 'Click to edit...'
    //        });
    //        $(".clickEdit", table).editable("/Person/EditExtra/", {
    //            indicator: "<img src='/images/loading.gif'>",
    //            tooltip: "Click to edit...",
    //            style: 'display: inline',
    //            width: '300px',
    //            height: 25,
    //            submit: 'OK'
    //        });
    //        $(".clickDatepicker").editable('/Person/EditExtra/', {
    //            type: 'datepicker',
    //            tooltip: 'Click to edit...',
    //            style: 'display: inline',
    //            width: '300px',
    //            submit: 'OK'
    //        });
    //        $(".clickSelect", table).editable("/Person/EditExtra/", {
    //            indicator: '<img src="/images/loading.gif">',
    //            loadurl: "/Person/ExtraValues/",
    //            loadtype: "POST",
    //            type: "select",
    //            submit: "OK",
    //            style: 'display: inline'
    //        });
    //        $(".clickCheckbox", table).editable('/Person/EditExtra', {
    //            type: 'checkbox',
    //            onblur: 'ignore',
    //            submit: 'OK'
    //        });
    //        $('.clickMultiselect', table).editable('/Person/EditExtra', {
    //            indicator: '<img src="/images/loading.gif">',
    //            loadurl: "/Person/ExtraValues2/",
    //            loadtype: "POST",
    //            type: "multiselect",
    //            submit: "OK",
    //            onblur: 'ignore',
    //            style: 'display: inline'
    //        });
    //    };
    $.getTable = function (f) {
        var q = f.serialize();
        $.post(f.attr('action'), q, function (ret) {
            $(f).html(ret).ready(function () {
                $.setClickOvers();
                //$.extraEditable('#extravalues');
            });
        });
        return false;
    };
    //        $('#memberDialog').dialog({
    //            title: 'Member Dialog',
    //            bgiframe: true,
    //            autoOpen: false,
    //            width: 600,
    //            height: 550,
    //            modal: true,
    //            overlay: {
    //                opacity: 0.5,
    //                background: "black"
    //            }, close: function () {
    //                $('iframe', this).attr("src", "");
    //            }
    //        });
    //        $('#previous-tab form a.membertype').live("click", function (e) {
    //            e.preventDefault();
    //            var d = $('#memberDialog');
    //            $('iframe', d).attr("src", this.href);
    //            d.dialog("open");
    //        });

    $(".CreateAndGo").click(function () {
        if (confirm($(this).attr("confirm")))
            $.post($(this).attr("href"), null, function (ret) {
                window.location = ret;
            });
        return false;
    });

    $("a.editaddr").click(function (ev) {
        ev.preventDefault();
        $("<div class='modal fade hide' />").load($(this).attr("href"), {}, function () {
            $(this).modal("show"); 
        });
    });
    $("a.close-saved-address").live("click", function() {
        $("#primaryaddress").html($("#primaryaddressnew").html());
        var target = $("#addressnew").data("target");
        $("#" + target).html($("#addressnew").html());
    });

$.setClickOvers = function () {
    $('tr a').not('a.evlink').click(function (e) {
        e.stopPropagation();
    });
    $('form a.membertype').click(function (e) {
        e.preventDefault();
        e.stopPropagation();
        $("#OrgMemberDialog").modal({ remote: $(this).attr("href") });
    });
    $("tr.section.notshown").live("click", function (ev) {
        ev.preventDefault();
        $(this).removeClass("notshown").addClass("shown");
        $(this).nextUntil("tr.section").find("div.collapse").collapse('show');
    });
    $("tr.section.shown").live("click", function (ev) {
        ev.preventDefault();
        $(this).nextUntil("tr.section").find("div.collapse").collapse('hide');
        $(this).removeClass("shown").addClass("notshown");
    });
    $('a[rel="reveal"]').click(function (ev) {
        ev.preventDefault();
        $(this).parents("tr").next("tr").find("div.collapse").collapse('toggle');
    });
    $('tr.organization').click(function (ev) {
        ev.preventDefault();
        $(this).next("tr").find("div.collapse").collapse('toggle');
    });
    $('tr.details').click(function (ev) {
        ev.preventDefault();
        $(this).find("div.collapse").collapse('hide');
    });
};
$("#currentLink").click(function () {
    $.showTable($('#current form'));
});
$("#previousLink").click(function () {
    $.showTable($('#previous form'));
});
$("#pendingLink").click(function () {
    $.showTable($('#pending form'));
});
$("#attendanceLink").click(function () {
    $.showTable($('#attendance-tab form'));
});
$("#contacts-link").click(function () {
    $("#contacts-tab form").each(function () {
        $.showTable($(this));
    });
});
$("#member-link").click(function () {
    var f = $("#memberdisplay");
    if ($("table", f).size() == 0) {
        $.post(f.attr('action'), null, function (ret) {
            $(f).html(ret).ready(function () {
                $.UpdateForSection(f);
            });
        });
        $.showTable($("#extras-tab form"));
        $.extraEditable('#extravalues');
    }
});
$("#system-link").click(function () {
    $.showTable($("#user-tab form"));
});
$("#changes-link").click(function () {
    $.showTable($("#changes-tab form"));
});
$("#volunteer-link").click(function () {
    $.showTable($("#volunteer-tab form"));
});
$("#duplicates-link").click(function () {
    $.showTable($("#duplicates-tab form"));
});
$("#optouts-link").click(function () {
    $.showTable($("#optouts-tab form"));
});
$('#family table.grid > tbody > tr:even').addClass('alt');
$("#recreg-link").click(function (ev) {
    ev.preventDefault();
    var f = $('#recreg-tab form');
    if ($('table', f).size() > 0)
        return false;
    var q = f.serialize();
    $.post(f.attr('action'), q, function (ret) {
        $(f).html(ret);
        $(".bt", f).button();
    });
    return false;
});

$("a.displayedit").live('click', function (ev) {
    ev.preventDefault();
    var f = $(this).closest('form');
    $.post($(this).attr('href'), null, function (ret) {
        $(f).html(ret).ready(function () {
            $.UpdateForSection(f);
        });
    });
    return false;
});
$.options = function (url) {
    return {
        minLength: 3,
        source: function (query, process) {
            return $.ajax({
                url: url,
                type: 'post',
                data: { query: query },
                dataType: 'json',
                success: function (jsonResult) {
                    return typeof jsonResult == 'undefined' ? false : process(jsonResult);
                }
            });
        }
    };
};
$.UpdateForSection = function (f) {
    $('#Employer').typeahead($.options("/Person2/Employers"));
    $('#School', f).typeahead($.options("/Person2/Schools"));
    $('#Occupation', f).typeahead($.options("/Person2/Occupations"));
    $('#NewChurch', f).typeahead($.options("/Person2/Churches"));
    $('#PrevChurch', f).typeahead($.options("/Person2/Churches"));
    $("form select").chosen();
    moment.lang('en');
    $(".date").datepicker();
    return false;
};
$("form.DisplayEdit a.submitbutton").live('click', function (ev) {
    ev.preventDefault();
    var f = $(this).closest('form');
    if (!$(f).valid())
        return false;
    var q = f.serialize();
    $.post($(this).attr('href'), q, function (ret) {
        $(f).html(ret).ready(function () {
            var bc = $('#businesscard');
            $.post($(bc).attr("href"), null, function (ret) {
                $(bc).html(ret);
            });
            $(".submitbutton,.bt").button();
        });
    });
    return false;
});
$("form").on('click', '#future', function (ev) {
    ev.preventDefault();
    var f = $(this).closest('form');
    var q = f.serialize();
    $.post($(f).attr("action"), q, function (ret) {
        $(f).html(ret);
    });
});
$("form.DisplayEdit").submit(function () {
    if (!$("#submitit").val())
        return false;
    return true;
});
$.validator.addMethod("date2", function (value, element, params) {
    var v = $.DateValid(value);
    return this.optional(element) || v;
}, $.format("Please enter valid date"));

$.validator.setDefaults({
    highlight: function (input) {
        $(input).addClass("ui-state-highlight");
    },
    unhighlight: function (input) {
        $(input).removeClass("ui-state-highlight");
    },
    rules: {
        "NickName": { maxlength: 15 },
        "Title": { maxlength: 10 },
        "First": { maxlength: 25 },
        "Middle": { maxlength: 15 },
        "Last": { maxlength: 100, required: true },
        "Suffix": { maxlength: 10 },
        "AltName": { maxlength: 100 },
        "Maiden": { maxlength: 20 },
        "HomePhone": { maxlength: 20 },
        "CellPhone": { maxlength: 20 },
        "WorkPhone": { maxlength: 20 },
        "EmailAddress": { maxlength: 150 },
        "School": { maxlength: 60 },
        "Employer": { maxlength: 60 },
        "Occupation": { maxlength: 60 },
        "WeddingDate": { date2: true },
        "DeceasedDate": { date2: true },
        "Grade": { number: true },
        "Address1": { maxlength: 40 },
        "Address2": { maxlength: 40 },
        "City": { maxlength: 30 },
        "Zip": { maxlength: 15 },
        "FromDt": { date2: true },
        "ToDt": { date2: true },
        "DecisionDate": { date2: true },
        "JoinDate": { date2: true },
        "BaptismDate": { date2: true },
        "BaptismSchedDate": { date2: true },
        "DropDate": { date2: true },
        "NewMemberClassDate": { date2: true }
    }
});
$('#addrf').validate();
$('#addrp').validate();
$('#basic').validate();
$("body").on("change", '.atck', function (ev) {
    var ck = $(this);
    $.post("/Meeting/MarkAttendance/", {
        MeetingId: $(this).attr("mid"),
        PeopleId: $(this).attr("pid"),
        Present: ck.is(':checked')
    }, function (ret) {
        if (ret.error) {
            ck.attr("checked", !ck.is(':checked'));
            alert(ret.error);
        }
        else {
            var f = ck.closest('form');
            var q = f.serialize();
            $.post($(f).attr("action"), q, function (ret) {
                $(f).html(ret);
            });
        }
    });
});
//    $("#newvalueform").dialog({
//        autoOpen: false,
//        buttons: {
//            "Ok": function () {
//                var v = $("input[name='typeval']:checked").val();
//                var fn = $("#fieldname").val();
//                var va = $("#fieldvalue").val();
//                if (fn)
//                    $.post("/Person/NewExtraValue/" + $("#PeopleId").val(), { field: fn, type: v, value: va }, function (ret) {
//                        if (ret.startsWith("error"))
//                            alert(ret);
//                        else {
//                            $.getTable($("#extras-tab form"));
//                            $.extraEditable('#extravalues');
//                        }
//                        $("#fieldname").val("");
//                        $("#fieldvalue").val("");
//                    });
//                $(this).dialog("close");
//            }
//        }
//    });
//    $("body").on("click", '#newextravalue', function (ev) {
//        ev.preventDefault();
//        var d = $('#newvalueform');
//        d.dialog("open");
//    });
$("body").on("click", 'a.deleteextra', function (ev) {
    ev.preventDefault();
    if (confirm("are you sure?"))
        $.post("/Person/DeleteExtra/" + $("#PeopleId").val(), { field: $(this).attr("field") }, function (ret) {
            if (ret.startsWith("error"))
                alert(ret);
            else {
                $.getTable($("#extras-tab form"));
                $.extraEditable('#extravalues');
            }
        });
    return false;
});
$("form").on('click', 'a.reverse', function (ev) {
    ev.preventDefault();
    var f = $(this).closest('form');
    $.post("/Person/Reverse", {
        id: $("#PeopleId").val(),
        field: $(this).attr("field"),
        value: $(this).attr("value"),
        pf: $(this).attr("pf")
    }, function (ret) {
        $(f).html(ret);
    });
});
//    $.editable.addInputType("checkbox", {
//        element: function (settings, original) {
//            var input = $('<input type="checkbox">');
//            $(this).append(input);
//            $(input).click(function () {
//                var value = $(input).attr("checked") ? 'True' : 'False';
//                $(input).val(value);
//            });
//            return (input);
//        },
//        content: function (string, settings, original) {
//            var checked = string == "True" ? true : false;
//            var input = $(':input:first', this);
//            $(input).attr("checked", checked);
//            var value = $(input).attr("checked") ? 'True' : 'False';
//            $(input).val(value);
//        }
//    });
$('#vtab>ul>li').click(function () {
    $('#vtab>ul>li').removeClass('selected');
    $(this).addClass('selected');
    var index = $('#vtab>ul>li').index($(this));
    $('#vtab>div').hide().eq(index).show();
});

//$.editable.addInputType("multiselect", {
//    element: function(settings, original) {
//        var textarea = $('<select />');
//        if (settings.rows) {
//            textarea.attr('rows', settings.rows);
//        } else {
//            textarea.height(settings.height);
//        }
//        if (settings.cols) {
//            textarea.attr('cols', settings.cols);
//        } else {
//            textarea.width(settings.width);
//        }
//        $(this).append(textarea);
//        return (textarea);
//    },
//    plugin: function(settings, original) {
//        $('textarea', this).multiselect();
//    },
//    submit: function(settings, original) {
//        var value = $('#hour_').val() + ':' + $('#min_').val();
//        $('input', this).val(value);
//    }
//});

});
function RebindMemberGrids() {
    $.updateTable($('#current-tab form'));
    $.updateTable($('#pending-tab form'));
    $("#memberDialog").dialog('close');
}
function RebindUserInfoGrid() {
    $.updateTable($('#user-tab form'));
    $("#memberDialog").dialog('close');
}
function AddSelected(ret) {
    window.location = "/Merge?PeopleId1=" + $("#PeopleId").val() + "&PeopleId2=" + ret.pid;
}
function dialogError(arg) {
    return arg;
}
///#source 1 1 /Scripts/People/SearchAdd2.js
$(function () {
    $("a.xsearchadd").click(function (ev) {
        ev.preventDefault();
        var href = $(this).attr("href");
        $('<div/>').dialog3({
            id: "search-add",
            content: href,
            type: "POST"
        });
    });
    $("a.searchadd").click(function (ev) {
        ev.preventDefault();
        $("<div id='search-add' class='modal fade hide' data-width='600' />")
            .load($(this).attr("href"), {}, function () {
            $(this).modal("show");
        });
    });
//    $("#search-add a.submit-post").live("click", function (ev) {
//        ev.preventDefault();
//        var f = $(this).closest("form");
//        var q = f.serialize();
//        $("#search-add").dialog2("close");
//        $.post($(this).attr("href"), q, function (ret) {
//            $(ret).dialog2({ id: "search-add" });
//        });
//    });
    $("#search-add a.commit").live("click", function (ev) {
        ev.preventDefault();
        var f = $(this).closest("form");
        var q = f.serialize();
        var loc = $(this).attr("href");
        $.block();
        $.post(loc, q, function (ret) {
            if (ret.close) {
                if (ret.message) {
                    alert(ret.message);
                }
                switch (ret.how) {
                    case 'rebindgrids':
                        if (self.parent.RebindMemberGrids)
                            self.parent.RebindMemberGrids();
                        break;
                    case 'addselected':
                        if (self.parent.AddSelected)
                            self.parent.AddSelected(ret);
                        break;
                    case 'addselected2':
                        if (self.parent.AddSelected2)
                            self.parent.AddSelected2(ret);
                        break;
                    case 'CloseAddDialog':
                        if (self.parent.CloseAddDialog)
                            self.parent.CloseAddDialog();
                        break;
                }
            }
            $.unblock();
        });
        return false;
    });
    $("a.clear").live('click', function (ev) {
        ev.preventDefault();
        $("#name").val('');
        $("#phone").val('');
        $("#address").val('');
        $("#dob").val('');
        return false;
    });
});


