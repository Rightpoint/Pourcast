define(['jquery', 'ko'], function ($, ko) {

    ko.components.register('backgroundColor', {
        viewModel: { require: 'app/component/backgroundColor' },
        template: { require: 'text!app/component/backgroundColor.html' }
    });

    function BackgroundColor(kegModel) {
        var self = this;

        self.isActive = ko.observable(true);
        self.component = "backgroundColor";
        self.importance = 100000;
    };

    return BackgroundColor;
});