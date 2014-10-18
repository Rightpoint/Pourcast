define(['jquery', 'ko'], function ($, ko) {

    function Keg(params) {
        var self = this;
        self.rendererManager = ko.utils.unwrapObservable(params.rendererManager);
        self.model = ko.observable(ko.utils.unwrapObservable(params.model));
    };

    return Keg;
});