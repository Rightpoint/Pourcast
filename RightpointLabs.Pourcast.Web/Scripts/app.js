requirejs.config({
    baseUrl: 'Scripts/libs/',
    //urlArgs: "bust=" + (new Date()).getTime(),
    urlArgs: "v=2",
    paths: {
        app: '../app',
        jquery: 'jquery-2.1.0',
        ko: 'knockout-3.2.0',
        signalr: 'jquery.signalR-2.0.3',
        'signalr.hubs': '/signalr/hubs?',
    },
    shim: {
        jquery: { exports: '$' },
        signalr: { deps: ['jquery'] },
        'signalr.hubs': { deps: ['signalr'] }
    },
    bundles: {
        'app/mainBundle': ['app/bindings', 'app/bubbles', 'app/camera', 'app/componentResolver', 'app/dataService', 'app/events'],
        'app/modelBundle': ['app/model/beer', 'app/model/brewery', 'app/model/keg', 'app/model/style', 'app/model/tap'],
        'app/componentsBundle': [
            'app/components/beer/config', 'app/components/beer/viewModel',
            'app/components/face/config', 'app/components/face/viewModel',
            'app/components/keg/config', 'app/components/keg/viewModel',
            'app/components/keg-body/config', 'app/components/keg-body/viewModel',
            'app/components/missing/viewModel',
            'app/components/percent/config', 'app/components/percent/viewModel',
            'app/components/pourcast/config', 'app/components/pourcast/viewModel',
            'app/components/tap/config', 'app/components/tap/viewModel'
        ]
    }
});

requirejs(['ko', 'app/bindings', 'app/componentResolver', 'app/dataService', 'app/camera'], function (ko, bindings, ComponentResolver, dataService) {
    bindings.init();

    var resolver = new ComponentResolver();
    resolver.register('pourcast', 'pourcast');
    resolver.register('tap', 'tap');

    dataService.getTaps().done(function (taps) {
        var viewModel = {
            taps: ko.observableArray(taps),
            resolver: resolver
        };

        ko.applyBindings(viewModel);
    });
});