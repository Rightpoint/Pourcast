define(['jquery', 'ko'], function ($, ko) {

    ko.components.register('backgroundBubbles', {
        viewModel: { require: 'app/component/backgroundBubbles' },
        template: { require: 'text!app/component/backgroundBubbles.html' }
    });

    function BackgroundBubbles(kegModel) {
        var self = this;

        self.isActive = ko.observable(true);
        self.component = "backgroundBubbles";
        self.importance = 0;
    };

    return BackgroundBubbles;
});