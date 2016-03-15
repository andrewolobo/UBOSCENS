/*!
 * jQuery counter Plugin
 * http://kcounter.kudoslabs.co.uk
 * Copyright (c) 2012 Kudos
 * Version: 0.1
 * Dual licensed under the MIT and GPL licenses:
 * http://www.opensource.org/licenses/mit-license.php
 * http://www.gnu.org/licenses/gpl.html
 * Requires: jQuery v1.7 or later
 */
(function($){	  
		  
	var methods = {
		init : function( options ){ 
		
			// defaults
			settings = $.extend({
				height: 50,
				width: 50,
				initial: false,
				easing: 'swing',
				duration: 'fast'
			}, options);
			
			$(this).data('settings',settings) ;
			
			return this.each(function(){									  				
									  
				var $this = $(this);
				
				var initialContent = settings.initial ? settings.initial : $this.html() ; // get the initial content
				var chars = initialContent.toString().split("") ; // everything is a string!
				var html = "" ;				
					
				$.each(chars, function(index, value) { // build individual counters									
					if( $.isNumeric(value) ) {
						html += "<ul><li style=\"top:-"+(value*settings.height)+"px\">" ;
						for (var i = 0; i < 10; i++) {
							html += '<span>'+ i +'</span>' ;
						}				   
						html += "</li></ul>" ;				
					} else {
						html += '<ul><li><span>'+value+'</span></li></ul>' ;	
					}
				});
				$this.html(html) ;
				
				methods.updateCss.call($this, settings);
				
			}) ;
		
		},
		updateCss : function(settings) {
			
			return this.each(function(){
									  
				var $this = $(this);
				
				$this.css({
					'overflow' : 'hidden'
				}) ;
				$('ul', $this).css({
					'position' : 'relative',
					'float' : 'left',
					'overflow' : 'hidden',
					'height' : settings.height+'px',
					'width' : settings.width+'px',
					'line-height' : settings.height+'px'
				}) ;
				$('li', $this).css({
					'position' : 'absolute',
					'width' : settings.width+'px'
				}) ;
				$('span', $this).css({
					'display' : 'block',
					'text-align' : 'center',
					'height' : settings.height+'px',
					'width' : settings.width+'px',
				}) ;		  
			}) ;
		},
		update : function( content ) {
			
			return this.each(function(){
									  
				var $this = $(this);
				
				var chars = content.toString().split("") ;				
				var numberCounters = $('ul', $this).length ;
				var settings = $(this).data('settings');
				
				if(numberCounters!==chars.length){
					var diff = chars.length-numberCounters ;
					numberCounters = chars.length ;			
					
					// decide whether to add or remove values
					if( diff<0 ) {	
					
						$('ul', $this).slice(diff).remove() ;
						
					} else {
					
						var html = '' ;
						while (diff--) {
							html += "<ul><li>" ;
							for (var i = 0; i < 10; i++) {
								html += '<span>'+ i +'</span>' ;
							}				   
							html += "</li></ul>" ;							
						}
						$this.prepend(html) ;		
						
					}							
				}				
				
				$.each(chars, function(index, value){
					if( $.isNumeric(value) ) {
						if( $('ul:nth-child('+(index+1)+') li span', $this).length==1 ) { // if this value was previously a char change it
							var html = '' ;
							for (var i = 0; i < 10; i++) {
								html += '<span>'+ i +'</span>' ;
							}				   
							$('ul:nth-child('+(index+1)+') li', $this).html(html) ;
						}
						$('ul:nth-child('+(index+1)+') li', $this).animate({ top: -value * settings.height }, settings.duration, settings.easing ) ;	
					} else {
						$('ul:nth-child('+(index+1)+') li', $this).html("<span>"+value+"</span>").animate({top: 0}, settings.duration, settings.easing) ;
					}
				});
				
				methods.updateCss.call($this, settings);
				
			}) ;
		}
	};	  
		  
 	$.fn.kCounter = function( method ){
		
		if ( methods[method] ) {
		  return methods[method].apply( this, Array.prototype.slice.call( arguments, 1 ));
		} else if ( typeof method === 'object' || ! method ) {
		  return methods.init.apply( this, arguments );
		} else {
		  $.error( 'Method ' +  method + ' does not exist on jQuery.kCounter' );
		} 
  		
	};
})( jQuery );