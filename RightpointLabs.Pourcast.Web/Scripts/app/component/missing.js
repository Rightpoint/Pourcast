define(['jquery', 'ko'], function ($, ko) {

    function Missing(params) {
        var self = this;
        self.key = ko.observable(params.key);
        self.renderer = ko.observable(ko.utils.unwrapObservable(params.renderer));
    };

    return Missing;
});