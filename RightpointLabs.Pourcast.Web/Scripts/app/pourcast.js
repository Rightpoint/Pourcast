(function (app, $, ko, toast, moment) {
    var vm = {
        title : "Hello World"
    };
    vm.brewery = new app.Brewery("Goose Island", "Chicago", "IL", []);

    app.init = function() {
        ko.applyBindings(vm);
    };

}(window.pourcast = window.pourcast || {}, jQuery, ko, toastr, moment));