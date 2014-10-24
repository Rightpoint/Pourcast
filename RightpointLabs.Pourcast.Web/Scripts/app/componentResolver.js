define(['ko'], function (ko) {

    function ComponentResolver() {
        this.components = ko.observableArray();
        this.configs = {};
    };

    ko.components.register('missing', { require: 'app/components/missing/viewModel' });

    var ensureConfig = function (component, model) {
        var self = this;
        var name = component.name;

        if (!self.configs[name]) {
            self.configs[name] = new component.Config(model);
        }

        return self.configs[name];
    };

    ComponentResolver.prototype = {
        register: function (name, location) {
            var self = this;

            require(['app/components/' + name + '/config'], function (Config) {
                if (!ko.components.isRegistered(name)) {
                    ko.components.register(name, { require: 'app/components/' + name + '/viewModel' });
                }

                self.components.push({
                    location: location,
                    name: name,
                    Config: Config
                });
            });
        },

        resolve: function (location, model) {
            var self = this;

            var matchingComponents = self.components().filter(function (component) {
                return component.location === location;
            });

            var configs = matchingComponents.map(function(component) {
                return {
                    name: component.name,
                    config: ensureConfig.call(self, component, model)
                };
            });

            return configs;
        },
    };

    return ComponentResolver;
});