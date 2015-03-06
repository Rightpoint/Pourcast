define('app/components/tap/config', ['ko'], function (ko) {

    function Config(model) {
        var self = this;

        self.isActive = ko.observable(true);
        self.rank = ko.observable(1);
    };

    return Config;
});