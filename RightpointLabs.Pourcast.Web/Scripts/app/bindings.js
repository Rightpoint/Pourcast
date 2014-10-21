define(['ko', 'jquery', 'app/bubbles'], function (ko, $, bubbles) {
    var bindings = {};

    bindings.init = function () {
        ko.bindingHandlers.bubbles = {
            init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                bubbles($(element), valueAccessor());
            },
            update: function (element, valueAccessor, allBindings, viewModel, bindingContext) { }
        };



        var getOrAddConfig = function (component, model, bindingContext) {
            if (!bindingContext.configs) {
                bindingContext.configs = [];
            }

            var matchingConfigs = bindingContext.configs.filter(function (config) {
                return config.name === component.name;
            });

            if (matchingConfigs.length > 0) {
                return matchingConfigs[0].config;

            } else {
                var config = new component.Config(model);
                bindingContext.configs.push({
                    name: component.name,
                    config: config
                });

                return config;
            }
        };

        var getClosestResolver = function (bindingContext) {
            if (bindingContext.$data.resolver) {
                return bindingContext.$data.resolver;
            }

            var i = 0;
            var resolver;
            while (!resolver) {
                var $parent = bindingContext.$parents[i];
                if ($parent.resolver) {
                    resolver = $parent.resolver;
                } else {
                    i++;
                }
            }

            return resolver;
        };

        ko.bindingHandlers.oneComponent = {
            init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                var params = valueAccessor();
                var model = bindingContext.$data;
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

                var components = resolver.resolve(location);

                components().forEach(function (component) {
                    getOrAddConfig(component, model, bindingContext);
                });
            },
            update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                var params = valueAccessor();
                var model = bindingContext.$data;
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

                var components = resolver.resolve(location);

                var bestComponent = components().reduce(function (best, component) {
                    var config = getOrAddConfig(component, model, bindingContext);

                    if (best == null || config.isActive() && config.rank() > best.config.rank()) {
                        return {
                            name: component.name,
                            config: config
                        };
                    } else {
                        return best;
                    }
                }, null);

                ko.virtualElements.emptyNode(element);

                var componentValueAccessor = function () {
                    return {
                        name: bestComponent.name,
                        params: model,
                    };
                };

                ko.bindingHandlers.component.init(element, componentValueAccessor, allBindings, viewModel, bindingContext);
            }
        };
        ko.virtualElements.allowedBindings.oneComponent = true;


        ko.bindingHandlers.manyComponents = {
            init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                var params = valueAccessor();
                var model = bindingContext.$data;
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

                var components = resolver.resolve(location);

                components().forEach(function (component) {
                    getOrAddConfig(component, model, bindingContext);
                });
            },
            update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                var params = valueAccessor();
                var model = bindingContext.$data;
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

                var components = resolver.resolve(location);

                ko.virtualElements.emptyNode(element);

                var elements = [];

                components().forEach(function (component) {
                    var config = getOrAddConfig(component, model, bindingContext);

                    if (config.isActive()) {

                        var innerElement = document.createElement('div');
                        elements.push(innerElement);

                        var componentValueAccessor = function () {
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
        ko.virtualElements.allowedBindings.manyComponents = true;
    };

    return bindings;
});