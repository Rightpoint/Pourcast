define(['jquery', 'app/model/tap', 'app/model/keg', 'app/model/beer', 'app/model/brewery', 'app/model/style'], function ($, Tap, Keg, Beer, Brewery, Style) {
    var dataService = {
        getCurrentTaps: function () {
            return $.get("/api/beerOnTap")
                .then(function (beerOnTapJson) {
                    var taps = [];
                    beerOnTapJson.forEach(function (data) {
                        var tap = new Tap(data.Tap);

                        if (data.Keg != null) {
                            var brewery = new Brewery(data.Brewery);
                            var style = new Style(data.Style);
                            var beer = new Beer(data.Beer, brewery, style);
                            var keg = new Keg(data.Keg, beer);

                            tap.keg(keg);
                            keg.beer(beer);
                            beer.style(style);
                            beer.brewery(brewery);
                        }
                        taps.push(tap);
                    });

                    return taps;
                });
        },

        getKegFromTapId: function(tapId) {
            return $.get('/api/beerOnTap/' + tapId)
                .then(function(data) {
                    var keg;

                    if (data.keg != null) {
                        var brewery = new Brewery(data.Brewery);
                        var style = new Style(data.Style);
                        var beer = new Beer(data.Beer, brewery, style);
                        keg = new Keg(data.Keg, beer);

                        tap.keg(keg);
                        keg.beer(beer);
                        beer.style(style);
                        beer.brewery(brewery);
                    }

                    return keg;
                });
        }
    };

    return dataService;
});