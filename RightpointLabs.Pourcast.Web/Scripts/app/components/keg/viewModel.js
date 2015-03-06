define('app/components/keg/viewModel', ['ko', 'app/events', 'app/dataService'], function (ko, events, dataService) {
    
    function Keg(model) {
        var self = this;

        self.isPouring = ko.computed(function() {
            return ko.unwrap(model.isPouring);
        });
        self.beerHeight = ko.computed(function() {
            return (ko.unwrap(model.percentRemaining) * 100) + '%';
        });
        self.beerColor = ko.computed(function() {
            return ko.unwrap(ko.unwrap(ko.unwrap(model.beer).style).color);
        })
        self.beer = ko.computed(function () {
            return ko.unwrap(model.beer);
        });
        self.keg = ko.computed(function() {
            return model;
        });
    };

    return {
        viewModel: Keg,
        template: { element: 'keg-template' }
    };
});