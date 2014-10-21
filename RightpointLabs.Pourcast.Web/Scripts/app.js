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

requirejs(['ko', 'app/bindings', 'app/componentResolver', 'app/dataService'], function (ko, bindings, componentResolver, dataService) {
    bindings.init();

    var resolver = new componentResolver();
    resolver.register('pourcast', 'pourcast');
    resolver.register('tap', 'tap');
    resolver.register('hat', 'tap');

    dataService.getTaps().done(function (taps) {
        var viewModel = {
            taps: taps,
            resolver: resolver
        };

        ko.applyBindings(viewModel);
    });
});