define(['ko', 'jquery', 'app/bubbles'], function(ko, $, bubbles) {
    var bindings = {};

    bindings.init = function() {
        ko.bindingHandlers.bubbles = {
            init: function(element, valueAccessor, allBindings, viewModel, bindingContext) {
                bubbles($(element), valueAccessor());
            },
            update: function(element, valueAccessor, allBindings, viewModel, bindingContext) {}
        };

        ko.bindingHandlers.oneComponent = {
            init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {

            },
            update: function(element, valueAccessor, allBindings, viewModel, bindingContext) {
                var elements = [];
                var params = valueAccessor();

                var location = params.location;
                var resolver = params.resolver;

                var components = resolver.resolve(location);

                components().forEach(function(component) {

                });

                elements.push(document.createComment('ko component: { name: $parent.name, params: $parents[1] }'));
                elements.push(document.createComment('/ko'));

                ko.virtualElements.setDomNodeChildren(element, elements);
            }
        }
        ko.virtualElements.allowedBindings.oneComponent = true;
    };

    return bindings;
});