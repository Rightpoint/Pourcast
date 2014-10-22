define(['ko', 'jquery', 'app/bubbles'], function (ko, $, bubbles) {
    var bindings = {};

    bindings.init = function () {
        ko.bindingHandlers.bubbles = {
            init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                bubbles($(element), valueAccessor());
            },
            update: function (element, valueAccessor, allBindings, viewModel, bindingContext) { }
        };

        (function() {
            var init = function(element, valueAccessor, allBindings, viewModel, bindingContext) {
                var model = bindingContext.$data;
                var components = getComponents(valueAccessor, bindingContext);

                components().forEach(function(component) {
                    ensureConfig(component, model, bindingContext);
                });
            }

            var ensureConfig = function(component, model, bindingContext) {
                bindingContext.configs = bindingContext.configs || {};

                var name = component.name;
                if (!bindingContext.configs[name]) {
                    bindingContext.configs[name] = new component.Config(model);
                }

                return bindingContext.configs[name];
            };

            var getClosestResolver = function(bindingContext) {
                if (bindingContext.$data.resolver) {
                    return bindingContext.$data.resolver;
                }

                var i = 0;
                var resolver = null;
                while (!resolver) {
                    var $parent = bindingContext.$parents[i];
                    if ($parent.resolver) {
                        resolver = $parent.resolver;
                    }

                    i++;
                }

                return resolver;
            };

            var getComponents = function(valueAccessor, bindingContext) {
                var params = valueAccessor();
                var resolver;
                var location;

                if (typeof params === 'string') {
                    resolver = getClosestResolver(bindingContext);
                    location = params;
                } else {
                    if (!params.resolver) {
                        resolver = params.resolver;
                    } else {
                        resolver = getClosestResolver(bindingContext);
                    }
                    location = params.location;
                }

                return resolver.resolve(location);
            }

            ko.bindingHandlers.oneComponent = {
                init: init,
                update: function(element, valueAccessor, allBindings, viewModel, bindingContext) {
                    var model = bindingContext.$data;
                    var components = getComponents(valueAccessor, bindingContext);

                    var bestComponent = components().reduce(function(best, component) {
                        var config = ensureConfig(component, model, bindingContext);

                        if (best == null) {
                            if (config.isActive()) {
                                return {
                                    name: component.name,
                                    config: config
                                };
                            } else {
                                return null;
                            }
                        } else {
                            if (config.isActive() && config.rank() > best.config.rank()) {
                                return {
                                    name: component.name,
                                    config: config
                                };
                            } else {
                                return best;
                            }
                        }
                    }, null);

                    ko.virtualElements.emptyNode(element);

                    var componentValueAccessor = function() {
                        return {
                            name: bestComponent.name,
                            params: model,
                        };
                    };

                    ko.bindingHandlers.component.init(element, componentValueAccessor, allBindings, viewModel, bindingContext);
                }
            };

            ko.bindingHandlers.manyComponents = {
                init: init,
                update: function(element, valueAccessor, allBindings, viewModel, bindingContext) {
                    var model = bindingContext.$data;
                    var components = getComponents(valueAccessor, bindingContext);

                    ko.virtualElements.emptyNode(element);

                    var elements = [];

                    components().forEach(function(component) {
                        var config = ensureConfig(component, model, bindingContext);

                        if (config.isActive()) {

                            var innerElement = document.createElement('div');
                            elements.push(innerElement);

                            var componentValueAccessor = function() {
                                return {
                                    name: component.name,
                                    params: model,
                                };
                            };
                            ko.bindingHandlers.component.init(innerElement, componentValueAccessor, allBindings, viewModel, bindingContext);
                        }
                    });

                    ko.virtualElements.setDomNodeChildren(element, elements);
                }
            };

            ko.virtualElements.allowedBindings.oneComponent = true;
            ko.virtualElements.allowedBindings.manyComponents = true;
        }());
    };

    return bindings;
});