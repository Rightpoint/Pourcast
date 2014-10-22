define(['ko', 'app/events', 'app/dataservice', 'text!app/components/tap/template.html'], function (ko, events, dataService, htmlString) {
    
    function Tap(model) {
        var self = this;

        self.id = ko.observable(model.Id);
        self.name = ko.observable(model.Name);
        self.hasKeg = ko.observable(model.HasKeg);
        self.keg = ko.observable(model.Keg);

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