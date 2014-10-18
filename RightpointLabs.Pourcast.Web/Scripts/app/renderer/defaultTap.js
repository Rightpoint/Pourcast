define(['jquery', 'ko'], function ($, ko) {

    ko.components.register('defaultTap', {
        viewModel: { require: 'app/component/defaultTap' },
        template: { require: 'text!app/component/defaultTap.html' }
    });

    // this constructor will be called by the rendererManager and provided the arguments provided as the second argument to the rendererManager.getComponents call
    function Tap(tapModel) {
        var self = this;

        // required parts of the renderer API - these can be simple values or observables (use simple values if they won't change to avoid unnecessary subscriptions)
        self.isActive = ko.observable(true);
        self.component = "defaultTap";
        // optional part of the renderer API - simple values or observables are fine here too
        self.importance = 0;

        // do whatever you want here - your component will get a reference to the constructed object, so it can access this stuff
        self.model = ko.observable(tapModel);
    };

    return Tap;
});