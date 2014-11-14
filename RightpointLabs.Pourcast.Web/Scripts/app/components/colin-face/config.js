define(['ko'], function (ko) {

    var count = 0;

    function Config(tap) {
        var self = this;

        self.isActive = ko.observable(count === 1);
        self.rank = ko.observable(2);

        count++;

        //setInterval(function () {
        //    self.isActive(!self.isActive());
        //}, 2000);
    };

    return Config;
});