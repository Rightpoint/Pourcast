define(['jquery', 'ko'], function ($, ko) {

    function Keg(params) {
        console.log("Creating keg component");
        var self = this;
        self.model = params.model;

        var beer = ko.computed(function () { return ko.utils.unwrapObservable(ko.utils.unwrapObservable(self.model).beer); });
        self.beerComponents = params.rendererManager.getComponents('beer', beer, { model: beer });

        self.backgroundComponents = params.rendererManager.getComponents('background', self.model, { model: self.model });
        self.statusMarkerComponent = params.rendererManager.getComponent('statusMarker', self.model, { model: self.model });
    };

    return Keg;
});