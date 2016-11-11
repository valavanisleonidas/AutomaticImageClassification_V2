
// DAMN YOU Keith Wood
//  replaces some of jquery.svgdom.js functionality

// supported functions: addClass, removeClass, hasClass.

(function($) { // Hide scope, no $ conflict

/* Support adding class names to SVG nodes. */
$.fn.addClass = function (origAddClass) {
    return function (classNames) {
        classNames = classNames || '';
        return this.each(function () { 
            if ($.svg.isSVGElem(this)) {
                var node = this;
                $.each(classNames.split(/\s+/), function (i, className) {
                    var classes = (node.className ? node.className.baseVal : node.getAttribute('class'));
                    if ($.inArray(className, classes.split(/\s+/)) == -1) {
                        classes += (classes ? ' ' : '') + className;
                        (node.className ? node.className.baseVal = classes : node.setAttribute('class', classes));
                    }
                });
            }
            else {
                origAddClass.apply($(this), [classNames]);
            }
        });
    };
} ($.fn.addClass);

/* Support removing class names from SVG nodes. */
$.fn.removeClass = function (origRemoveClass) {
    return function (classNames) {
        classNames = classNames || '';
        return this.each(function () {
            if ($.svg.isSVGElem(this)) {
                var node = this;
                $.each(classNames.split(/\s+/), function (i, className) {
                    var classes = (node.className ? node.className.baseVal : node.getAttribute('class'));
                    classes = $.grep(classes.split(/\s+/), function (n, i) { return n != className; }).join(' ');
                    (node.className ? node.className.baseVal = classes : node.setAttribute('class', classes));
                });
            }
            else {
                origRemoveClass.apply($(this), [classNames]);
            }
        });
    };
} ($.fn.removeClass);


/* Support checking class names on SVG nodes. */
$.fn.hasClass = function (origHasClass) {
    return function (className) {
        className = className || '';
        var found = false;
        this.each(function () { 
            if ($.svg.isSVGElem(this)) {
                var classes = (this.className ? this.className.baseVal : this.getAttribute('class')).split(/\s+/);
                found = ($.inArray(className, classes) > -1);
            }
            else {
                found = (origHasClass.apply($(this), [className]));
            }
            return !found;
        });
        return found;
    };
} ($.fn.hasClass);

})(jQuery);