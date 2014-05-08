var pourcast = pourcast || {};

pourcast.Brewery = (function ($, ko) {
    function Brewery(breweryJSON) {
        var self = this;

        self.id = ko.observable(breweryJSON.Id);
        self.name = ko.observable(breweryJSON.Name);
        self.city = ko.observable(breweryJSON.City);
        self.state = ko.observable(breweryJSON.State);
        self.website = ko.observable(breweryJSON.Website);
    };

    return Brewery;
}(jQuery, ko));