// Header navigation binding
(function () {
    $(document).on('mouseleave', '.header-navigation .dropdown', function () {
        $(this).removeClass('open');
    });
})();

// Globalized validation binding
(function () {
    $.validator.methods.date = function (value, element) {
        return this.optional(element) || Globalize.parseDate(value);
    };

    $.validator.methods.number = function (value, element) {
        return this.optional(element) || !isNaN(Globalize.parseFloat(value));
    };

    $.validator.methods.min = function (value, element, param) {
        return this.optional(element) || Globalize.parseFloat(value) >= parseFloat(param);
    };

    $.validator.methods.max = function (value, element, param) {
        return this.optional(element) || Globalize.parseFloat(value) <= parseFloat(param);
    };

    $.validator.methods.range = function (value, element, param) {
        return this.optional(element) || (Globalize.parseFloat(value) >= parseFloat(param[0]) && Globalize.parseFloat(value) <= parseFloat(param[1]));
    };

    $.validator.addMethod('greater', function (value, element, param) {
        return this.optional(element) || Globalize.parseFloat(value) > parseFloat(param);
    });
    $.validator.unobtrusive.adapters.add('greater', ['min'], function (options) {
        options.rules['greater'] = options.params.min;
        if (options.message) {
            options.messages['greater'] = options.message;
        }
    });

    $.validator.addMethod('integer', function (value, element) {
        return this.optional(element) || /^[+-]?\d+$/.test(value);
    });
    $.validator.unobtrusive.adapters.addBool('integer');

    $.validator.addMethod('filesize', function (value, element, param) {
        if (this.optional(element) || !element.files)
            return true;

        var bytes = 0;
        for (var i = 0; i < element.files.length; i++) {
            bytes += element.files[i].size;
        }

        return bytes <= parseFloat(param);
    });
    $.validator.unobtrusive.adapters.add('filesize', ['max'], function (options) {
        options.rules['filesize'] = options.params.max;
        if (options.message) {
            options.messages['filesize'] = options.message;
        }
    });
    $(document).on('change', '[type="file"]', function () {
        $(this).focusout();
    });

    $.validator.addMethod('acceptfiles', function (value, element, param) {
        if (this.optional(element))
            return true;

        var files = $.map($(element).prop('files'), function (file) { return file.name.split('\\').pop(); });
        var params = param.split(/[,|]/);

        for (var i = 0; i < files.length; i++) {
            if (params.indexOf('.' + files[i].split('.').pop()) < 0) {
                return false;
            }
        }

        return true;
    });
    $.validator.unobtrusive.adapters.add('acceptfiles', ['extensions'], function (options) {
        options.rules['acceptfiles'] = options.params.extensions;
        if (options.message) {
            options.messages['acceptfiles'] = options.message;
        }
    });

    $(document).on('change', '.mvc-lookup-value', function () {
        var validator = $(this).parents('form').validate();

        if (validator && this.id) {
            var control = $(this).closest('.mvc-lookup').find('.mvc-lookup-control');
            if (validator.element('#' + this.id)) {
                control.removeClass('input-validation-error');
            } else {
                control.addClass('input-validation-error');
            }
        }
    });
    $('form').on('invalid-form', function (form, validator) {
        var values = $(this).find('.mvc-lookup-value');
        for (var i = 0; i < values.length; i++) {
            var control = $(values[i]).closest('.mvc-lookup').find('.mvc-lookup-control');

            if (validator.invalid[values[i].id]) {
                control.addClass('input-validation-error');
            } else {
                control.removeClass('input-validation-error');
            }
        }
    });
    var values = $('.mvc-lookup-value.input-validation-error');
    for (var i = 0; i < values.length; i++) {
        $(values[i]).closest('.mvc-lookup').find('.mvc-lookup-control').addClass('input-validation-error');
    }

    var currentIgnore = $.validator.defaults.ignore;
    $.validator.setDefaults({
        ignore: function () {
            return $(this).is(currentIgnore) && !$(this).hasClass('mvc-lookup-value');
        }
    });

    var lang = $('html').attr('lang');

    Globalize.cultures.en = null;
    Globalize.addCultureInfo(lang, window.cultures.globalize[lang]);
    Globalize.culture(lang);
})();

// Widgets binding
(function () {
    Alerts.init();
    Menu.init();
})();

