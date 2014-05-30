define(['jquery', 'ko', 'events'], function($, ko, events) {
    function Keg(kegJSON, beer) {
        var self = this;

        self.id = ko.observable(kegJSON.Id);
        self.percentRemaining = ko.observable(kegJSON.PercentRemaining * 100 + "%");
        self.isEmpty = ko.observable(kegJSON.IsEmpty);
        self.isPouring = ko.observable(kegJSON.IsPouring);
        self.capacity = ko.observable(kegJSON.Capacity);
        self.beer = ko.observable(beer);

        self.percentRemainingBubble = ko.computed(function() {
            return (kegJSON.PercentRemaining * 100) > 25 ? "high" : "low";
        });

        events.on("PourStarted", self.pourStarted);
        events.on("PourStopped", self.pourStopped);
    };

    Keg.prototype.pourStarted = function(e) {
        console.log("PourStarted");
    };

    Keg.prototype.pourStopped = function(e) {
        console.log("PourStopped");
    };

    return Keg;
});