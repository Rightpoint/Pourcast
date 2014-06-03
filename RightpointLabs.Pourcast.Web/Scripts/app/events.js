define(['jquery', 'toastr', 'signalr.hubs'], function ($, toastr) {

    // in order to get the client to actually subscribe for events on a hub, we have to register at least one client-side call for that hub...
    // logging (the next line) will help show that if you need further details
    $.connection.eventsHub.client.dummyClientCallback = function () { };
    //$.connection.hub.logging = true;
    $.connection.hub.start({ waitForPageLoad: false });

    var pub = {
        on: function(event, callback) {
            $.connection.eventsHub.on(event, callback);
        },
        off: function(event, callback) {
            $.connection.eventsHub.off(event, callback);
        }
    };


    // reconnect
    var retryCount = 0;
    var isDisconnected = false;
    var disconnectedToast;

    $.connection.hub.stateChanged(function(e) {
        if (e.newState === $.connection.connectionState.connected) {
            if (isDisconnected) {
                toastr.clear(disconnectedToast);
                toastr.success("Reconnected");

                isDisconnected = false;
            }
        } else if (e.newState === $.connection.connectionState.disconnected) {
            if (!isDisconnected) {
                disconnectedToast = toastr.error("Disconnected", "", {
                    timeOut: 0,
                    extendedTimeOut: 0
                });

                retryCount = 0;
                isDisconnected = true;
            }
        }
    });

    $.connection.hub.disconnected(function() {
        setTimeout(function() {
            $.connection.hub.start();
        }, Math.pow(2, retryCount) * 1000);

        retryCount++;
    });


    // admin refresh
    pub.on("Refresh", function() {
        window.location.reload();
    });

    return pub;
});