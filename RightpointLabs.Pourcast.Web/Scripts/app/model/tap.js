define('app/model/tap', ['ko', 'app/events', 'app/dataService'], function (ko, events, dataService) {

    function Tap(tapJSON, keg) {
        var self = this;

        self.id = ko.observable(tapJSON.id);
        self.name = ko.observable(tapJSON.name);
        self.hasKeg = ko.observable(tapJSON.hasKeg);
        self.keg = ko.observable(keg);

        events.on("KegRemovedFromTap", function(e) {
            self.removeKeg(e);
        });
        events.on("kegTapped", function(e) {
            self.tapKeg(e);
        });
    };

    Tap.prototype = {
        removeKeg: function (e) {
            var self = this;

            if (e.TapId == self.id()) {
                self.keg(null);
            }
        },

        tapKeg: function (e) {
            var self = this;

            if (e.TapId == self.id()) {
                dataService.getKegFromTapId(e.TapId).done(function (keg) {
                    self.keg(keg);
                });
            }
        }
    };

    return Tap;
});