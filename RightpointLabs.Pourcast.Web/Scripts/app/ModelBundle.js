///#source 1 1 /Scripts/app/model/beer.js
define('app/model/beer', ['ko'], function(ko) {
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
///#source 1 1 /Scripts/app/model/brewery.js
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
///#source 1 1 /Scripts/app/model/keg.js
define('app/model/keg', ['ko', 'app/events'], function (ko, events) {
    function Keg(kegJSON, beer) {
        var self = this;

        self.id = ko.observable(kegJSON.id);
        self.percentRemaining = ko.observable(kegJSON.percentRemaining);
        self.isEmpty = ko.observable(kegJSON.isEmpty);
        self.isPouring = ko.observable(kegJSON.isPouring);
        self.capacity = ko.observable(kegJSON.capacity);
        self.beer = ko.observable(beer);
        self.isLow = ko.computed(function () {
            return ko.unwrap(self.percentRemaining) < .25;
        });

        events.on("PourStarted", function(e) {
            self.pourStarted(e);
        });
        events.on("Pouring", function(e) {
            self.pouring(e);
        });
        events.on("PourStopped", function(e) {
            self.pourStopped(e);
        });
    };

    Keg.prototype = {
        pourStarted: function (e) {
            console.log("PourStarted");
            var self = this;

            if (e.KegId === self.id()) {
                self.isPouring(true);
            }
        },

        pouring: function(e) {
            console.log("Pouring");
            var self = this;

            if (e.KegId === self.id()) {
                self.percentRemaining(e.PercentRemaining);
            }
        },

        pourStopped: function(e) {
            console.log("PourStopped");
            var self = this;

            if (e.KegId === self.id()) {
                self.isPouring(false);
                self.percentRemaining(e.PercentRemaining);
            }
        }
    }

    return Keg;
});

///#source 1 1 /Scripts/app/model/style.js
define('app/model/style', ['ko'], function (ko) {
    function Style(styleJSON) {
        var self = this;

        self.id = ko.observable(styleJSON.id);
        self.name = ko.observable(styleJSON.name);
        self.color = ko.observable(styleJSON.color);
        self.glass = ko.observable(styleJSON.glass);
    };

    return Style;
});
///#source 1 1 /Scripts/app/model/tap.js
define('app/model/tap', ['ko', 'app/events', 'app/dataService'], function (ko, events, dataService) {

    function Tap(tapJSON, keg) {
        var self = this;

        self.id = ko.observable(tapJSON.id);
        self.name = ko.observable(tapJSON.name);
        self.hasKeg = ko.observable(tapJSON.hasKeg);
        self.keg = ko.observable(keg);

        events.on("KegRemovedFromTap", function(e) {
            self.removeKeg(e);
        });
        events.on("kegTapped", function(e) {
            self.tapKeg(e);
        });
    };

    Tap.prototype = {
        removeKeg: function (e) {
            var self = this;

            if (e.TapId == self.id()) {
                self.keg(null);
            }
        },

        tapKeg: function (e) {
            var self = this;

            if (e.TapId == self.id()) {
                dataService.getKegFromTapId(e.TapId).done(function (keg) {
                    self.keg(keg);
                });
            }
        }
    };

    return Tap;
});
