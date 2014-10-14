define(['jquery', 'ko', 'text!/extras/hat/template.html'], function ($, ko, template) {
    var Example = function (keg) {
        var self = this;

        // enables flair to be dependent on keg state
        self.keg = keg;

        // defines the markup to be used with this flair (required)
        self.template = template;

        // provides a sortable rank value (required for emotions/attitudes)
        // returns a number
        self.importance = ko.observable(1);

        // indicates if the flair is active (required)
        // returns a boolean
        self.isActive = ko.observable(true);

        // additional values can be added as necessary
        self.someValue = ko.observable(5);
    };

    Example.prototype = {
        // function that initiates the async cancellation of this flair (required)
        // returns a promise
        cancel: function () {
            var deferred = new $.Deferred();
            deferred.success();

            return deferred.promise();
        }
    };

    return Example;
});