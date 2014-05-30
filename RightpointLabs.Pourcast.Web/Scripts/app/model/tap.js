define('Tap', ['jquery', 'ko'], function ($, ko) {
    
    function Tap(tapJSON) {
        var self = this;

        self.id = ko.observable(tapJSON.Id);
        self.name = ko.observable(tapJSON.Name);
        self.hasKeg = ko.observable(tapJSON.HasKeg);
        self.keg = ko.observable();
    };

    Tap.prototype.removeKeg = function (tapId) {
        this.loadKeg(tapId, null);
    };

    Tap.prototype.loadKeg = function(tapId, keg) {
        var self = this;

        if (tapId === this.id()) {
            self.keg(keg);
        }
    };

    return Tap;
});