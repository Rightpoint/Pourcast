define(['ko', 'app/events'], function (ko, events) {
    function Keg(kegJSON, beer) {
        var self = this;

        self.id = ko.observable(kegJSON.id);
        self.percentRemaining = ko.observable(kegJSON.percentRemaining);
        self.isEmpty = ko.observable(kegJSON.isEmpty);
        self.isPouring = ko.observable(kegJSON.isPouring);
        self.capacity = ko.observable(kegJSON.capacity);
        self.beer = ko.observable(beer);
        self.isLow = ko.computed(function () {
            return ko.unwrap(self.percentRemaining) < .25;
        });

        events.on("PourStarted", function(e) {
            self.pourStarted(e);
        });
        events.on("Pouring", function(e) {
            self.pouring(e);
        });
        events.on("PourStopped", function(e) {
            self.pourStopped(e);
        });
    };

    Keg.prototype = {
        pourStarted: function (e) {
            console.log("PourStarted");
            var self = this;

            if (e.KegId === self.id()) {
                self.isPouring(true);
            }
        },

        pouring: function(e) {
            console.log("Pouring");
            var self = this;

            if (e.KegId === self.id()) {
                self.percentRemaining(e.PercentRemaining);
            }
        },

        pourStopped: function(e) {
            console.log("PourStopped");
            var self = this;

            if (e.KegId === self.id()) {
                self.isPouring(false);
                self.percentRemaining(e.PercentRemaining);
            }
        }
    }

    return Keg;
});
