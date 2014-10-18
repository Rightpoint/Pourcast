define(['ko', 'app/componentResolver', 'app/events', 'app/dataservice'], function (ko, ComponentResolver, events, dataService) {
    
    function Tap(tapJSON) {
        var self = this;

        self.id = ko.observable(tapJSON.Id);
        self.name = ko.observable(tapJSON.Name);
        self.hasKeg = ko.observable(tapJSON.HasKeg);
        self.keg = ko.observable();

        self.resolver = new ComponentResolver(this);
        self.resolver.register('tap', 'tap');

        events.on("KegRemovedFromTap", self.removeKeg);
        events.on("kegTapped", self.removeKeg);
    };

    Tap.prototype = {
        removeKeg: function(e) {
            self.keg(null);
        },
        tapKeg: function (e) {
            var self = this;

            dataService.getKegFromTapId(e.TapId).done(function(keg) {
                self.keg(keg);
            });
        }
    };

    return Tap;
});