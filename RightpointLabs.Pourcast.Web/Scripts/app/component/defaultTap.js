define(['jquery', 'ko'], function ($, ko) {

    function Tap(params) {
        var self = this;

        // guaranteed objects on params include rendererManager (in case you want to have renderer children), and renderer (the one that chose this component)
        self.rendererManager = ko.observable(ko.utils.unwrapObservable(params.rendererManager));

        // additionally, the object passed as the third argument to rendererManager.getComponents is merged into params as well, 
        //   so if the contract for this kind of renderer gives you anything, you can use it.  (the 'model' property is commonly here)
        self.model = ko.observable(ko.utils.unwrapObservable(params.model));
    };

    return Tap;
});