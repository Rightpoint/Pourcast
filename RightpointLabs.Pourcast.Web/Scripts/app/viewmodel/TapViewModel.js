define('TapViewModel', ['jquery', 'ko', 'app/events', 'app/dataService'], function ($, ko, events, dataService) {
    var TapViewModel = function() {
        var self = this;

        self.taps = ko.observableArray();

        events.on("KegRemovedFromTap", function (e) {
            self.taps().foreach(function(tap) {
                tap.removeKeg(e.TapId);
            });
        });

        events.on("KegTapped", function (e) {
            dataService.getKegFromTapId(e.TapId).done(function(keg) {
                self.taps().foreach(function(tap) {
                    tap.loadKeg(e.TapId, keg);
                });
            });
        });

        (function() {
            if (dataService) {
                dataService.getCurrentTaps().done(function(taps) {
                    for (var i = 0; i < taps.length; i++) {
                        self.taps.push(taps[i]);
                    }
                });
            }
        }());

    };

    return TapViewModel;
});