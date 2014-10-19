define(['jquery', 'ko'], function ($, ko) {

    function Keg(params) {
        console.log("Creating keg component");
        var self = this;
        self.model = params.model;

        var beer = ko.computed(function () { return ko.utils.unwrapObservable(ko.utils.unwrapObservable(params.model).beer); });
        self.beerComponents = params.rendererManager.getComponents('beer', beer, { model: beer });
    };

    return Keg;
});