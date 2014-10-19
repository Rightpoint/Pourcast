define(['jquery', 'ko'], function ($, ko) {

    function Tap(params) {
        var self = this;

        // guaranteed objects on params include rendererManager (in case you want to have renderer children), and renderer (the one that chose this component)
        //  if you're going to have children, call getComponent/getComponents here.  They return observables which you can wire right to component/foreach bindings
        var keg = ko.computed(function() { return ko.utils.unwrapObservable(params.model).keg; });
        self.kegComponent = params.rendererManager.getComponent('keg', keg, { model: keg });

        // additionally, the object passed as the third argument to rendererManager.getComponents is merged into params as well, 
        //   so if the contract for this kind of renderer gives you anything, you can use it.  (the 'model' property is commonly here)
        self.model = params.model;
    };

    return Tap;
});