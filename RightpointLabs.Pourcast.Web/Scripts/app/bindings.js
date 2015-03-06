define('app/bindings', ['ko', 'jquery', 'app/bubbles'], function (ko, $, bubbles) {
    var bindings = {};

    bindings.init = function () {
        ko.bindingHandlers.bubbles = {
            init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                bubbles($(element), valueAccessor());
            },
            update: function (element, valueAccessor, allBindings, viewModel, bindingContext) { }
        };

        (function () {
            var getModel = function (params, bindingContext) {
                if (typeof params === 'string' || !params.model) {
                    return bindingContext.$data;
                } else {
                    // is this unwrapping bad?
                    return ko.unwrap(params.model);
                }
            };

            var getResolver = function(params, bindingContext) {
                if (typeof params === 'string' || !params.resolver) {
                    return getClosestResolver(bindingContext);
                } else {
                    return params.resolver;
                }
            };

            var getKey = function(params) {
                if (typeof params === 'string' || !params.key) {
                    return params;
                } else {
                    return params.key;
                }
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

            ko.bindingHandlers.oneComponent = {
                init: function(element, valueAccessor, allBindings, viewModel, bindingContext) { },
                update: function(element, valueAccessor, allBindings, viewModel, bindingContext) {
                    var params = valueAccessor();
                    var key = getKey(params);
                    var resolver = getResolver(params, bindingContext);
                    var model = getModel(params, bindingContext);
                    var components = resolver.resolve(key, model);

                    var bestComponent = components.reduce(function(best, component) {
                        var config = component.config;

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

                    var componentValueAccessor = function () {
                        if (bestComponent) {
                            return {
                                name: bestComponent.name,
                                params: model,
                            };
                        } else {
                            return {
                                name: 'missing',
                                params: model,
                            };
                        }
                    };

                    ko.bindingHandlers.component.init(element, componentValueAccessor, allBindings, viewModel, bindingContext);
                }
            };

            ko.bindingHandlers.manyComponents = {
                init: function(element, valueAccessor, allBindings, viewModel, bindingContext) { },
                update: function(element, valueAccessor, allBindings, viewModel, bindingContext) {
                    var params = valueAccessor();
                    var key = getKey(params);
                    var resolver = getResolver(params, bindingContext);
                    var model = getModel(params, bindingContext);
                    var components = resolver.resolve(key, model);

                    ko.virtualElements.emptyNode(element);

                    var elements = [];

                    components.forEach(function(component) {
                        var config = component.config;

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