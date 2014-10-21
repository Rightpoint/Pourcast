define(['ko'], function (ko) {

    function Config(tap) {
        var self = this;

        self.isActive = ko.computed(function () {
            return true;
        });

        rank: ko.computed(function () {
            return 1;
        });
    };

    return Config;
});