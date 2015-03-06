///#source 1 1 /Scripts/app/components/beer/config.js
define('app/components/beer/config', ['ko'], function (ko) {

    function Config(model) {
        var self = this;

        self.isActive = ko.observable(true);
        self.rank = ko.observable(1);
    };

    return Config;
});
///#source 1 1 /Scripts/app/components/beer/viewModel.js
define('app/components/beer/viewModel', ['ko', 'app/events', 'app/dataService'], function (ko, events, dataService) {
    
    function Beer(model) {
        var self = this;

        self.name = ko.computed(function () {
            return ko.unwrap(model.name);
        });
        self.brewery = ko.computed(function() {
            return ko.unwrap(model.brewery);
        });
        self.style = ko.computed(function () {
            return ko.unwrap(model.style);
        });
    };

    return {
        viewModel: Beer,
        template: { element: 'beer-template' }
    };
});
///#source 1 1 /Scripts/app/components/face/config.js
define('app/components/face/config', ['ko'], function (ko) {

    function Config(model) {
        var self = this;

        self.isActive = ko.observable(true);
        self.rank = ko.observable(1);
    };

    return Config;
});
///#source 1 1 /Scripts/app/components/face/viewModel.js
define('app/components/face/viewModel', ['ko', 'app/events', 'app/dataService'], function (ko, events, dataService) {
    
    function Face(model) {
        var self = this;

        self.isLow = ko.computed(function () {
            return ko.unwrap(model.isLow);
        });
    };

    return {
        viewModel: Face,
        template: { element: 'face-template' }
    };
});
///#source 1 1 /Scripts/app/components/hat/config.js
define('app/components/hat/config', ['ko'], function (ko) {

    var count = 0;

    function Config(tap) {
        var self = this;

        self.isActive = ko.observable(count === 0);
        self.rank = ko.observable(2);

        count++;

        //setInterval(function () {
        //    self.isActive(!self.isActive());
        //}, 2000);
    };

    return Config;
});
///#source 1 1 /Scripts/app/components/hat/viewModel.js
define('app/components/hat/viewModel', ['ko', 'text!app/components/hat/template.html'], function (ko, htmlString) {

    function Hat(model) {
        var self = this;
    };

    return {
        viewModel: Hat,
        template: htmlString
    };
});
///#source 1 1 /Scripts/app/components/keg/config.js
define('app/components/keg/config', ['ko'], function (ko) {

    function Config(model) {
        var self = this;

        self.isActive = ko.observable(true);
        self.rank = ko.observable(1);
    };

    return Config;
});
///#source 1 1 /Scripts/app/components/keg/viewModel.js
define('app/components/keg/viewModel', ['ko', 'app/events', 'app/dataService'], function (ko, events, dataService) {
    
    function Keg(model) {
        var self = this;

        self.isPouring = ko.computed(function() {
            return ko.unwrap(model.isPouring);
        });
        self.beerHeight = ko.computed(function() {
            return (ko.unwrap(model.percentRemaining) * 100) + '%';
        });
        self.beerColor = ko.computed(function() {
            return ko.unwrap(ko.unwrap(ko.unwrap(model.beer).style).color);
        })
        self.beer = ko.computed(function () {
            return ko.unwrap(model.beer);
        });
        self.keg = ko.computed(function() {
            return model;
        });
    };

    return {
        viewModel: Keg,
        template: { element: 'keg-template' }
    };
});
///#source 1 1 /Scripts/app/components/keg-body/config.js
define('app/components/keg-body/config', ['ko'], function (ko) {

    function Config(model) {
        var self = this;

        self.isActive = ko.observable(true);
        self.rank = ko.observable(1);
    };

    return Config;
});
///#source 1 1 /Scripts/app/components/keg-body/viewModel.js
define('app/components/keg-body/viewModel', ['ko', 'app/events', 'app/dataService'], function (ko, events, dataService) {
    
    function KegBody(model) {
        var self = this;
    };

    return {
        viewModel: KegBody,
        template: { element: 'keg-body-template' }
    };
});
///#source 1 1 /Scripts/app/components/missing/viewModel.js
define('app/components/missing/viewModel', ['ko', 'jquery'], function (ko, $) {

    function Missing() { };

    return {
        viewModel: Missing,
        template: { element: 'missing-template' }
    };
});
///#source 1 1 /Scripts/app/components/percent/config.js
define('app/components/percent/config', ['ko'], function (ko) {

    function Config(model) {
        var self = this;

        self.isActive = ko.observable(true);
        self.rank = ko.observable(1);
    };

    return Config;
});
///#source 1 1 /Scripts/app/components/percent/viewModel.js
define('app/components/percent/viewModel', ['ko', 'app/events', 'app/dataService'], function (ko, events, dataService) {
    
    function Percent(model) {
        var self = this;

        self.percentRemaining = ko.computed(function() {
            return (ko.unwrap(model.percentRemaining) * 100).toFixed(1);
        });
        self.levelCssClass = ko.computed(function () {
            return ko.unwrap(model.isLow) ? "low" : "high";
        });
    };

    return {
        viewModel: Percent,
        template: { element: 'percent-template' }
    };
});
///#source 1 1 /Scripts/app/components/pourcast/config.js
define('app/components/pourcast/config', ['ko'], function (ko) {

    function Config(model) {
        var self = this;

        self.isActive = ko.observable(true);
        self.rank = ko.observable(1);
    };

    return Config;
});
///#source 1 1 /Scripts/app/components/pourcast/viewModel.js
define('app/components/pourcast/viewModel', ['ko'], function (ko) {

    function Pourcast(model) {
        var self = this;

        self.taps = ko.computed(function() {
            return ko.unwrap(model.taps);
        });
    };

    return {
        viewModel: Pourcast,
        template: { element: 'pourcast-template' }
    };
});
///#source 1 1 /Scripts/app/components/tap/config.js
define('app/components/tap/config', ['ko'], function (ko) {

    function Config(model) {
        var self = this;

        self.isActive = ko.observable(true);
        self.rank = ko.observable(1);
    };

    return Config;
});
///#source 1 1 /Scripts/app/components/tap/viewModel.js
define('app/components/tap/viewModel', ['ko', 'app/componentResolver'], function (ko, ComponentResolver) {
    
    function Tap(model) {
        var self = this;

        self.hasKeg = ko.computed(function() {
            return ko.unwrap(model.hasKeg);
        });
        self.keg = ko.computed(function() {
            return ko.unwrap(model.keg);
        });

        self.resolver = new ComponentResolver();
        self.resolver.register('keg', 'keg');
        self.resolver.register('keg-body', 'kegBody');
        self.resolver.register('face', 'face');
        self.resolver.register('beer', 'beer');
        self.resolver.register('percent', 'bits');
    };

    return {
        viewModel: Tap,
        template: { element: 'tap-template' }
    };
});
