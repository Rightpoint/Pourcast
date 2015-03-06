define('app/components/hat/config', ['ko'], function (ko) {

    var count = 0;

    function Config(tap) {
        var self = this;

        self.isActive = ko.observable(count === 0);
        self.rank = ko.observable(2);

        count++;

        //setInterval(function () {
        //    self.isActive(!self.isActive());
        //}, 2000);
    };

    return Config;
});