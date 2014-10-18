define(['ko', 'jquery', 'text!/components/pourcast/template.html'], function (ko, $, htmlString) {

    function Pourcast(pourcast) {
        var self = this;
        $.extend(self, pourcast);

        self.pourcast = ko.observable(pourcast);
    };

    return {
        viewModel: Pourcast,
        template: htmlString
    };
});