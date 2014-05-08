(function() {

    ko.bindingHandlers.bubbles = {
        init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            bubbles($(element), valueAccessor());
        },
        update: function (element, valueAccessor, allBindings, viewModel, bindingContext) { }
    };

}());