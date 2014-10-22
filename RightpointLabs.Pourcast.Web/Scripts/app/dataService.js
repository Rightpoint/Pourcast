define(['jquery'], function ($) {
    var dataService = {

        getTaps: function () {
            return $.get("/api/beerOnTap")
                .then(function (beerOnTapJson) {
                    var taps = [];
                    beerOnTapJson.forEach(function (data) {
                        var tap = data.Tap;

                        if (data.Keg != null) {
                            tap.Keg = data.Keg;
                            tap.Keg.Beer = data.Beer;
                            tap.Keg.Beer.Style = data.Style;
                            tap.Keg.Beer.Brewery = data.Brewery;
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
                        keg.Beer = data.Beer;
                        keg.Beer.Style = data.Style;
                        keg.Beer.Brewery = data.Brewery;
                    }

                    return keg;
                });
        }
    };

    return dataService;
});