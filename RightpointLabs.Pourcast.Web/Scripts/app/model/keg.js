define(['jquery', 'ko', 'app/events'], function ($, ko, events) {
    function Keg(kegJSON, beer) {
        var self = this;

        self.id = ko.observable(kegJSON.Id);
        self.percentRemaining = ko.observable(this.decimalToPercent(kegJSON.PercentRemaining));
        self.isEmpty = ko.observable(kegJSON.IsEmpty);
        self.isPouring = ko.observable(kegJSON.IsPouring);
        self.capacity = ko.observable(kegJSON.Capacity);
        self.beer = ko.observable(beer);
        self.accessories = ko.observableArray();
        
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

        events.on("PourStarted", function (e) {
            console.log("PourStarted");
            if (e.KegId === self.id()) {
                self.isPouring(true);
            }
        });
        events.on("Pouring", function(e) {
            console.log("Pouring");

            if (e.KegId === self.id()) {
                self.percentRemaining(this.decimalToPercent(e.PercentRemaining));
            }
        });
        events.on("PourStopped", function(e) {
            console.log("PourStopped");

            if (e.KegId === self.id()) {
                self.isPouring(false);
                self.percentRemaining(this.decimalToPercent(e.PercentRemaining));
            }
        });

        this.loadAccessories(['/extras/hat/module.js']);
    };

    Keg.prototype = {
        loadAccessories: function (accessories) {
            var self = this;

            accessories.forEach(function (accessoryModule) {
                require([accessoryModule], function(Accessory) {
                    var accessory = new Accessory(self);
                    accessory.id = 'accessory-' + (Math.random() + '').split('.')[1];
                    self.accessories.push(accessory);
                });
            });
        },
        decimalToPercent: function(decimal) {
            return (decimal * 100).toFixed(1);
        }
    };

    return Keg;
});
