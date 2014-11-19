define(['ko', 'app/events', 'app/dataService', 'text!app/components/keg/template.html'], function (ko, events, dataService, htmlString) {
    
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
        template: htmlString
    };
});