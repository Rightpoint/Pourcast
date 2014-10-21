define(['ko'], function (ko) {

    function Config(pourcast) {
        var self = this;

        self.pourcast = ko.observable(pourcast);

        self.isActive = ko.computed(function() {
            return true;
        });

        rank: ko.computed(function() {
            return 0;
        });
    };

    return Config;
});