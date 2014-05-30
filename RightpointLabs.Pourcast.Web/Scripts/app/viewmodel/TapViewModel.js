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


        self.init = function() {
            dataService.getCurrentTaps().done(function(taps) {
                for (var i = 0; i < taps.length; i++) {
                    self.taps.push(taps[i]);
                }
            });
        };

    };

    return TapViewModel;
});