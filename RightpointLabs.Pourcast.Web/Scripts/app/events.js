define(['jquery', 'toastr', 'signalr.hubs'], function ($, toastr, hub) {

    hub.start();
    $.connection.hub.start();

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
    pub.on("refresh", function() {
        window.location.reload();
    });

    return pub;
});