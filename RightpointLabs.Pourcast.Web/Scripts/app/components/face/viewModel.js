define('app/components/face/viewModel', ['ko', 'app/events', 'app/dataService'], function (ko, events, dataService) {
    
    function Face(model) {
        var self = this;

        self.isLow = ko.computed(function () {
            return ko.unwrap(model.isLow);
        });
    };

    return {
        viewModel: Face,
        template: { element: 'face-template' }
    };
});