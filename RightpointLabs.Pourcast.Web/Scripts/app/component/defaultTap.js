define(['jquery', 'ko'], function ($, ko) {

    function Tap(params) {
        var self = this;
        var renderer = ko.utils.unwrapObservable(params.renderer);
        self.rendererManager = ko.observable(ko.utils.unwrapObservable(params.rendererManager));
        self.renderer = ko.observable(renderer);
        self.model = ko.observable(ko.utils.unwrapObservable(renderer.model));
    };

    return Tap;
});