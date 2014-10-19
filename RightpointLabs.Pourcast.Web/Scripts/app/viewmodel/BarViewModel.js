define('BarViewModel', ['jquery', 'ko', 'app/events', 'app/dataservice'], function ($, ko, events, dataService) {
    var BarViewModel = function() {
        var self = this;
        self.taps = ko.observableArray();
        self.dataService = dataService;

        var reloadKeg = function (tapId) {
            self.dataService.getKegFromTapId(tapId).done(function (keg) {
                console.log(tapId, keg);
                for (var i = 0; i < self.taps().length; i++) {
                    self.taps()[i].loadKeg(tapId, keg);
                }
            });
        }

        events.on("KegRemovedFromTap", function (e) {
            for (var i = 0; i < self.taps().length; i++) {
                self.taps()[i].removeKeg(e.TapId);
            }
        });

        events.on("KegTapped", function (e) {
            reloadKeg(e.TapId);
        });
        events.on("Reconnected", function (e) {
            self.taps().forEach(function (tap) {
                reloadKeg(tap.id);
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

    return BarViewModel;
});