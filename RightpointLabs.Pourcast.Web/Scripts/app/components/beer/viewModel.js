define(['ko', 'app/events', 'app/dataservice', 'text!app/components/beer/template.html'], function (ko, events, dataService, htmlString) {
    
    function Beer(model) {
        var self = this;

        self.id = ko.observable(model.Id);
        self.name = ko.observable(model.Name);
        self.abv = ko.observable(model.ABV);
        self.style = ko.observable(model.Style);
        self.brewery = ko.observable(model.Brewery);
    };

    return {
        viewModel: Beer,
        template: htmlString
    };
});