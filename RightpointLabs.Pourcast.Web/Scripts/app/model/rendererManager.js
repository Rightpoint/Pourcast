define(['jquery', 'ko', 'require'], function ($, ko, require) {

    ko.components.register('missingRenderer', {
        viewModel: { require: 'app/component/missing' },
        template: { require: 'text!app/component/missing.html' }
    });

    function RendererManager(configJSON) {
        // we're all fake for now - we'll fix that later :)
        configJSON = {
            'tap': ['app/renderer/defaultTap']
        };
        var self = this;

        var renderModules = [];
        $.each(configJSON, function (k, v) {
            renderModules = $.merge(renderModules, v);
        });

        self.renderers = ko.observable({});
        require(renderModules, function () {
            var requireResults = arguments;
            var renderers = {};
            // now all the modules we need are loaded
            $.map(configJSON, function (v, k) {
                renderers[k] = $.map(v, function (vv) {
                    var ix = $.inArray(vv, renderModules);
                    if (ix < 0 && window.console && console.log) {
                        console.log("Cannot find", vv, "in", renderModules);
                    }
                    return requireResults[ix];
                });
            });
            self.renderers(renderers);
        });
    };

    RendererManager.prototype.getComponents = function (rendererKey, rendererCtorArgs, componentParams) {
        var self = this;
        return ko.pureComputed(function () {
            // grab the constructors we require-ed earlier
            var renderers = self.renderers()[rendererKey] || [];

            // grab the current value of rendererCtorArgs and make sure it's an array (in case we just got a single value or something)
            var ctorArgs = ko.utils.unwrapObservable(rendererCtorArgs);
            if (null == ctorArgs) {
                ctorArgs = [];
            } else if (!$.isArray(ctorArgs)) {
                ctorArgs = [ctorArgs];
            }

            // now build the actual renderer objects
            renderers = $.map(renderers, function(constructor) {
                // http://stackoverflow.com/a/14378462/12502
                var args = [null].concat(ctorArgs);
                var factoryFunction = constructor.bind.apply(constructor, args);
                return new factoryFunction();
            });

            // filter out the inactive ones
            renderers = $.grep(renderers, function(e) {
                return ko.utils.unwrapObservable(e.isActive);
            });

            // now sort them
            renderers.sort(function(a, b) {
                a = ko.utils.unwrapObservable(a.importance || 0);
                b = ko.utils.unwrapObservable(b.importance || 0);
                return a > b ? -1 : a < b ? 1 : 0;
            });

            // and finally, build the object to be passed to the ko component binding
            renderers = $.map(renderers, function (e) {
                return {
                    name: ko.utils.unwrapObservable(e.component),
                    params: $.extend({
                        rendererManager: self,
                        renderer: e
                    }, ko.utils.unwrapObservable(componentParams) || {})
                };
            });

            return renderers;
        });
    };

    RendererManager.prototype.getComponent = function (rendererKey, rendererCtorArgs, componentParams) {
        var self = this;
        var all = this.getComponents(rendererKey, rendererCtorArgs, componentParams);
        return ko.pureComputed(function () {
            var r = all();
            return r.length ? r[0] : { name: 'missingRenderer', params: { rendererManager: self, key: rendererKey } };
        });
    };

    return RendererManager;
});