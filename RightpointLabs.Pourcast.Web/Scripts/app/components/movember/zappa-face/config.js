define(['ko'], function (ko) {

    var count = 0;

    function Config(tap) {
        var self = this;

        self.isActive = ko.observable(count === 1);
        self.rank = ko.observable(2);

        count++;
    };

    return Config;
});