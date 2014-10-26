define(['ko', 'app/events', 'app/dataService', 'text!app/components/keg/template.html'], function (ko, events, dataService, htmlString) {
    
    function Keg(model) {
        var self = this;

        self.isPouring = ko.computed(function() {
            return ko.unwrap(model.isPouring);
        });
        self.beerHeight = ko.computed(function() {
            return decimalToPercent(ko.unwrap(model.percentRemaining)) + '%';
        });
        self.beer = ko.computed(function () {
            return ko.unwrap(model.beer);
        });
        self.keg = ko.computed(function() {
            return model;
        });
    };

    function decimalToPercent(decimal) {
        return (decimal * 100).toFixed(1);
    }

    return {
        viewModel: Keg,
        template: htmlString
    };
});