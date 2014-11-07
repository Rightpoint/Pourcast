define(['ko'], function (ko) {

    var count = 0;

    function Config(model) {
        var self = this;

        self.isActive = ko.observable(count === 0);
        self.rank = ko.observable(2);

        count++;
    };

    return Config;
});