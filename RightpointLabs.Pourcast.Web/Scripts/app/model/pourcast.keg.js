(function(app, $, ko, toast, moment) {
    app.Keg = app.Keg || function(tapId, tapName, initVolume, beer, pours) {
        var self = this;
        self.tapId = ko.observable(tapId || 0);
        self.tapName = ko.observable(tapName || "");
        self.initalVolume = initVolume;
        self.beer = beer || null;
        self.pours = ko.observableArray(pours || []);

        self.volume = ko.computed(function() {
            var pourAmt = 0;
            ko.utils.arrayForEach(self.pours, function(pour) {
                pourAmt += pour.volume();
            });

            return self.initalVolume - pourAmt;
        });
    };
}(window.pourcast = window.pourcast || {}, jQuery, ko, toastr, moment));