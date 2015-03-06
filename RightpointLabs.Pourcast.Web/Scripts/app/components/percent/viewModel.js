define('app/components/percent/viewModel', ['ko', 'app/events', 'app/dataService'], function (ko, events, dataService) {
    
    function Percent(model) {
        var self = this;

        self.percentRemaining = ko.computed(function() {
            return (ko.unwrap(model.percentRemaining) * 100).toFixed(1);
        });
        self.levelCssClass = ko.computed(function () {
            return ko.unwrap(model.isLow) ? "low" : "high";
        });
    };

    return {
        viewModel: Percent,
        template: { element: 'percent-template' }
    };
});