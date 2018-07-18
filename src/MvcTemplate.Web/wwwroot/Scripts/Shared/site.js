// Widgets init
(function () {
    DatePicker.init();
    Validator.init();
    Alerts.init();
    Header.init();
    JsTree.init();
    Lookup.init();
    Menu.init();
    Grid.init();
})();

// Read only binding
(function () {
    $(document).on('click', '[readonly]', function () {
        return false;
    });

    var widgets = $('.widget-box.readonly');
    widgets.find('input').attr({ readonly: 'readonly', tabindex: -1 });
    widgets.find('textarea').attr({ readonly: 'readonly', tabindex: -1 });

    if (window.MvcLookup) {
        widgets.find('.mvc-lookup').each(function (element) {
            new MvcLookup(element, { readonly: true });
        });
    }
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
