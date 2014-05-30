define(['jquery', 'TapViewModel'], function($, TapViewModel) {
    var pub = { };

    pub.init = function () {
        pub.tapViewModel = new TapViewModel();

        ko.applyBindings(pub);
    };

    return pub;
});