define(['jquery', 'ko'], function ($, ko) {

    function DefaultStatusMarker(params) {
        var self = this;
        self.model = params.model;

        self.position = ko.computed(function () {
            var pct = self.model().percentRemaining();
            if (pct >= 85) {
                return -50;
            } else if (pct <= 15) {
                return -130;
            } else {
                // math to get offset...

                // normalize percentage to the range we're moving the marker
                pct = (pct - 15) / (85 - 15);

                // and now convert to degrees
                return -130 + (130 - 50) * pct;
            }
        });

        self.bitTransform = ko.computed(function () {
            return "rotate(" + self.position() + "deg) translateY(-220%) rotate(-45deg)";
        });

        self.contentsTransform = ko.computed(function () {
            var pos = 45 - self.position();
            return "rotate(" + pos + "deg)";
        });
    };

    return DefaultStatusMarker;
});