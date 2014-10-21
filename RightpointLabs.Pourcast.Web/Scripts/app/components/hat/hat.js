define(['jquery', 'ko', 'text!/components/hat/template.html'], function ($, ko, htmlString) {
    function Hat(params) {
        var self = this;

        self.params = params;
        self.isActive = ko.observable(true);
        self.someValue = ko.observable('omg hat!');
    };

    return {
        viewModel: Hat,
        template: htmlString
    };
});