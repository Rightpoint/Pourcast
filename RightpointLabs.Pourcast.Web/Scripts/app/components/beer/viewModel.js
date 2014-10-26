define(['ko', 'app/events', 'app/dataService', 'text!app/components/beer/template.html'], function (ko, events, dataService, htmlString) {
    
    function Beer(model) {
        var self = this;

        self.name = ko.computed(function () {
            return ko.unwrap(model.name);
        });
        self.brewery = ko.computed(function() {
            return ko.unwrap(model.brewery);
        });
        self.style = ko.computed(function () {
            return ko.unwrap(model.style);
        });
    };

    return {
        viewModel: Beer,
        template: htmlString
    };
});