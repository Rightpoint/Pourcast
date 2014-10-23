define(['ko', 'app/events', 'app/dataService', 'text!app/components/tap/template.html'], function (ko, events, dataService, htmlString) {
    
    function Tap(model) {
        var self = this;

        self.id = ko.observable(model.id);
        self.name = ko.observable(model.name);
        self.hasKeg = ko.observable(model.hasKeg);
        self.keg = ko.observable(model.keg);

        events.on("KegRemovedFromTap", self.removeKeg);
        events.on("kegTapped", self.removeKeg);
    };

    Tap.prototype = {
        removeKeg: function (e) {
            self.keg(null);
        },
        tapKeg: function (e) {
            var self = this;

            dataService.getKegFromTapId(e.TapId).done(function (keg) {
                self.keg(keg);
            });
        }
    };

    return {
        viewModel: Tap,
        template: htmlString
    };
});