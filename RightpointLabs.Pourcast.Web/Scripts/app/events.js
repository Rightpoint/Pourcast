define('app/events', ['jquery', 'toastr', 'signalr.hubs'], function ($, toastr) {

    var subscribers = {};

    // in order to get the client to actually subscribe for events on a hub, we have to register at least one client-side call for that hub...
    // logging (the next line) will help show that if you need further details
    $.connection.eventsHub.client.dummyClientCallback = function () { };
    //$.connection.hub.logging = true;
    $.connection.hub.start({ waitForPageLoad: false });

    var pub = {
        on: function(event, callback) {
            $.connection.eventsHub.on(event, callback);

            if (!subscribers[event]) {
                subscribers[event] = [];
            }

            subscribers[event].push(callback);
        },
        off: function(event, callback) {
            $.connection.eventsHub.off(event, callback);

            var eventSubscribers = subscribers[event];
            var index = eventSubscribers.indexOf(callback);

            if (index > -1) {
                eventSubscribers.splice(index, 1);
            }
        },
        raise: function(event, args) {
            subscribers[event].forEach(function(callback) {
                callback(args);
            });
        },
        send: function (event, args) {
            $.connection.eventsHub.server[event].apply($.connection.eventsHub.server, args);
        }
    };


    // reconnect
    var retryCount = 0;
    var wasDisconnected = false;
    var disconnectedToast;

    $.connection.hub.stateChanged(function(e) {
        var isConnected = e.newState === $.connection.connectionState.connected;
        var isDisconnected = e.newState === $.connection.connectionState.disconnected;

        if (isConnected) {
            if (wasDisconnected) {
                toastr.clear(disconnectedToast);
                toastr.success("Reconnected");
                pub.raise("Reconnected");

                wasDisconnected = false;
            }
        } else if (isDisconnected) {
            if (!wasDisconnected) {
                disconnectedToast = toastr.error("Disconnected", "", {
                    timeOut: 0,
                    extendedTimeOut: 0
                });

                retryCount = 0;
                wasDisconnected = true;
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