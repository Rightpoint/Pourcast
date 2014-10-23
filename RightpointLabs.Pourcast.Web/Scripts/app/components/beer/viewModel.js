define(['ko', 'app/events', 'app/dataService', 'text!app/components/beer/template.html'], function (ko, events, dataService, htmlString) {
    
    function Beer(model) {
        var self = this;

        self.id = ko.observable(model.id);
        self.name = ko.observable(model.name);
        self.abv = ko.observable(model.abv);
        self.style = ko.observable(model.style);
        self.brewery = ko.observable(model.brewery);
    };

    return {
        viewModel: Beer,
        template: htmlString
    };
});