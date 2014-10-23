define(['ko', 'app/events', 'app/dataService', 'text!app/components/face/template.html'], function (ko, events, dataService, htmlString) {
    
    function Face(model) {
        var self = this;

        self.isLow = model.isLow;
    };

    return {
        viewModel: Face,
        template: htmlString
    };
});