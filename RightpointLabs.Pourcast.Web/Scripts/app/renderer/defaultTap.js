define(['jquery', 'ko'], function ($, ko) {

    ko.components.register('defaultTap', {
        viewModel: { require: 'app/component/defaultTap' },
        template: { require: 'text!app/component/defaultTap.html' }
    });

    // this constructor will be called by the rendererManager and provided the model/tap.js model for the tap this renderer is being used for.
    function Tap(tapModel) {
        var self = this;

        // required parts of the renderer API
        self.isActive = ko.observable(true);
        self.component = "defaultTap";

        // do whatever you want here - your component will be able 
        self.model = ko.observable(tapModel);
    };

    return Tap;
});