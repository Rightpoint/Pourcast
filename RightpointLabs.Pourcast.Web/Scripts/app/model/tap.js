(function (app, $, ko, toast, moment) {
    app.Tap = app.Tap || function (tapJSON) {
        var self = this;

        self.id = ko.observable(tapJSON.Id);
        self.name = ko.observable(tapJSON.Name);
        self.hasKeg = ko.observable(tapJSON.HasKeg);
        self.keg = ko.observable();

        self.loadKeg();

        app.events.on("KegRemovedFromTap", function(e) {
            if (e.TapId === self.id()) {
                self.keg(null);
                console.log("KegRemovedFromTap");
            }
        });

        app.events.on("KegTapped", function(e) {
            if (e.TapId === self.id()) {
                self.loadKeg();
                console.log("KegTapped");
            }
        });
    };

    app.Tap.prototype.loadKeg = function () {
        var self = this;

        $.get("/api/beerOnTap/" + self.id(),
            function (beerOnTapJSON) {
                console.log(beerOnTapJSON);

                var brewery = new app.Brewery(beerOnTapJSON.Brewery);
                var beer = new app.Beer(beerOnTapJSON.Beer, brewery);
                var keg = new app.Keg(beerOnTapJSON.Keg, beer);

                self.keg(keg);
            }
        );
    };
}(window.pourcast = window.pourcast || {}, jQuery, ko, toastr, moment));