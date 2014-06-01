define(['jquery', 'ko'], function($, ko) {
    function Beer(beerJSON, brewery, style) {
        var self = this;

        self.id = ko.observable(beerJSON.Id);
        self.name = ko.observable(beerJSON.Name);
        self.abv = ko.observable(beerJSON.ABV);

        self.style = ko.observable(style);
        self.brewery = ko.observable(brewery);
    };

    return Beer;
});