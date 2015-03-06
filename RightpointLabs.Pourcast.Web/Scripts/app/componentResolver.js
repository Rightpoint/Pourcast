define('app/componentResolver', ['ko'], function (ko) {

    function ComponentResolver() {
        this.components = ko.observableArray([]);
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
        register: function (name, key) {
            var self = this;

            require(['app/components/' + name + '/config'], function (Config) {
                if (!ko.components.isRegistered(name)) {
                    ko.components.register(name, { require: 'app/components/' + name + '/viewModel' });
                }

                self.components.push({
                    key: key,
                    name: name,
                    Config: Config
                });
            });
        },

        resolve: function (key, model) {
            var self = this;

            var matchingComponents = self.components().filter(function (component) {
                return component.key === key;
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