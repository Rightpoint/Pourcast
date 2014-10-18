define(['ko'], function (ko) {

    function ComponentResolver(param) {
        this.param = param;
        this.components = ko.observable({});
    };

    ComponentResolver.prototype = {
        register: function (name, location) {
            var self = this;

            if (!(location in self.components())) {
                self.components()[location] = ko.observableArray();
            }

            require(['/components/' + name + '/config.js'], function (Config) {
                var config = new Config(self.param);

                if (!ko.components.isRegistered(name)) {
                    ko.components.register(name, { require: '/components/' + name + '/viewModel.js' });
                }

                self.components()[location].push({
                    name: name,
                    config: config
                });
            });
        },

        resolve: function (location) {
            var self = this;

            return ko.computed(function () {
                var components = self.components()[location]();

                if (components.length === 0) {
                    return '';
                }

                if (components.length === 1) {
                    return components[0].name;
                }

                return components.reduce(function (best, item) {
                    if (best.config.isActive() && item.config.rank() > best.config.rank()) {
                        return item;
                    } else {
                        return best;
                    }
                }).name;
            });
        },

        resolveMany: function (location) {
            var self = this;

            return ko.computed(function () {
                var components = self.components()[location]();

                return components.map(function (item) {
                    return item.name;
                });
            });
        },
    };

    return ComponentResolver;
});