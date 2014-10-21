define(['ko'], function (ko) {

    function Config(tap) {
        var self = this;

        self.isActive = ko.observable(true);
        self.rank = ko.observable(2);

        setInterval(function () {
            self.isActive(!self.isActive());
        }, 2000);
    };

    return Config;
});