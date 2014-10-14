define(['jquery', 'ko', 'text!/extras/hat/template.html'], function ($, ko, template) {
    var Hat = function(keg) {
        var self = this;

        self.keg = keg;
        self.template = template;
        self.isActive = ko.observable(true);
        self.someValue = ko.observable('omg hat!');
    };

    Hat.prototype = {
        cancel: function () {
            var deferred = new $.Deferred();
            deferred.success();

            return deferred.promise();
        }
    };

    return Hat;
});