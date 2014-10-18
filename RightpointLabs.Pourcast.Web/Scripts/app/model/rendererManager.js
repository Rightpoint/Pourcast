define(['jquery', 'ko', 'require'], function ($, ko, require) {

    ko.components.register('missingRenderer', {
        viewModel: { require: 'app/component/missing' },
        template: { require: 'text!app/component/missing.html' }
    });

    function RendererManager(tap, configJSON) {
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
                    return new requireResults[ix](tap);
                });
            });
            self.renderers(renderers);
        });
    };

    RendererManager.prototype.getComponents = function (rendererKey) {
        var self = this;
        return ko.pureComputed(function () {
            return $.map($.grep(self.renderers()[rendererKey] || [], function(e) {
                return ko.utils.unwrapObservable(e.isActive);
            }), function (e) {
                return {
                    name: ko.utils.unwrapObservable(e.component),
                    params: {
                        rendererManager: self,
                        renderer: e
                    }
                };
            });
        });
    };

    RendererManager.prototype.getComponent = function (rendererKey) {
        var self = this;
        var all = this.getComponents(rendererKey);
        return ko.pureComputed(function () {
            var r = all();
            return r.length ? r[0] : { name: 'missingRenderer', params: { rendererManager: self, key: rendererKey } };
        });
    };

    return RendererManager;
});