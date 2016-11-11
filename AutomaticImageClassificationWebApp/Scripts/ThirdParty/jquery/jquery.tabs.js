(function()
{
	  var methods = {
	    init: function(options)		
		{
			return this.each(function()
			{
				$("ul li", this).on('click',function(e)								
				{
					//e.stopPropagation();

					var $this = $(this);
					$ul = $this.parent();
					var $active = $("li.active", $ul);
					
					$active.removeClass("active");
					$this.addClass("active");
					
					$("#" + $("a", $active).attr("tab")).hide();
					$("#" + $("a", $this).attr("tab")).show();
				});
			});
		},
	    selectTab: function(tabIndex)		
		{			
			return $("ul li", this).each(function(index, el)
			{
				var $el = $(el);
				if ((index + 1) == tabIndex) 
				{
					$el.addClass("active");
					$("#" + $("a", $el).attr("tab")).show();
				}
				else 
				{
					$el.removeClass("active");
					$("#" + $("a", $el).attr("tab")).hide();
					
				}
			});		
		},
	  };
  
  
	  	jQuery.fn.tabs = function(method)
		{
			if (methods[method]) 
				return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
			else if (typeof method === 'object' || !method) 
				return methods.init.apply(this, arguments);
			else $.error('Method ' + method + ' does not exist on jQuery.tooltip');
		};
})();


(function()
{
	  var methods = {
	    init: function(options)		
		{
			return this.each(function()
			{
				$("ul li", this).on('click',function(e)								
				{
					//e.stopPropagation();
					var $this = $(this);
					$ul = $this.parent();
					var $active = $("li.active", $ul);
					
					$active.removeClass("active");
					$this.addClass("active");
				    //TODO find a better way to define which element to show hide
					$("." + $("a", $active).attr("tab"),$ul.parent().parent()).hide();
					$("." + $("a", $this).attr("tab"),$ul.parent().parent()).show();
				});
			});
		},
	    selectTab: function(tabIndex)		
		{			
			return $("ul li", this).each(function(index, el)
			{
				var $el = $(el);
				if ((index + 1) == tabIndex)
				{
					$el.addClass("active");
					$("." + $("a", $el).attr("tab")).show();
				}
				else 
				{
					$el.removeClass("active");
					$("." + $("a", $el).attr("tab")).hide();
					
				}
			});		
		},
	  };
  
  
	  	jQuery.fn.clmsTabs = function(method)
	  	{
			if (methods[method]) 
				return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
			else if (typeof method === 'object' || !method) 
				return methods.init.apply(this, arguments);
			else $.error('Method ' + method + ' does not exist on jQuery.tooltip');
		};
})();