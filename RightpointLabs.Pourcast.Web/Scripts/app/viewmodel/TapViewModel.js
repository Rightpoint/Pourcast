define('TapViewModel', ['jquery', 'ko', 'app/events', 'app/dataservice'], function ($, ko, events, dataService) {
    var TapViewModel = function() {
        var self = this;
        self.taps = ko.observableArray();

        events.on("KegRemovedFromTap", function (e) {
            for (var i = 0; i < self.taps.length; i++) {
                self.taps()[i].removeKeg(e.TapId);
            }
        });

        events.on("KegTapped", function (e) {
            self.dataService.getKegFromTapId(e.TapId).done(function (keg) {
                for (var i = 0; i < self.taps.length; i++) {
                    self.taps()[i].loadKeg(e.TapId, keg);
                }
            });
        });


        self.init = function () {
            var df = $.Deferred();

            dataService.getCurrentTaps().done(function (taps) {
                if (queryObj().flip) {
                    taps.reverse();
                }

                for (var i = 0; i < taps.length; i++) {
                    self.taps.push(taps[i]);
                }
                df.resolve();
            });

            return df.promise();
        };

    };

    function queryObj() {
        var result = {}, keyValuePairs = location.search.slice(1).split('&');

        keyValuePairs.forEach(function (keyValuePair) {
            keyValuePair = keyValuePair.split('=');
            result[keyValuePair[0]] = keyValuePair[1] || '';
        });

        return result;
    }

    return TapViewModel;
});