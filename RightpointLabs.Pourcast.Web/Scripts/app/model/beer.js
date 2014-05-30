define('Beer', ['jquery', 'ko'], function($, ko) {
    function Beer(beerJSON, brewery) {
        var self = this;

        self.id = ko.observable(beerJSON.Id);
        self.name = ko.observable(beerJSON.Name);
        self.abv = ko.observable(beerJSON.ABV);
        self.color = ko.observable(beerJSON.Color);

        self.backgroundColor = ko.computed(function() {
            return self.color();
        });

        self.glass = ko.observable(beerJSON.Glass);
        self.style = ko.observable(beerJSON.Style);

        self.brewery = ko.observable(brewery);
    };

    return Beer;
});