define('app/model/style', ['ko'], function (ko) {
    function Style(styleJSON) {
        var self = this;

        self.id = ko.observable(styleJSON.id);
        self.name = ko.observable(styleJSON.name);
        self.color = ko.observable(styleJSON.color);
        self.glass = ko.observable(styleJSON.glass);
    };

    return Style;
});