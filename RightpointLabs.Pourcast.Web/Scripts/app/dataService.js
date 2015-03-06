define('app/dataService', ['jquery', 'app/model/tap', 'app/model/keg', 'app/model/beer', 'app/model/brewery', 'app/model/style', 'exports'], function($, Tap, Keg, Beer, Brewery, Style, exports) {

    exports.getTaps = function() {
        return $.get("/api/beerOnTap").then(function(beerOnTapJson) {
            var taps = [];

            beerOnTapJson.forEach(function(data) {
                var tap, brewery, style, beer, keg;

                if (data.keg != null) {
                    brewery = new Brewery(data.brewery);
                    style = new Style(data.style);
                    beer = new Beer(data.beer, brewery, style);
                    keg = new Keg(data.keg, beer);
                }

                tap = new Tap(data.tap, keg);
                taps.push(tap);
            });

            return taps;
        });
    };

    exports.getKegFromTapId = function(tapId) {
        return $.get('/api/beerOnTap/' + tapId).then(function(data) {
            var brewery, style, beer, keg;

            if (data.keg != null) {
                brewery = new Brewery(data.brewery);
                style = new Style(data.style);
                beer = new Beer(data.beer, brewery, style);
                keg = new Keg(data.keg, beer);
            }

            return keg;
        });
    };
});