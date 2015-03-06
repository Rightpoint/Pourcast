define('app/model/brewery', ['ko'], function(ko) {
    function Brewery(breweryJSON) {
        var self = this;

        self.id = ko.observable(breweryJSON.id);
        self.name = ko.observable(breweryJSON.name);
        self.city = ko.observable(breweryJSON.city);
        self.state = ko.observable(breweryJSON.state);
        self.website = ko.observable(breweryJSON.website);
        self.location = ko.computed(function() {
            return ko.unwrap(self.city) + ", " + ko.unwrap(self.state);
        });
    };

    return Brewery;
});