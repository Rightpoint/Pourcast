define(['jquery', 'ko', 'require'], function ($, ko, require) {

    ko.components.register('missingRenderer', {
        viewModel: { require: 'app/component/missing' },
        template: { require: 'text!app/component/missing.html' }
    });

    function RendererManager(configJSON) {
        // we're all fake for now - we'll fix that later :)
        configJSON = {
            'statusMarker': ['app/renderer/defaultStatusMarker'],
            'background': ['app/renderer/backgroundColor', 'app/renderer/backgroundBubbles', 'app/renderer/backgroundBubblesActive'],
            'beer': ['app/renderer/defaultBeer'],
            'tap': ['app/renderer/defaultTap'],
            'keg': ['app/renderer/defaultKeg']
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
    
    RendererManager.prototype.createChild = function() {
        return new ChildRendererManager(this);
    }

    function ChildRendererManager(root) {
        var self = this;

        self.root = root;
    }

    ChildRendererManager.prototype.getComponents = function (rendererKey, rendererCtorArg, componentParams) {
        var self = this;

        function createRenderers() {
            // grab the constructors we require-ed earlier - the subscription will result in us getting called
            var renderers = self.root.renderers.peek()[rendererKey] || [];

            // now build the actual renderer objects and the component reference for them
            renderers = $.map(renderers, function (constructor) {
                var renderer = new constructor(rendererCtorArg);
                return {
                    renderer: renderer,
                    componentReference: {
                        name: renderer.component,
                        params: ko.observable({
                            rendererManager: new ChildRendererManager(self.root),
                            renderer: renderer
                        })
                    }
                }
            });

            return renderers;
        }
        var rendererObjects = ko.observable(null);
        self.root.renderers.subscribe(function () {
            rendererObjects(null);
        });

        return ko.computed(function () {
            var renderers = rendererObjects();
            if (null == renderers) {
                renderers = createRenderers();
                rendererObjects(renderers);
            }

            // filter out the inactive ones
            renderers = $.grep(renderers, function (e) {
                return ko.utils.unwrapObservable(e.renderer.isActive);
            });

            // now sort them
            renderers.sort(function (a, b) {
                a = ko.utils.unwrapObservable(a.renderer.importance || 0);
                b = ko.utils.unwrapObservable(b.renderer.importance || 0);
                return a > b ? -1 : a < b ? 1 : 0;
            });

            // and finally, get and update the object to be passed to the ko component binding
            var cParams = ko.utils.unwrapObservable(componentParams)|| {};
            var components = $.map(renderers, function (e) {
                var params = e.componentReference.params.peek();
                var doUpdate = false;
                for (var k in cParams) {
                    if (cParams[k] !== params[k]) {
                        doUpdate = true;
                    }
                }
                if (doUpdate) {
                    e.componentReference.params($.extend({}, params, cParams));
                }
                return e.componentReference;
            });

            return components;
        }, null, { deferEvaluation: true });
    };

    ChildRendererManager.prototype.getComponent = function (rendererKey, rendererCtorArgs, componentParams) {
        var self = this;
        var all = self.getComponents(rendererKey, rendererCtorArgs, componentParams);
        return ko.pureComputed(function () {
            var r = all();
            return r.length ? r[0] : { name: 'missingRenderer', params: { rendererManager: self, key: rendererKey } };
        });
    };

    return RendererManager;
});