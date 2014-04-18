/*
    Brewery View Model
*/
(function(app, $, ko, toast, moment) {
    app.BreweryVM = app.BreweryVM || function(breweries) {
        var self = this;
        self.breweries = ko.observableArray(breweries || []);
        self.createVisible = ko.observable(false);
        self.newBrewery = ko.observable();

        self.create = function() {
            self.newBrewery(new app.Brewery());
            self.createVisible(true);
        };

        self.save = function() {
            var brewery = self.newBrewery();
            
            if (brewery.name()) {
                self.breweries.push(brewery);
                toast.success("Created " + brewery.name());
                self.newBrewery(null);
                self.createVisible(false);
            }
        };

    };
}(window.pourcast = window.pourcast || {}, jQuery, ko, toastr, moment));