var SORT_COLUMN_INDEX;

function jscss(a, o, c1, c2)
{
	switch (a)
	{
		case 'swap':
			o.className = !jscss('check', o, c1) ? o.className.replace(c2, c1) : o.className.replace(c1, c2);
			break;
			
		case 'add':
			if (!jscss('check', o, c1)) o.className += o.className ? ' ' + c1 : c1;
			break;
			
		case 'remove':
			var rep = o.className.match(' ' + c1) ? ' ' + c1 : c1;
			o.className = o.className.replace(rep, '');
			break;
			
		case 'check':
			return new RegExp('\\b' + c1 + '\\b').test(o.className);
			break;
	}
}

// restore the class names
function colorRows(table)
{
	var rows = table.rows;
	var l = rows.length;
	for (var i = 0; i < l; i++) 
	{
		jscss('remove',rows[i], i % 2 ? "odd" : "even");
		jscss('add',rows[i], i % 2 ? "even" : "odd");
	}
}	
	
function makeSortable(table)
{
    if (table.rows && table.rows.length > 0) 
        var firstRow = table.rows[0];
    
    if (!firstRow) 
        return;
    
    // We have a first row: assume it's the header, and make its contents clickable links
    for (var i = 0; i < firstRow.cells.length; i++) 
    {
        var cell = firstRow.cells[i];
        
        if ((' ' + cell.className + ' ').indexOf("unsortable") != -1) 
            ;
        else 
        {
            var txt = ts_getInnerText(cell);
            
            cell.innerHTML = '<a href="javascript:void(0)" onclick="ts_resortTable(this, ' + i + ');">' + txt +
            '<span class="sortindicator"></span></a>';
        }
    }
}

function ts_getInnerText(el)
{
	if (el==null)
	    return "";
    if (typeof el == "string") 
        return el;
    if (typeof el == "undefined") 
    {
        return el
    };
    if (el.innerText) 
        return el.innerText; //Not needed but it is faster
    var str = "";
    
    var cs = el.childNodes;
    var l = cs.length;
    for (var i = 0; i < l; i++) 
    {
        switch (cs[i].nodeType)
        {
            case 1: //ELEMENT_NODE
                str += ts_getInnerText(cs[i]);
                break;
            case 3: //TEXT_NODE
                str += cs[i].nodeValue;
                break;
        }
    }
    return str;
}

function ts_resortTable(lnk, clid)
{
	
    // get the span
    var span;
    for (var ci = 0; ci < lnk.childNodes.length; ci++) 
    {
        if (lnk.childNodes[ci].tagName && lnk.childNodes[ci].tagName.toLowerCase() == 'span') 
            span = lnk.childNodes[ci];
    }
    var spantext = ts_getInnerText(span);
    var td = lnk.parentNode;
    var column = clid || td.cellIndex;
    var table = getParent(td, 'TABLE');
    
    // Work out a type for the column
    if (table.rows.length <= 1) 
        return;
		
    var itm = ts_getInnerText(table.rows[1].cells[column]);
    sortfn = ts_sort_caseinsensitive;
    
    if (itm.match(/^([1-9]|0[1-9]|[12][0-9]|3[01])\/([1-9]|0[1-9]|1[012])\/(19[0-9][0-9]|20[0-9][0-9])/)) 
		sortfn = ts_sort_date;
    
    //if (itm.match(/^\d\d[\/-]\d\d[\/-]\d\d\d\d$/)) sortfn = ts_sort_date;
    //if (itm.match(/^\d\d[\/-]\d\d[\/-]\d\d$/)) sortfn = ts_sort_date;
    if (itm.match(/^[�$]/)) 
        sortfn = ts_sort_currency;

    if (itm.match(/^[\d\.]+$/)) 
        sortfn = ts_sort_numeric;
    
    SORT_COLUMN_INDEX = column;
    var firstRow = new Array();
    var newRows = new Array();
	
    for (i = 0; i < table.rows[0].length; i++) 
    {
        firstRow[i] = table.rows[0][i];
    }
	
    for (j = 1; j < table.rows.length; j++) 
    {
        newRows[j - 1] = table.rows[j];
    }
    
    newRows.sort(sortfn);
    
    if (span.getAttribute("sortdir") == 'down') 
    {
        ARROW = 'sortindicator-desc';
        newRows.reverse();
        span.setAttribute('sortdir', 'up');
    }
    else 
    {
        ARROW = 'sortindicator-asc';
        span.setAttribute('sortdir', 'down');
    }
    
    // We appendChild rows that already exist to the tbody, so it moves them rather than creating new ones
    // don't do sortbottom rows
    for (i = 0; i < newRows.length; i++) 
		for (j = 0; j < newRows[i].cells.length; j++)
		jscss('remove',newRows[i].cells(j),"sorted");		
	
		
    for (i = 0; i < newRows.length; i++) 
    {		
        if (!newRows[i].className || (newRows[i].className && (newRows[i].className.indexOf('sortbottom') == -1))) 
		{
			jscss('add',newRows[i].cells(clid),"sorted");
			table.tBodies[0].appendChild(newRows[i]);
		}
    }
	
    // do sortbottom rows only
    for (i = 0; i < newRows.length; i++) 
    {
        if (newRows[i].className && (newRows[i].className.indexOf('sortbottom') != -1)) 
            table.tBodies[0].appendChild(newRows[i]);
    }
    
    // Delete any other arrows there may be showing
	var header=table.rows[0];
    for (var oEnum = new Enumerator(header.cells); !oEnum.atEnd(); oEnum.moveNext()) 
	{
		var cell = oEnum.item();
		cell.className="";
	}  
	
	header.cells[SORT_COLUMN_INDEX].className="sorted";
	  
	var arr=header.getElementsByTagName('span');
	for (i=0; i<arr.length; i++)
	     arr[i].className="sortindicator";
    
    
	jscss("add",span,ARROW);
	colorRows(table);
}

