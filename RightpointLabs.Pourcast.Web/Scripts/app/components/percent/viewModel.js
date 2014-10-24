define(['ko', 'app/events', 'app/dataService', 'text!app/components/percent/template.html'], function (ko, events, dataService, htmlString) {
    
    function Percent(model) {
        var self = this;

        self.percentRemaining = model.percentRemaining;

        self.isLow = ko.computed(function () {
            return self.percentRemaining() < 25;
        });

        self.percentRemainingHtml = ko.computed(function () {
            return parseFloat(self.percentRemaining()).toFixed(1) + '<span class="symbol">%</span>';
        });
        self.percentRemainingClass = ko.computed(function () {
            return self.isLow() ? "low" : "high";
        });
    };

    return {
        viewModel: Percent,
        template: htmlString
    };
});