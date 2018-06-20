Lookup = {
    init: function () {
        if (window.MvcLookup) {
            var lang = document.documentElement.lang;
            MvcLookup.prototype.lang = window.cultures.lookup[lang];

            [].forEach.call(document.getElementsByClassName('mvc-lookup'), function (element) {
                new MvcLookup(element);
            });
        }
    }
};
