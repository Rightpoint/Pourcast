define(['jquery', 'ko'], function ($, ko) {

    ko.components.register('defaultBeer', {
        viewModel: { require: 'app/component/defaultBeer' },
        template: { require: 'text!app/component/defaultBeer.html' }
    });

    function Beer(beerModel) {
        console.log("Creating beer renderer");
        var self = this;

        self.isActive = ko.observable(true);
        self.component = "defaultBeer";
    };

    return Beer;
});