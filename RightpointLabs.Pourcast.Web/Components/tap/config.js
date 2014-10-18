define(['ko', 'jquery'], function (ko, $) {

    function Config(tap) {
        var self = this;
        
        self.isActive = ko.computed(function() {
            return true;
        });

        rank: ko.computed(function() {
            return 0;
        });
    };

    return Config;
});