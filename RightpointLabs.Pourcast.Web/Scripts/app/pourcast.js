define(['jquery', 'ko', 'BarViewModel'], function ($, ko, BarViewModel) {
    var pub = { };

    pub.init = function () {
        pub.barViewModel = new BarViewModel();
        pub.barViewModel.init().done(ko.applyBindings(pub));
    };

    return pub;
});