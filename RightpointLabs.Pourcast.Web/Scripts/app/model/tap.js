var pourcast = pourcast || {};

pourcast.Tap = (function ($, ko) {

    function Tap(tapJSON) {
        var self = this;

        self.id = ko.observable(tapJSON.Id);
        self.name = ko.observable(tapJSON.Name);
        self.hasKeg = ko.observable(tapJSON.HasKeg);
        self.keg = ko.observable();

        pourcast.events.on("KegRemovedFromTap", self.kegRemovedFromTap);
        pourcast.events.on("KegTapped", self.kegTapped);

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
                console.log(beerOnTapJSON);

                var brewery = new pourcast.Brewery(beerOnTapJSON.Brewery);
                var beer = new pourcast.Beer(beerOnTapJSON.Beer, brewery);
                var keg = new pourcast.Keg(beerOnTapJSON.Keg, beer);

                self.keg(keg);
            }
        );
    };

    return Tap;
}(jQuery, ko));