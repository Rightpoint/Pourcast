var pourcast = pourcast || {};

pourcast.app = (function ($, ko, toast, moment) {
    var pub = {};

    pub.taps = ko.observableArray();

    var loadTaps = function () {
        $.get("/api/tap",
            function (tapsJSON) {
                tapsJSON.forEach(function (tapJSON) {
                    var tap = new pourcast.Tap(tapJSON);
                    pub.taps.push(tap);
                });
            }
        );
    };

    pub.init = function () {
        loadTaps();

        ko.applyBindings(pub);
    };

    return pub;
}(jQuery, ko, toastr, moment));