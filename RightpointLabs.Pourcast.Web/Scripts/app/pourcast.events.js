(function (app, $, ko, toast, moment) {
    app.events = app.events || function () {
        $.connection.hub.start();

        return {
            on: function (event, callback) {
                $.connection.eventsHub.on(event, callback);
            },
            off: function (event, callback) {
                $.connection.eventsHub.off(event, callback);
            }
        }
    }();

}(window.pourcast = window.pourcast || {}, jQuery, ko, toastr, moment));