function getParent(el, pTagName)
{
    if (el == null) 
        return null;
    else 
        if (el.nodeType == 1 && el.tagName.toLowerCase() == pTagName.toLowerCase()) // Gecko bug, supposed to be uppercase
            return el;
        else 
            return getParent(el.parentNode, pTagName);
}

function ts_sort_date(a, b)
{
    // y2k notes: two digit years less than 50 are treated as 20XX, greater than 50 are treated as 19XX
    value1 = ts_getInnerText(a.cells[SORT_COLUMN_INDEX]);
    value2 = ts_getInnerText(b.cells[SORT_COLUMN_INDEX]);
    
    var date1, date2;
    var month1, month2;
    var year1, year2;
    var hour1, hour2;
    var min1, min2;
    var sec1, sec2;
    
    date1 = value1.substring(0, value1.indexOf("/"));
    month1 = value1.substring(value1.indexOf("/") + 1, value1.lastIndexOf("/"));
    year1 = value1.substr(value1.lastIndexOf("/") + 1, 4);
    
    date2 = value2.substring(0, value2.indexOf("/"));
    month2 = value2.substring(value2.indexOf("/") + 1, value2.lastIndexOf("/"));
    year2 = value2.substr(value2.lastIndexOf("/") + 1, 4);

    var time1 = value1.split(" ")[1];
    var time2 = value2.split(" ")[1];

    hour1 = time1.substring(0, time1.indexOf(":"));
    min1 = time1.substring(time1.indexOf(":") + 1, time1.lastIndexOf(":"));
    sec1 = time1.substr(time1.lastIndexOf(":") + 1, 2);

    hour2 = time2.substring(0, time2.indexOf(":"));
    min2 = time2.substring(time2.indexOf(":") + 1, time2.lastIndexOf(":"));
    sec2 = time2.substr(time2.lastIndexOf(":") + 1, 2);

    value1 = new Date(year1, month1, date1, hour1, min1, sec1);
    value2 = new Date(year2, month2, date2, hour2, min2, sec2);
    
    if (value1 == value2) 
        return 0;
    if (value1 < value2) 
        return -1;
    return 1;
    
    
}

function ts_sort_currency(a, b)
{
    aa = ts_getInnerText(a.cells[SORT_COLUMN_INDEX]).replace(/[^0-9.]/g, '');
    bb = ts_getInnerText(b.cells[SORT_COLUMN_INDEX]).replace(/[^0-9.]/g, '');
    return parseFloat(aa) - parseFloat(bb);
}

function ts_sort_numeric(a, b)
{
    aa = parseFloat(ts_getInnerText(a.cells[SORT_COLUMN_INDEX]));
    if (isNaN(aa)) 
        aa = 0;
    bb = parseFloat(ts_getInnerText(b.cells[SORT_COLUMN_INDEX]));
    if (isNaN(bb)) 
        bb = 0;
    return aa - bb;
}

function ts_sort_caseinsensitive(a, b)
{
    aa = ts_getInnerText(a.cells[SORT_COLUMN_INDEX]).toLowerCase();
    bb = ts_getInnerText(b.cells[SORT_COLUMN_INDEX]).toLowerCase();
    if (aa == bb) 
        return 0;
    if (aa < bb) 
        return -1;
    return 1;
}

function ts_sort_default(a, b)
{
    aa = ts_getInnerText(a.cells[SORT_COLUMN_INDEX]);
    bb = ts_getInnerText(b.cells[SORT_COLUMN_INDEX]);
    if (aa == bb) 
        return 0;
    if (aa < bb) 
        return -1;
    return 1;
}
