define(['jquery', 'ko'], function ($, ko) {

    function DefaultStatusMarker(params) {
        var self = this;
        self.model = params.model;
    };

    return DefaultStatusMarker;
});