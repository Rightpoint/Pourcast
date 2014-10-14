define(['jquery', 'ko', 'text!/extras/hat/template.html'], function ($, ko, template) {
    var Example = function (keg) {
        var self = this;

        self.keg = keg;
        self.template = template;
        self.importance = ko.observable(1);
        self.isActive = ko.observable(true);
        self.someValue = ko.observable(5);
    };

    Example.prototype = {
        tryCancel: function () {
            var deferred = new $.Deferred();
            deferred.success();

            return deferred.promise();
        }
    };

    return Example;
});