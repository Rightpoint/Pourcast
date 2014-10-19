define(['jquery', 'ko'], function ($, ko) {

    ko.components.register('defaultStatusMarker', {
        viewModel: { require: 'app/component/defaultStatusMarker' },
        template: { require: 'text!app/component/defaultStatusMarker.html' }
    });

    function StatusMarker(kegModel) {
        var self = this;

        self.isActive = ko.observable(true);
        self.component = "defaultStatusMarker";
    };

    return StatusMarker;
});