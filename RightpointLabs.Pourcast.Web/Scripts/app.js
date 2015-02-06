requirejs.config({
    baseUrl: 'Scripts/libs/',
    //urlArgs: "bust=" + (new Date()).getTime(),
    paths: {
        app: '../app',
        jquery: 'jquery-2.1.0',
        ko: 'knockout-3.2.0',
        signalr: 'jquery.signalR-2.0.3',
        'signalr.hubs': '/signalr/hubs?'
    },
    shim: {
        jquery: { exports: '$' },
        signalr: { deps: ['jquery'] },
        'signalr.hubs': { deps: ['signalr'] }
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