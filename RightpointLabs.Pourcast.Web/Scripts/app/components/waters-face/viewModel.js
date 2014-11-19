define(['ko', 'app/events', 'app/dataService', 'text!app/components/waters-face/template.html'], function (ko, events, dataService, htmlString) {
    
    function WatersFace(model) {
        var self = this;

        self.isLow = ko.computed(function () {
            return ko.unwrap(model.isLow);
        });
    };

    return {
        viewModel: WatersFace,
        template: htmlString
    };
});