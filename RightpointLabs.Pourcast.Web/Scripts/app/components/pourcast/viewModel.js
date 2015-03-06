define('app/components/pourcast/viewModel', ['ko'], function (ko) {

    function Pourcast(model) {
        var self = this;

        self.taps = ko.computed(function() {
            return ko.unwrap(model.taps);
        });
    };

    return {
        viewModel: Pourcast,
        template: { element: 'pourcast-template' }
    };
});