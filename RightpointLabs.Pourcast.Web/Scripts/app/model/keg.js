(function(app, $, ko, toast, moment) {
    app.Keg = app.Keg || function(kegJSON, beer) {
        var self = this;

        self.id = ko.observable(kegJSON.Id);
        self.percentRemaining = ko.observable(kegJSON.PercentRemaining * 100 + "%");
        self.isEmpty = ko.observable(kegJSON.IsEmpty);
        self.isPouring = ko.observable(kegJSON.IsPouring);
        self.capacity = ko.observable(kegJSON.Capacity);
        self.beer = ko.observable(beer);

        app.events.on("PourStarted", function(e) {

        });

        app.events.on("PourStopped", function (e) {

        });
    };

    app.Keg.prototype.bound = function(element) {
        alert('hi');
    };
}(window.pourcast = window.pourcast || {}, jQuery, ko, toastr, moment));