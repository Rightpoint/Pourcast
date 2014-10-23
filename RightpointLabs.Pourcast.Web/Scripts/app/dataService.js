define(['jquery'], function ($) {
    var dataService = {

        getTaps: function () {
            return $.get("/api/beerOnTap")
                .then(function (beerOnTapJson) {
                    var taps = [];
                    beerOnTapJson.forEach(function (data) {
                        var tap = data.tap;

                        if (data.keg != null) {
                            tap.keg = data.keg;
                            tap.keg.beer = data.beer;
                            tap.keg.beer.style = data.style;
                            tap.keg.beer.brewery = data.brewery;
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
                        keg = data.keg;
                        keg.beer = data.beer;
                        keg.beer.style = data.style;
                        keg.beer.brewery = data.brewery;
                    }

                    return keg;
                });
        }
    };

    return dataService;
});