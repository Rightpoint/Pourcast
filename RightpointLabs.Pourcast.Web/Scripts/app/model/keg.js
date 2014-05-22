var pourcast = pourcast || {};

pourcast.Keg = (function ($, ko) {
    function Keg(kegJSON, beer) {
        var self = this;

        self.id = ko.observable(kegJSON.Id);
        self.percentRemaining = ko.observable(kegJSON.PercentRemaining * 100 + "%");
        self.isEmpty = ko.observable(kegJSON.IsEmpty);
        self.isPouring = ko.observable(kegJSON.IsPouring);
        self.capacity = ko.observable(kegJSON.Capacity);
        self.beer = ko.observable(beer);

        pourcast.events.on("PourStarted", function(e) {
            self.pourStarted(e);
        });

        pourcast.events.on("PourStopped", function(e) {
            self.pourStopped(e);
        });
    };

    Keg.prototype.pourStarted = function(e) {
        console.log("PourStarted");
    };

    Keg.prototype.pourStopped = function (e) {
        console.log("PourStopped");
    };

    return Keg;
}(jQuery, ko));