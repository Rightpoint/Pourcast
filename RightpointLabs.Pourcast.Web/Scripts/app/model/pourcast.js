define(['ko', 'jquery', 'app/componentResolver', 'app/events', 'app/dataservice'], function (ko, $, ComponentResolver, events, dataService) {

    function Pourcast() {
        var self = this;

        self.taps = ko.observableArray([]);
        self.resolver = new ComponentResolver(this);

        self.resolver.register('pourcast', 'pourcast');

        events.on("Reconnected", self.loadTaps);

        self.loadTaps();
    };

    Pourcast.prototype = {
        loadTaps: function () {
            var self = this;

            dataService.getCurrentTaps()
                .done(function (taps) {
                    self.taps(taps);
                });
        }
    };

    return Pourcast;
});