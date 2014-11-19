define(['jquery', 'app/model/tap', 'app/model/keg', 'app/model/beer', 'app/model/brewery', 'app/model/style'], function ($, Tap, Keg, Beer, Brewery, Style) {
    var dataService = {};

    dataService.getCurrentTaps = function () {
        var df = $.Deferred();

        $.get("/api/beerOnTap")
            .done(
                function(beerOnTapJson) {
                    var taps = [];
                    beerOnTapJson.forEach(function(data) {
                        var tap = new Tap(data.Tap),
                            brewery, style,  beer, keg;
                        if (data.Keg != null) {
                            brewery = new Brewery(data.Brewery);
                            style = new Style(data.Style);
                            beer = new Beer(data.Beer, brewery, style);
                            keg = new Keg(data.Keg, beer);
                            tap.loadKeg(tap.id(), keg);
                        }
                        taps.push(tap);
                    });
                    df.resolve(taps);
                })
            .fail(df.reject);

        return df.promise();
    };

    dataService.getKegFromTapId = function(tapId) {
        var df = $.Deferred();

        $.get('/api/beerOnTap/' + tapId).done(function(data) {
            var brewery, style, beer, keg = null;

            if (data.Keg != null) {
                brewery = new Brewery(data.Brewery);
                style = new Style(data.Style);
                beer = new Beer(data.Beer, brewery, style);
                keg = new Keg(data.Keg, beer);
            }
            df.resolve(keg);
        }).fail(df.reject);

        return df.promise();
    }

    return dataService;
});