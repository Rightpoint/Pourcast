var pourcast = pourcast || {};

pourcast.Tap = (function ($, ko) {

    function Tap(tapJSON) {
        var self = this;

        self.id = ko.observable(tapJSON.Id);
        self.name = ko.observable(tapJSON.Name);
        self.hasKeg = ko.observable(tapJSON.HasKeg);
        self.keg = ko.observable();

        pourcast.events.on("KegRemovedFromTap", function(e) {
            self.kegRemovedFromTap(e);
        });

        pourcast.events.on("KegTapped", function(e) {
            self.kegTapped(e);
        });

        self.loadKeg();
    };

    Tap.prototype.kegRemovedFromTap = function(e) {
        if (e.TapId === this.id()) {
            this.loadKeg();
            console.log("KegRemovedFromTap");
        }
    };

    Tap.prototype.kegTapped = function(e) {
        if (e.TapId === this.id()) {
            this.loadKeg();
            console.log("KegTapped");
        }
    }

    Tap.prototype.loadKeg = function () {
        var self = this;

        $.get("/api/beerOnTap/" + self.id(),
            function (beerOnTapJSON) {
                var keg, beer, brewery;

                if (beerOnTapJSON.Keg != null) {
                    brewery = new pourcast.Brewery(beerOnTapJSON.Brewery);
                    beer = new pourcast.Beer(beerOnTapJSON.Beer, brewery);
                    keg = new pourcast.Keg(beerOnTapJSON.Keg, beer);
                }

                self.keg(keg);
            }
        );
    };

    return Tap;
}(jQuery, ko));