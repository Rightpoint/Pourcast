define(['jquery', 'ko'], function ($, ko) {

    ko.components.register('backgroundBubblesActive', {
        viewModel: { require: 'app/component/backgroundBubblesActive' },
        template: { require: 'text!app/component/backgroundBubblesActive.html' }
    });

    function BackgroundBubblesActive(kegModel) {
        var self = this;

        self.isActive = ko.observable(true);
        self.component = "backgroundBubblesActive";
        self.importance = -1;
    };

    return BackgroundBubblesActive;
});