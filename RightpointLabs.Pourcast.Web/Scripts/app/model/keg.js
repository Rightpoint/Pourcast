define(['jquery', 'ko', 'app/events'], function($, ko, events) {
    function Keg(kegJSON, beer) {
        var self = this;

        self.id = ko.observable(kegJSON.Id);
        self.percentRemaining = ko.observable(Math.floor(kegJSON.PercentRemaining * 100));
        self.isEmpty = ko.observable(kegJSON.IsEmpty);
        self.isPouring = ko.observable(kegJSON.IsPouring);
        self.capacity = ko.observable(kegJSON.Capacity);
        self.beer = ko.observable(beer);

        self.percentRemainingStyle = ko.computed(function () {
            return self.percentRemaining() + '%';
        });
        self.percentRemainingHtml = ko.computed(function() {
            return self.percentRemaining() + '<span class="symbol">%</span>';
        });
        self.percentRemainingBubble = ko.computed(function() {
            return self.percentRemaining() > 25 ? "high" : "low";
        });

        events.on("PourStarted", function(e) {
            console.log("PourStarted");
        });
        events.on("PourStopped", function(e) {
            console.log("PourStopped");

            self.percentRemaining(Math.floor(e.PercentRemaining * 100));
        });
    };

    return Keg;
});