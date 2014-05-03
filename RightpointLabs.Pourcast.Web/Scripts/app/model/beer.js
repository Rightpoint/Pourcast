(function (app, $, ko, toast, moment) {
    app.Beer = app.Beer || function (beerJSON, brewery) {
        var self = this;

        self.id = ko.observable(beerJSON.Id);
        self.name = ko.observable(beerJSON.Name);
        self.abv = ko.observable(beerJSON.ABV);
        self.color = ko.observable(beerJSON.Color);
        self.glass = ko.observable(beerJSON.Glass);
        self.style = ko.observable(beerJSON.Style);

        self.brewery = ko.observable(brewery);
    };

}(window.pourcast = window.pourcast || {}, jQuery, ko, toastr, moment));