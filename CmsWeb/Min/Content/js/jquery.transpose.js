/*!
 * jQuery transpose() plugin
 *
 * Version 1.2 (2 Aug 2011)
 *
 * Copyright (c) 2011 Robert Koritnik
 * Licensed under the terms of the MIT license
 * http://www.opensource.org/licenses/mit-license.php
 */
(function(n){var t={containerKey:"transposed",itemKey:"pre-transpose-index"};n.fn.extend({transpose:function(){if(this.length>2){var r=this,i=this.parent();i.each(function(){var v=this,c=n(this),o,u,s,i,l,a,h,e,f;if(typeof c.data(t.containerKey)=="undefined"&&(c.data(t.containerKey,!0),o=r.filter(function(){return this.parentNode==v}),u=o.length,u>2)){for(l=o.eq(0).position().top,i=1;i<u&&l==o.eq(i).position().top;i++);if(a=parseInt(u/i)+1,u>i&&i>1){for(h=[],f=0;f<i;f++)h.push(a-(f%i<u%i?0:1));for(s=n(),e=0,f=0;f<u;f++)s=s.add(o.eq(e).detach().data(t.itemKey,e)),e+=h[f%i],e>=u&&e++,e=e%u;s.appendTo(c).after("\n")}}})}return this}})})(jQuery)