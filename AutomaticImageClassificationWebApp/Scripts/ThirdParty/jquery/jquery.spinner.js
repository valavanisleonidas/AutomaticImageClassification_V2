(function()
{
	var methods = 
	{
		init: function(options)
		{
		    options = options || {};
			return (this).each(function(index, el)
			{
				var $spinInput = $(this).find("input.SpinInput");
				var $spinUp = $(this).find("input.SpinUp");
				var $spinDown = $(this).find("input.SpinDown");
				
				var $self=$(this);
				var isNumeric = function(value)
				{
					if (isNaN(value) || value == '') 
						return 0;
					else 
						return value;
				}
				
				var addVal = function()
				{
					var step = 1;
					currentVal = parseInt(isNumeric($spinInput.val()));
					$spinInput.val(currentVal + parseInt(step));
					$spinInput.trigger('change').trigger('blur').focus();
				}
				
				var subVal = function()
				{
					var step = 1;
					currentVal = parseInt(isNumeric($spinInput.val()));
					if (options.allowNegative === true) {
					    $spinInput.val(currentVal - parseInt(step));
					    $spinInput.trigger('change');
					    $spinInput.trigger('change').trigger('blur').focus();
					}else {
					    if (currentVal - parseInt(step) >= 0) {
					        $spinInput.val(currentVal - parseInt(step));
					        $spinInput.trigger('change');
					        $spinInput.trigger('change').trigger('blur').focus();
					    }
					}

				}
				
				
				var TestKeyPress = function(evt)
				{
					var curChar = String.fromCharCode(evt.which);
					var inpStr = evt.target.value + curChar
					evt.target.title = '';
					if (options.allowNegative === true && inpStr == "-") {
					    return;
					}
					result = (options.allowNegative === true) ? inpStr.match('^-?[0-9]+$') : inpStr.match('^[0-9]+$');
					if (!result && evt.which != 8 && evt.which != 0) 
					{
						evt.target.title = 'Please enter only numbers.';
						//evt.returnValue = false;
						//evt.cancel      = true;
						evt.preventDefault();
					}
				}
				
				$spinInput.bind('keypress', function(evt)
				{
					TestKeyPress(evt);
				});
				
				// keypress on special characters does not work on IE and Chrome
				$spinInput.bind('keydown', function(evt)
				{
					if (evt.keyCode == 38) 
					{ // panw belaki
						addVal();
						return;
					}
					if (evt.keyCode == 40) 
					{ // katw belaki
						subVal();
						return;
					}
				});
				
				$spinUp.mousedown(function(e)
				{
					addVal();
				});
				
				$spinDown.mousedown(function(e)
				{
					subVal();
				});
			});
		}
	}
	
	
	
	jQuery.fn.spinner = function(method)
	{
		if (methods[method]) 
			return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
		else 
			if (typeof method === 'object' || !method) 
				return methods.init.apply(this, arguments);
			else 
				$.error('Method ' + method + ' does not exist on jQuery.spinner');
	};
})();