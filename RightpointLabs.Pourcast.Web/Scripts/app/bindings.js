define(['ko', 'app/bubbles'], function(ko, bubbles) {
    var bindings = {};
    bindings.init = function() {
        ko.bindingHandlers.bubbles = {
            init: function(element, valueAccessor, allBindings, viewModel, bindingContext) {
                bubbles($(element), valueAccessor());
            },
            update: function(element, valueAccessor, allBindings, viewModel, bindingContext) {}
        };
    };

    return bindings;
});