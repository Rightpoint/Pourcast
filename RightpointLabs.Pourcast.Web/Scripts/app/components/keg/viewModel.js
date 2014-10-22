define(['ko', 'app/events', 'app/dataservice', 'text!app/components/keg/template.html'], function (ko, events, dataService, htmlString) {
    
    function Keg(model) {
        var self = this;

        self.id = ko.observable(model.Id);
        self.percentRemaining = ko.observable(decimalToPercent(model.PercentRemaining));
        self.isEmpty = ko.observable(model.IsEmpty);
        self.isPouring = ko.observable(model.IsPouring);
        self.capacity = ko.observable(model.Capacity);
        self.beer = ko.observable(model.Beer);

        self.isLow = ko.computed(function () {
            return self.percentRemaining() < 25;
        });

        self.percentRemainingStyle = ko.computed(function () {
            return Math.floor(self.percentRemaining()) + '%';
        });
        self.percentRemainingHtml = ko.computed(function () {
            return parseFloat(self.percentRemaining()).toFixed(1) + '<span class="symbol">%</span>';
        });
        self.percentRemainingClass = ko.computed(function () {
            return self.isLow() ? "low" : "high";
        });

        events.on("PourStarted", self.pourStarted);
        events.on("Pouring", self.pouring);
        events.on("PourStopped", self.pourStopped);
    };

    Keg.prototype = {
        pourStarted: function (e) {
            console.log("PourStarted");
            var self = this;

            if (e.KegId === self.id()) {
                self.isPouring(true);
            }
        },

        pouring: function (e) {
            console.log("Pouring");
            var self = this;

            if (e.KegId === self.id()) {
                self.percentRemaining(decimalToPercent(e.PercentRemaining));
            }
        },

        pourStopped: function (e) {
            console.log("PourStopped");
            var self = this;

            if (e.KegId === self.id()) {
                self.isPouring(false);
                self.percentRemaining(decimalToPercent(e.PercentRemaining));
            }
        }
    }

    function decimalToPercent(decimal) {
        return (decimal * 100).toFixed(1);
    }

    return {
        viewModel: Keg,
        template: htmlString
    };
});