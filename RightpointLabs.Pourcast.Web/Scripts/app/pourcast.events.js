(function (app, $) {
    app.events = app.events || function () {
        $.connection.hub.start();

        var timeout = 0;

        // try reconnect if disconnected
        $.connection.eventsHub.disconnected(function() {
            setTimeout(function () {
                $.connection.eventsHub.start();
                timeout++;
            }, Math.pow(2, timeout) * 1000);
        });

        $.connection.hub.reconnected(function () {
            timeout = 0;
        });

        return {
            on: function (event, callback) {
                $.connection.eventsHub.on(event, callback);
            },
            off: function (event, callback) {
                $.connection.eventsHub.off(event, callback);
            }
        }
    }();

}(window.pourcast = window.pourcast || {}, jQuery));