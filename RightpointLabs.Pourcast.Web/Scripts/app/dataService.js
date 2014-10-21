define(['jquery'], function ($) {
    var dataService = {
        getTaps: function () {
            return $.get("/api/beerOnTap")
                .then(function (beerOnTapJson) {
                    var taps = [];
                    beerOnTapJson.forEach(function (data) {
                        var tap = data.Tap;

                        if (data.Keg != null) {
                            tap.keg = data.Keg;
                            tap.keg.beer = data.Beer;
                            tap.keg.beer.style = data.Style;
                            tap.keg.beer.brewery = data.Brewery;
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
                        keg = data.Keg;
                        keg.beer = data.Beer;
                        keg.beer.style = data.Style;
                        keg.beer.brewery = data.Brewery;
                    }

                    return keg;
                });
        }
    };

    return dataService;
});