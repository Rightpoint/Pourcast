/*
    Brewery View Model
*/
(function (app, $, ko, toast, moment) {
    app.BeerVM = app.BeerVM|| function () {
        var self = this;
        self.createVisible = ko.observable(false);
        self.newBeer = ko.observable();

        self.create = function () {
            self.newBeer(new app.Beer());
            self.createVisible(true);
        };

        self.save = function () {
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