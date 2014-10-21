define(['ko'], function (ko) {

    function Config(tap) {
        var self = this;

        self.isActive = ko.observable(true);
        self.rank = ko.observable(1);
    };

    return Config;
});