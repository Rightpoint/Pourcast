define(['jquery', 'ko'], function ($, ko) {

    ko.components.register('defaultKeg', {
        viewModel: { require: 'app/component/defaultKeg' },
        template: { require: 'text!app/component/defaultKeg.html' }
    });

    function Keg(kegModel) {
        console.log("Creating keg renderer");
        var self = this;

        self.isActive = ko.observable(true);
        self.component = "defaultKeg";
    };

    return Keg;
});