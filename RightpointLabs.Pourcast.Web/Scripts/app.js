requirejs.config({
    baseUrl: 'Scripts/libs/',
    paths: {
        app: '../app',
        jquery: 'jquery-2.1.0',
        BarViewModel: '../app/viewmodel/BarViewModel',
        ko: 'knockout-3.2.0.debug',
        signalr: 'jquery.signalR-2.0.3',
        'signalr.hubs' : '/signalr/hubs?'
    },
    shim: {
        jquery : { exports: '$' },
        signalr: { deps: ['jquery'] },
        'signalr.hubs': { deps: ['signalr'] }
    }
});

requirejs(['app/bindings', 'app/pourcast'], function (bindings, pourcast) {
    bindings.init();
    pourcast.init();
});