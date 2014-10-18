define(['ko', 'text!/components/tap/template.html'], function (ko, htmlString) {
    
    function Tap(tap) {
        var self = this;
        $.extend(self, tap);

        self.tap = ko.observable(tap);
    };

    return {
        viewModel: Tap,
        template: htmlString
    };
});