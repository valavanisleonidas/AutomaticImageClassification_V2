(function()
{
	/*
	 We only bind arrays to tables at the moment
	 */
	jQuery.fn.databind = function(data, controller, settings)
	{
		settings = jQuery.extend(
		{
			allowChanges: true
		}, settings);
		var initial = this;
		
		//Assign Click handler on insert on header
		var thead = jQuery(this).find('thead').find('[class*=field]').each(function()
		{
			var t = this;
			
			// get the actual property name out of the field using regex
			this.className.replace(/field\[(\w+)\]/g, function()
			{
				var propName = arguments[1];
				// special case for creating a delete button - handler to delete current row
				switch (propName)
				{
					case 'insert':
						if (settings.allowChanges == true) 
						{
							$(t).unbind('click').click(function()
							{
								controller.onInsert(-1, null);
							});
						}
						break;
				}
			});
		});
		
		
		return this.each(function()
		{
		
			// grab the tbody element
			var tbody = jQuery(this).find('tbody');
			
			// grab the repeatable section - the initial contents of tbody
			var repeat;
			if ('jqRepeat' in this) 
			{
				repeat = this.jqRepeat;
			}
			else 
			{
				repeat = tbody.find('tr');
				this.jqRepeat = repeat;
				repeat.remove();
			}
			
			// prepare array of items bound to data rows
			this.bindRepeatItems = this.bindRepeatItems || [];
			
			// If items have been deleted from the array since the last bind
			// then the indexes in the repeat items array will be out of sync
			// with those of the data array. As a result we need to keep track 
			// of this offset.
			var workingOffset = 0;
			
			
			for (var i = 0; i < data.length; i++) 
			{
			
				var newRepeat = false;
				
				// determine whether we've bound to this row before by checking 
				// for bindIndex property on data row
				if ('bindIndex' in data[i]) 
				{
				
					// bound to this data row before so check its index within the 
					// array hasn't changed as this tells us some preceding elements
					// have been deleted and that we will need to delete the related
					// repeat items
					
					// if rows were missing from the array earlier on then the offset
					// will already be out so compensate for this by subtracting the 
					// working offset from the bindIndex of the current row
					var bindIndexWithOffset = data[i].bindIndex - workingOffset;
					
					// check the corrected bindIndex against our current data array 
					// index
					if (bindIndexWithOffset !== i) 
					{
					
						// we're missing some elements so work out how many					
						var offset = bindIndexWithOffset - i;
						
						// update our working offset
						workingOffset += offset;
						
						// grab the related repeat items and remove
						removeRepeatItems(this.bindRepeatItems, i, offset);
					}
				}
				else 
				{
				
					// not bound to this data item before so add a repeat item for it
					
					var clns;
					if (i >= this.bindRepeatItems.length) 
					{
						// adding to the end
						clns = repeat.clone(true, true).appendTo(tbody);
					}
					else 
					{
						// inserting in the middle
						clns = repeat.clone(true, true).insertBefore(this.bindRepeatItems[i].eq(0));
					}
					
					// insert new repeat item in repeat items array
					this.bindRepeatItems.splice(i, 0, clns);
					
					newRepeat = true;
				}
				
				// set our bindIndex so we can track adds and removals
				data[i].bindIndex = i;
				
				// should have done any adding and removing by this point so 
				// refresh repeat item content
				//this.bindRepeatItems[i].attr("bindIndex",i);
				// bindable elements have a class of format "field[ {property name} ]" e.g. field[firstname]
				this.bindRepeatItems[i].find('[class*=field]').each(function()
				{
					var t = this;
					
					// get the actual property name out of the field using regex
					this.className.replace(/field\[(\w+)\]/g, function()
					{
						var propName = arguments[1];
						var row = data[i];
						
						$(t).attr("bindIndex", i);
						
						// special case for creating a delete button - handler to delete current row
						switch (propName)
						{
							case 'delete':
								if (settings.allowChanges == true && newRepeat == true) 
								{
									$(t).click(function()
									{
										controller.onDelete(row.bindIndex, row)
									});
								}
								break;
							case 'insert':
								if (settings.allowChanges == true && newRepeat == true) 
								{
									$(t).click(function()
									{
										controller.onInsert(row.bindIndex, row);
									});
								}
								break;
							default:
								// not doing the delete special case so we're actually binding a value
								// so get the value from the data store
								var val = row[propName];
								
								switch (t.tagName.toUpperCase())
								{
									case 'INPUT':
										switch (t.type.toUpperCase())
										{
											case 'CHECKBOX':
												if (valueIsTrue(val)) 
												{
													t.checked = 'checked';
												}
												else 
												{
													t.checked = '';
												}
												
												if (settings.allowChanges == true && newRepeat == true) 
												{
													jQuery(t).click(function()
													{
														if (row[propName] != $(t).prop("checked")) 
														{
															row[propName] = $(t).prop("checked");
															controller.onChange(propName, row.bindIndex, row);
														}
													});
												}
												break;
											case 'TEXT':
											case 'HIDDEN':
												t.value = val;
												if (settings.allowChanges == true && newRepeat == true) 
												{
													jQuery(t).blur(function()
													{
														if (row[propName] != this.value) 
														{
															row[propName] = this.value;
															controller.onChange(propName, row.bindIndex, row);
														}
													});
												}
												
												
												break;
										}
										break;
									case 'TEXTAREA':
										if (settings.allowChanges == true && newRepeat == true) 
										{
											jQuery(t).blur(function()
											{
												row[propName] = this.value;
											});
										}
										t.value = val;
										break;
									case 'A':
										t.href = val;
										break;
									case 'SPAN':
										$(t).text(val);
										break;
										
									case "SELECT":
										t.value = val;
										if (settings.allowChanges == true && newRepeat == true) 
										{
											jQuery(t).change(function()
											{
												if (row[propName] != this.value) 
												{
													row[propName] = this.value;
													controller.onChange(propName, row.bindIndex, row);
												}
											});
										}
										
										break;
									default:
										$(t).text(val);
								}
								break;
						}
					});
				});
			}
			
			// any remaining repeat items can be trimmed as their related data
			// item has been removed
			if (this.bindRepeatItems.length > data.length) 
			{
				removeRepeatItems(this.bindRepeatItems, data.length, this.bindRepeatItems.length - data.length);
			}
			
		});
	};
	
	var removeRepeatItems = function(aryItems, start, length)
	{
		var aryItemsToRemove = aryItems.splice(start, length);
		for (var j = 0; j < aryItemsToRemove.length; j++) 
		{
			aryItemsToRemove[j].remove();
		}
	}
	
	var valueIsTrue = function(value)
	{
		if (value == "-1" || value.toString().toLowerCase() == "true" || value === true) 
			return true;
		else 
			return false;
	}
})();