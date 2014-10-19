define(['jquery', 'ko'], function ($, ko) {

    ko.components.register('dynamicStatusMarker', {
        viewModel: { require: 'app/component/dynamicStatusMarker' },
        template: { require: 'text!app/component/dynamicStatusMarker.html' }
    });

    function DynamicStatusMarker(kegModel) {
        var self = this;

        self.isActive = ko.observable(true);
        self.component = "dynamicStatusMarker";
        self.importance = 1;
    };

    return DynamicStatusMarker;
});