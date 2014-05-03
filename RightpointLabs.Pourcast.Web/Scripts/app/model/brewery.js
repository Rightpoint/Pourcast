(function(app, $, ko, toast, moment) {
    app.Brewery = app.Brewery || function (breweryJSON) {
        var self = this;

        self.id = ko.observable(breweryJSON.Id);
        self.name = ko.observable(breweryJSON.Name);
        self.city = ko.observable(breweryJSON.City);
        self.state = ko.observable(breweryJSON.State);
        self.website = ko.observable(breweryJSON.Website);
    };

}(window.pourcast = window.pourcast || {}, jQuery, ko, toastr, moment));