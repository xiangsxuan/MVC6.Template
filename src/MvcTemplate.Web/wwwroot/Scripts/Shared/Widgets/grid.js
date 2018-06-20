Grid = {
    init: function () {
        if ($.fn.mvcgrid) {
            $('.mvc-grid').mvcgrid();
            var lang = document.documentElement.lang;
            $.fn.mvcgrid.lang = window.cultures.grid[lang];

            if (MvcGridNumberFilter) {
                MvcGridNumberFilter.prototype.isValid = function (value) {
                    return value == '' || !isNaN(Globalize.parseFloat(value));
                }
            }
        }
    }
};