// JQuery UI binding
(function () {
    window.addEventListener('mousedown', function (e) {
        if ($(e.target || e.srcElement).hasClass('ui-widget-overlay')) {
            $('.ui-dialog-content:visible').dialog('close');

            e.stopImmediatePropagation();
        }
    }, true);
})();

// Datepicker binding
(function () {
    var lang = $('html').attr('lang');

    if ($.fn.datepicker) {
        $.datepicker.setDefaults(window.cultures.datepicker[lang]);
        $('.datepicker').datepicker({
            beforeShow: function (e) {
                return !$(e).attr('readonly');
            },
            onSelect: function (value, data) {
                $(this).focusout();
                if (value != data.lastVal) {
                    $(this).change();
                }
            }
        });
    }

    if ($.fn.timepicker) {
        $.timepicker.setDefaults(window.cultures.timepicker[lang]);
        $('.datetimepicker').datetimepicker({
            beforeShow: function (e) {
                return !$(e).attr('readonly');
            },
            onSelect: function () {
                $(this).focusout();
            }
        });
    }
})();

// JsTree binding
(function () {
    var trees = $('.js-tree-view');
    for (var i = 0; i < trees.length; i++) {
        $(trees[i]).on('click.jstree', '.jstree-anchor', function (e) {
            if ($(this).closest('.widget-box.readonly').length) {
                e.stopImmediatePropagation();
            }
        });

        var tree = $(trees[i]).jstree({
            'core': {
                'themes': {
                    'icons': false
                }
            },
            'plugins': [
                'checkbox'
            ],
            'checkbox': {
                'keep_selected_style': false
            }
        });

        tree.on('ready.jstree', function (e, data) {
            var selected = $(this).prev('.js-tree-view-ids').children();
            for (var j = 0; j < selected.length; j++) {
                data.instance.select_node(selected[j].value, false, true);
            }

            data.instance.open_node($.makeArray(data.instance.element.find('> ul > li')), null, null);
            data.instance.element.show();
        });
    }

    $(document).on('submit', 'form', function () {
        var trees = $(this).find('.js-tree-view');
        for (var i = 0; i < trees.length; i++) {
            var tree = $(trees[i]).jstree();
            var ids = tree.element.prev('.js-tree-view-ids');

            ids.empty();
            var selected = tree.get_selected();
            for (var j = 0; j < selected.length; j++) {
                var node = tree.get_node(selected[j]);
                if (node.li_attr.id) {
                    ids.append('<input type="hidden" value="' + node.li_attr.id + '" name="' + tree.element.attr('for') + '" />');
                }
            }
        }
    });
})();

// Grid binding
(function () {
    if ($.fn.mvcgrid) {
        $('.mvc-grid').mvcgrid();
        $.fn.mvcgrid.lang = window.cultures.grid[$('html').attr('lang')];

        if (MvcGridNumberFilter) {
            MvcGridNumberFilter.prototype.isValid = function (value) {
                return value == '' || !isNaN(Globalize.parseFloat(value));
            }
        }
    }
})();

// Lookup binding
(function () {
    if ($.fn.mvclookup) {
        $.fn.mvclookup.lang = window.cultures.lookup[$('html').attr('lang')];
        $('.mvc-lookup').mvclookup();
    }
})();

// Read only binding
(function () {
    $(document).on('click', 'input:checkbox[readonly],input:radio[readonly]', function () {
        return false;
    });

    var widgets = $('.widget-box.readonly');
    widgets.find('input').attr('readonly', 'readonly');
    widgets.find('textarea').attr('readonly', 'readonly');
    if ($.fn.mvclookup) { widgets.find('.mvc-lookup').mvclookup({ readonly: true }); }
})();

// Input focus binding
(function () {
    var invalidInput = $('.content-container .input-validation-error:visible:not([readonly],.datepicker,.datetimepicker):first');
    if (invalidInput.length > 0) {
        invalidInput[0].setSelectionRange(invalidInput[0].value.length, invalidInput[0].value.length);
        invalidInput.focus();
    } else {
        var input = $('.content-container input:text:visible:not([readonly],.datepicker,.datetimepicker):first');
        if (input.length > 0) {
            input[0].setSelectionRange(input[0].value.length, input[0].value.length);
            input.focus();
        }
    }
})();

// Bootstrap binding
(function () {
    $('body').tooltip({
        selector: '[data-toggle=tooltip]'
    });
})();
