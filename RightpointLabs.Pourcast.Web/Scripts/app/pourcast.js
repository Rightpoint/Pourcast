(function (app, $, ko, toast, moment) {
    var vm = {
        title : "Hello World"
    };
   
    var brewery = new app.Brewery("Goose Island", "Chicago", "IL", []);
    brewery.beers.push(new app.Beer("Honker's Ale", 4.3, "http://cdn.beeradvocate.com/im/beers/1157.jpg", "English Bitter", "Pint"));
    
    vm.brewery = new app.BreweryVM([brewery]);
    app.init = function() {
        ko.applyBindings(vm);
        toast.success(vm.title);
    };

}(window.pourcast = window.pourcast || {}, jQuery, ko, toastr, moment));