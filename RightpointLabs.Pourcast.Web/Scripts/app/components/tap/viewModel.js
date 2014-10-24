define(['ko', 'app/events', 'app/componentResolver', 'app/dataService', 'text!app/components/tap/template.html'], function (ko, events, ComponentResolver, dataService, htmlString) {
    
    function Tap(model) {
        var self = this;

        self.id = ko.observable(model.id);
        self.name = ko.observable(model.name);
        self.hasKeg = ko.observable(model.hasKeg);
        self.keg = ko.observable(model.keg);

        events.on("KegRemovedFromTap", self.removeKeg);
        events.on("KegTapped", self.tapKeg);

        self.resolver = new ComponentResolver();
        self.resolver.register('keg', 'keg');
        self.resolver.register('keg-body', 'kegBody');
        self.resolver.register('face', 'face');
        self.resolver.register('beer', 'beer');
        self.resolver.register('hat', 'outsideRing');
        self.resolver.register('percent', 'bits');
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