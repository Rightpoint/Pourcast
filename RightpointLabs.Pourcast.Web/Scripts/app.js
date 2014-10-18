requirejs.config({
    baseUrl: 'Scripts/libs/',
    //urlArgs: "bust=" + (new Date()).getTime(),
    paths: {
        app: '../app',
        jquery: 'jquery-2.1.0',
        TapViewModel: '../app/viewmodel/TapViewModel',
        ko: 'knockout-3.2.0',
        signalr: 'jquery.signalR-2.0.3',
        'signalr.hubs' : '/signalr/hubs?'
    },
    shim: {
        jquery : { exports: '$' },
        signalr: { deps: ['jquery'] },
        'signalr.hubs': { deps: ['signalr'] }
    }
});

requirejs(['ko', 'app/bindings', 'app/model/pourcast'], function (ko, bindings, Pourcast) {
    bindings.init();

    ko.applyBindings(new Pourcast());
});