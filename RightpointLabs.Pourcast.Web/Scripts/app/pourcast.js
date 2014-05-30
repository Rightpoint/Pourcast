define(['jquery', 'ko', 'TapViewModel'], function($, ko, TapViewModel) {
    var pub = { };

    pub.init = function () {
        pub.tapViewModel = new TapViewModel();
        pub.tapViewModel.init();
        ko.applyBindings(pub);
    };

    return pub;
});