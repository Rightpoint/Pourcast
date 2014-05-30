requirejs.config({
    baseUrl: 'Scripts/libs/',
    paths: {
        app: '../app',
        jquery: 'jquery-2.1.0',
        TapViewModel: '../app/viewmodel/TapViewModel',
        ko: 'knockout-3.1.0',
        signalr: 'jquery.signalR-2.0.3',
        "signalr.hubs" : '../../signalr/hubs?'
    },
    shims: {
        jquery : { exports: "$" },
        signalr : { deps: ["jquery"] },
        "signalr.hubs": { deps: ['jquery', 'jquery.signalR-2.0.3'] }
    }
});

requirejs(['app/bindings', 'app/pourcast'], function (bindings, pourcast) {
    bindings.init();
    pourcast.init();
});