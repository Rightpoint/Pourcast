define('app/components/beer/viewModel', ['ko', 'app/events', 'app/dataService'], function (ko, events, dataService) {
    
    function Beer(model) {
        var self = this;

        self.name = ko.computed(function () {
            return ko.unwrap(model.name);
        });
        self.brewery = ko.computed(function() {
            return ko.unwrap(model.brewery);
        });
        self.style = ko.computed(function () {
            return ko.unwrap(model.style);
        });
    };

    return {
        viewModel: Beer,
        template: { element: 'beer-template' }
    };
});