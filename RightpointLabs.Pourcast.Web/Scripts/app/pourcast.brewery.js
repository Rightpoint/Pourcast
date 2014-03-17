(function(app, $, ko, toast, moment) {
    app.Brewery = app.Brewery || function(name, city, state, beers) {
        this.name = ko.observable(name);
        this.city = ko.observable(city);
        this.state = ko.observable(state);
        this.beers = ko.observableArray(beers);
        this.location = ko.computed(this.getLocation, this);
    };

    app.Brewery.prototype = {
        getLocation: function() {
            return this.city() + ", " + this.state();
        }
    };

}(window.pourcast = window.pourcast || {}, jQuery, ko, toastr, moment));