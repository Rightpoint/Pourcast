define(['ko'], function(ko) {
    function Beer(beerJSON, brewery, style) {
        var self = this;

        self.id = ko.observable(beerJSON.id);
        self.name = ko.observable(beerJSON.name);
        self.abv = ko.observable(beerJSON.abv);
        self.style = ko.observable(style);
        self.brewery = ko.observable(brewery);
    };

    return Beer;
});