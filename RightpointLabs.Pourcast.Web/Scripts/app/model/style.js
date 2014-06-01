define(['jquery', 'ko'], function ($, ko) {
    function Style(styleJSON, brewery) {
        var self = this;

        self.id = ko.observable(styleJSON.Id);
        self.name = ko.observable(styleJSON.Name);
        self.color = ko.observable(styleJSON.Color);
        self.glass = ko.observable(styleJSON.Glass);

        self.brewery = ko.observable(brewery);
    };

    return Style;
});