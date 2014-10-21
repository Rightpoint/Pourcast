define(['ko', 'componentResolver', 'app/events', 'app/dataservice', 'text!/components/tap/tap.html'], function (ko, ComponentResolver, events, dataService, htmlString) {
    
    function Tap(tapJson) {
        var self = this;

        self.id = ko.observable(tapJson.Id);
        self.name = ko.observable(tapJson.Name);
        self.hasKeg = ko.observable(tapJson.HasKeg);
        self.keg = ko.observable();
        self.resolver = new ComponentResolver();

        self.resolver.register('keg');
    };

    Tap.prototype = {
        removeKeg: function(tapId) {
            this.loadKeg(tapId, null);
        },
        loadKeg: function(tapId, keg) {
            var self = this;

            if (tapId === this.id()) {
                self.keg(keg);
            }
        },
    };

    return {
        viewModel: Tap,
        template: htmlString
    };
});